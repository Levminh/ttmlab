using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;

namespace ttm3._0.Controllers
{
    public class tbProjectOpenStacksController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbProjectOpenStacks
        public ActionResult Index()
        {
            return View(db.tbProjectOpenStacks.ToList());
        }

        // GET: tbProjectOpenStacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProjectOpenStack tbProjectOpenStack = db.tbProjectOpenStacks.Find(id);
            if (tbProjectOpenStack == null)
            {
                return HttpNotFound();
            }
            return View(tbProjectOpenStack);
        }

        // GET: tbProjectOpenStacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbProjectOpenStacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProject,ProjectName,ProjectId")] tbProjectOpenStack tbProjectOpenStack)
        {
            if (ModelState.IsValid)
            {
                db.tbProjectOpenStacks.Add(tbProjectOpenStack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbProjectOpenStack);
        }

        // GET: tbProjectOpenStacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProjectOpenStack tbProjectOpenStack = db.tbProjectOpenStacks.Find(id);
            if (tbProjectOpenStack == null)
            {
                return HttpNotFound();
            }
            return View(tbProjectOpenStack);
        }

        // POST: tbProjectOpenStacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProject,ProjectName,ProjectId")] tbProjectOpenStack tbProjectOpenStack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbProjectOpenStack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbProjectOpenStack);
        }

        // GET: tbProjectOpenStacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProjectOpenStack tbProjectOpenStack = db.tbProjectOpenStacks.Find(id);
            if (tbProjectOpenStack == null)
            {
                return HttpNotFound();
            }
            return View(tbProjectOpenStack);
        }

        // POST: tbProjectOpenStacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbProjectOpenStack tbProjectOpenStack = db.tbProjectOpenStacks.Find(id);
            db.tbProjectOpenStacks.Remove(tbProjectOpenStack);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult vnc(int? IdProject)
        {
            if (!IdProject.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbGiamSat> lstCOM = db.tbGiamSats.Where(p => p.IdProject == IdProject).ToList();
            double sl = lstCOM.Count / (double)3;
            if (sl % 1 != 0)
                sl = sl + 1;
            ViewBag.SoLuong = (int)sl;
            ViewBag.Count = lstCOM.Count;
            int dem = 0;
            foreach (tbGiamSat com in lstCOM.OrderBy(p => p.Id))
            {
                com.TT = dem++;
            }
            return View(lstCOM.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
