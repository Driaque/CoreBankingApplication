using Core;
using Data;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class GLCategoryController : Controller
    {
        public ActionResult Index()
        {
            var glCategories = new List<GLCategory>();

            try
            {
                using (ISession session = NHibernateHelper.Session)
                {
                    glCategories = session.Query<GLCategory>().ToList();
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(glCategories);

        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.GlCategoryTypes = Enum.GetValues(typeof(GLCategoryType));

            return View(new GLCategory());
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(GLCategory glCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ISession session = NHibernateHelper.Session)
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            glCategory.DateAdded = DateTime.Now;
                            glCategory.DateUpdated = DateTime.Now;
                            session.Save(glCategory);

                            transaction.Commit();
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {

                }
            }

            ViewBag.GlCategoryTypes = Enum.GetValues(typeof(GLCategoryType));

            return View(glCategory);
        }

        public ActionResult Edit(int id)
        {
            var category = new Repository<GLCategory>();
            ViewBag.GlCategoryTypes = Enum.GetValues(typeof(GLCategoryType));
            return View(category.GetById(id));
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, GLCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var glRepository = new Repository<GLCategory>();
                    var OldEntity = glRepository.GetById(id);
                    category.DateAdded = OldEntity.DateAdded;
                    category.DateUpdated = DateTime.Now;
                    glRepository.Update(category, category.Id);

                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.Exception = exception;
                }
            }
            ViewBag.GlCategoryTypes = Enum.GetValues(typeof(GLCategoryType));
            return View(category);
        }

        public ActionResult Details(int id)
        {
            using (ISession session = NHibernateHelper.Session)
            {
                var category = new Repository<GLCategory>();
                return View(category.GetById(id));
            }
        }

    }
}