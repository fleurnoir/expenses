using System;
using Expenses.Web.Common;
using System.Web.Mvc;
using Expenses.BL.Service;
using System.Net;
using System.Data.Entity.Infrastructure;
using Expenses.BL.Entities;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web
{
    [CustomActionFilter]
    public abstract class ExpensesControllerBase<TEntity, TView> : Controller where TEntity : Entity, new() where TView : Entity, new()
    {
        private IExpensesService m_service;

        protected IExpensesService Service { get { return m_service ?? (m_service = Services.Get<IExpensesService> ()); } }

        protected virtual IEntityService<TView> EntityService { get { return new EntityViewService<TEntity,TView> (Service.GetEntityService<TEntity> ()); } }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currency = FillUpViewItem(EntityService.Select((long)id));
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        protected abstract IEnumerable<TView> FillUpViewItems(IEnumerable<TView> items);

        protected abstract TView FillUpViewItem(TView item);

        protected virtual void PopulateSelectLists(TView item) {}

        public virtual ActionResult Create()
        {
            PopulateSelectLists (null);
            return View();
        }

        protected virtual void OnSaving(TView item){
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TView item)
        {
            PopulateSelectLists (item);
            try
            {
                if (ModelState.IsValid)
                {
                    OnSaving(item);
                    EntityService.Add (item);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(item);
        }

        public virtual ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = EntityService.Select((long)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            PopulateSelectLists (item);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(TView item)
        {
            PopulateSelectLists (item);
            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                OnSaving(item);
                EntityService.Update(item);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(item);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = FillUpViewItem(EntityService.Select((long)id));
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try {
                EntityService.Delete(id);
            }
            catch(Exception e) {
                ModelState.AddModelError (String.Empty, e.Message);
                return Delete(id);
            }
            return RedirectToAction("Index");
        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && m_service != null)
//                m_service.Dispose();
//            base.Dispose(disposing);
//        }
    }

    public abstract class ExpensesController<TEntity, TView> : ExpensesControllerBase<TEntity,TView> where TEntity : Entity, new() where TView : Entity, new()
    {
        public virtual ActionResult Index()
        {
            return View(FillUpViewItems(EntityService.Select()));
        }
    }

    public class ExpensesController<TEntity> : ExpensesController<TEntity, TEntity> where TEntity : Entity, new()
    {
        protected override IEnumerable<TEntity> FillUpViewItems (IEnumerable<TEntity> entities) => entities;

        protected override TEntity FillUpViewItem (TEntity entity) => entity;

        protected override IEntityService<TEntity> EntityService {
            get {
                return Service.GetEntityService<TEntity> ();
            }
        }
    }
}

