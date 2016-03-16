using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using Expenses.BL.Entities;
using Expenses.BL.Service;

namespace Expenses.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private IExpensesService m_service = new ExpensesService("name=Expenses");

        // GET: Course
        public ActionResult Index()
        {
            return View(m_service.Currencies.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = m_service.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Currency currency)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_service.Currencies.Add(currency);
                    m_service.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(currency);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = m_service.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currencyToUpdate = m_service.Currencies.Find(id);
            if (TryUpdateModel(currencyToUpdate, "",
               new string[] { "ShortName", "Name", "Comment" }))
            {
                try
                {
                    m_service.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(currencyToUpdate);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = m_service.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var currency = m_service.Currencies.Find(id);
            m_service.Currencies.Remove(currency);
            m_service.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                m_service.Dispose();
            base.Dispose(disposing);
        }
    }
}
