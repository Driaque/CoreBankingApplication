using Core;
using Data;
using Data.Repository;
using NHibernate;
using NHibernate.Linq;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class CustomerController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        // GET: Customer
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            // ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var customers = unitOfWork.EntityRepository<Customer>().GetAll();
            try
            {
                using (ISession session = NHibernateHelper.Session)
                {

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        customers = session.Query<Customer>().Where(u => u.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                        u.FirstName.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            customers = customers.OrderByDescending(a => a.LastName).ToList();
                            break;
                        case "id_desc":
                            customers = customers.OrderByDescending(a => a.Id).ToList();
                            break;
                        default:
                            customers = customers.OrderBy(u => u.Id).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(customers.ToPagedList(pageNumber, pageSize));
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new Customer());
        }

        public ActionResult Create()
        {
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            return View(new Customer());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customers = unitOfWork.EntityRepository<Customer>().Filter(c => c.Email == customer.Email);

                    if (customers.Count == 0)
                    {
                        customer.DateAdded = DateTime.Now;
                        customer.DateUpdated = DateTime.Now;
                        unitOfWork.EntityRepository<Customer>().Save(customer);
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("Email", "The email exists.");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception.Message;
                }

            }
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            return View();
        }

        //
        public ActionResult Edit(int id)
        {
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            return View(unitOfWork.EntityRepository<Customer>().GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldentity = unitOfWork.EntityRepository<Customer>().GetById(id);
                    customer.DateAdded = oldentity.DateAdded;
                    customer.DateUpdated = DateTime.Now;
                    unitOfWork.EntityRepository<Customer>().Update(customer, customer.Id);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            ViewBag.Genders = Enum.GetValues(typeof(Gender));

            return View(customer);
        }


        public ActionResult Details(int id)
        {
            return View(unitOfWork.EntityRepository<Customer>().GetById(id));

        }

    }
}