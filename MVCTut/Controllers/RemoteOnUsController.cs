using Core;
using Data.Repository;
using System;
using System.Text;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class RemoteOnUsController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET: RemoteOnUs
        public ActionResult Index()
        {
            var setups = unitofwork.EntityRepository<RemoteOnUs>().GetAll();
            try
            {
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(setups);
        }

        // GET: OnUsWithdrawal/Create
        public ActionResult Create()
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            return View();
        }

        // POST: OnUsWithdrawal/Create
        [HttpPost]
        public ActionResult Create(RemoteOnUs remoteSetup)
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            try
            {
                // TODO: Add insert logic here
                var setups = unitofwork.EntityRepository<RemoteOnUs>().Filter(c => c.Name == remoteSetup.Name ||
                c.GlAccount.GLAccountName == remoteSetup.GlAccount.GLAccountName);

                if (setups.Count == 0)
                {
                    remoteSetup.GlAccount.GLCode = GenerateGLAccountCode(remoteSetup.GlAccount, remoteSetup.GlAccount.Id);
                    remoteSetup.DateAdded = DateTime.Now;
                    remoteSetup.DateUpdated = DateTime.Now;
                    unitofwork.EntityRepository<GLAccount>().Save(remoteSetup.GlAccount);
                    unitofwork.EntityRepository<RemoteOnUs>().Save(remoteSetup);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", " Name exists.");
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            return View(unitofwork.EntityRepository<RemoteOnUs>().GetById(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, RemoteOnUs remoteSetup)
        {
            ViewBag.Catergories = unitofwork.EntityRepository<GLCategory>().GetAll();
            ViewBag.Branches = unitofwork.EntityRepository<Branch>().GetAll();
            // if (ModelState.IsValid)
            {
                try
                {

                    var oldEntity = unitofwork.EntityRepository<RemoteOnUs>().GetById(remoteSetup.Id);
                    remoteSetup.GlAccount.GLCode = oldEntity.GlAccount.GLCode;
                    remoteSetup.GlAccount.DateUpdated = DateTime.Now;
                    remoteSetup.GlAccount.DateAdded = oldEntity.GlAccount.DateAdded;
                    remoteSetup.DateAdded = oldEntity.DateAdded;
                    remoteSetup.DateUpdated = DateTime.Now;
                    unitofwork.EntityRepository<GLAccount>().Update(remoteSetup.GlAccount, remoteSetup.GlAccount.Id);
                    unitofwork.EntityRepository<RemoteOnUs>().Update(remoteSetup, remoteSetup.Id);

                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            return View(remoteSetup);
        }

        private string GenerateGLAccountCode(GLAccount glCode, int Id)
        {
            StringBuilder builder = new StringBuilder();
            var a = glCode.GLCategory.CategoryType;
            Random r = new Random();
            switch (a)
            {
                case GLCategoryType.Asset:
                    int assetKey = 1;
                    builder = builder.Append(assetKey);
                    break;
                case GLCategoryType.Liability:
                    int liabilityKey = 2;
                    builder = builder.Append(liabilityKey);
                    break;
                case GLCategoryType.Capital:
                    int capKey = 3;
                    builder = builder.Append(capKey);
                    break;
                case GLCategoryType.Income:
                    int IncKey = 4;
                    builder = builder.Append(IncKey);
                    break;
                case GLCategoryType.Expense:
                    int expKey = 5;
                    builder = builder.Append(expKey);
                    break;
            }
            builder.Append(Id).Append(0);
            int length = 10 - builder.Length;
            for (int i = 0; i < length; i++)
            {
                builder.Append(r.Next(0, 9));
            }
            return builder.ToString();
        }
    }
}