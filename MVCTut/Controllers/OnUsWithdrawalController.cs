using Core;
using Data.Repository;
using System;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class OnUsWithdrawalController : Controller
    {
        UnitOfWork unitofwork = new UnitOfWork();
        // GET: OnUsWithdrawal
        public ActionResult Index()
        {

            var ouwSetups = unitofwork.EntityRepository<OnUsWithdrawalSetup>().GetAll();
            try
            {
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(ouwSetups);
        }

        // GET: OnUsWithdrawal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OnUsWithdrawal/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: OnUsWithdrawal/Create
        [HttpPost]
        public ActionResult Create(OnUsWithdrawalSetup onUswithdrawal)
        {
            try
            {
                // TODO: Add insert logic here
                var setups = unitofwork.EntityRepository<OnUsWithdrawalSetup>().Filter(c => c.Name == onUswithdrawal.Name);

                if (setups.Count == 0)
                {
                    onUswithdrawal.DateAdded = DateTime.Now;
                    onUswithdrawal.DateUpdated = DateTime.Now;
                    unitofwork.EntityRepository<OnUsWithdrawalSetup>().Save(onUswithdrawal);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", "A terminal with that Name exists.");
            }
            catch
            {

            }
            return View();
        }

        // GET: OnUsWithdrawal/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OnUsWithdrawal/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, OnUsWithdrawalSetup onUsSetup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    onUsSetup.DateUpdated = DateTime.Now;
                    unitofwork.EntityRepository<OnUsWithdrawalSetup>().Update(onUsSetup, onUsSetup.Id);
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            return View(onUsSetup);
        }

        // GET: OnUsWithdrawal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OnUsWithdrawal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
