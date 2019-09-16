using Core;
using Data.Repository;
using System;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class BranchController : Controller
    {
        // GET: Branch
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var branches = unitOfWork.EntityRepository<Branch>().GetAll();
            try
            {
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(branches);

        }

        public ActionResult Create()
        {

            ViewBag.branchStat = Enum.GetValues(typeof(Status));
            Session["BranchCount"] = unitOfWork.EntityRepository<Branch>().GetAll().Count;

            return View(new Branch());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Branch bran)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var branches = unitOfWork.EntityRepository<Branch>().Filter(c => c.Name == bran.Name);

                    if (branches.Count == 0)
                    {
                        bran.DateAdded = DateTime.Now;
                        bran.DateUpdated = DateTime.Now;
                        unitOfWork.EntityRepository<Branch>().Save(bran);

                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("Name", "This Branch name already exists.");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception.Message;
                }
            }
            ViewBag.branchStat = Enum.GetValues(typeof(Status));


            return View();
        }
        public ActionResult Edit(int id)
        {
            var bran = unitOfWork.EntityRepository<Branch>().GetById(id);
            ViewBag.branchStat = Enum.GetValues(typeof(Status));
            return View(bran);

        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Branch bran)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bran.DateUpdated = DateTime.Now;
                    unitOfWork.EntityRepository<Branch>().Update(bran, bran.branchID);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }

            ViewBag.branchStat = Enum.GetValues(typeof(Status));
            return View(bran);
        }


        public ActionResult Details(int id)
        {
            return View(unitOfWork.EntityRepository<Branch>().GetById(id));
        }


    }
}