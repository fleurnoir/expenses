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
using Expenses.Web.DAL;
using Expenses.Common.Utils;

namespace Expenses.Web.Controllers
{
    [CustomActionFilter]
    public class CurrencyController : Controller
    {
        private IExpensesService m_service;

        private IExpensesService Service => m_service ?? (m_service = Services.Get<IExpensesService>());


        // GET: Course
        public ActionResult Index()
        {
            return View(Service.GetCurrencies());
        }

        // GET: Course/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = Service.GetCurrency((long)id);
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
                    Service.AddCurrency (currency);
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(currency);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = Service.GetCurrency((long)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currencyToUpdate = new Currency() {Id = (long)id};
            if (TryUpdateModel(currencyToUpdate, "",
               new string[] { "ShortName", "Name", "Comment" }))
            {
                try
                {
                    Service.UpdateCurrency(currencyToUpdate);
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
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = Service.GetCurrency((long)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Service.DeleteCurrency(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && m_service != null)
                m_service.Dispose();
            base.Dispose(disposing);
        }
    }
}
