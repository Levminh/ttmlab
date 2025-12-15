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
    public class tbGiamSatsController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbGiamSats
        public ActionResult Index(int? id)
        {
            if(id==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbProjectOpenStack pro = db.tbProjectOpenStacks.Find(id);
            if(pro==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbGiamSat> tbGiamSats = pro.tbGiamSats.ToList();
            ViewBag.id = id;
            return View(tbGiamSats.ToList());
        }

        // GET: tbGiamSats/Create
        public ActionResult Create(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbGiamSat gs = new tbGiamSat();
            gs.IdProject = id;
            return View(gs);
        }

        // POST: tbGiamSats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ten,Link,IdProject")] tbGiamSat tbGiamSat)
        {
            if (ModelState.IsValid)
            {
                db.tbGiamSats.Add(tbGiamSat);
                db.SaveChanges();
                return RedirectToAction("Index",new { id = tbGiamSat.IdProject });
            }

            ViewBag.IdProject = new SelectList(db.tbProjectOpenStacks, "IdProject", "ProjectName", tbGiamSat.IdProject);
            return View(tbGiamSat);
        }

        // GET: tbGiamSats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGiamSat tbGiamSat = db.tbGiamSats.Find(id);
            if (tbGiamSat == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProject = new SelectList(db.tbProjectOpenStacks, "IdProject", "ProjectName", tbGiamSat.IdProject);
            return View(tbGiamSat);
        }

        // POST: tbGiamSats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ten,Link,IdProject")] tbGiamSat tbGiamSat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbGiamSat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { id=tbGiamSat.IdProject});
            }
            ViewBag.IdProject = new SelectList(db.tbProjectOpenStacks, "IdProject", "ProjectName", tbGiamSat.IdProject);
            return View(tbGiamSat);
        }

        // GET: tbGiamSats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGiamSat tbGiamSat = db.tbGiamSats.Find(id);
            if (tbGiamSat == null)
            {
                return HttpNotFound();
            }
            return View(tbGiamSat);
        }

        // POST: tbGiamSats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbGiamSat tbGiamSat = db.tbGiamSats.Find(id);
            int? id2 = tbGiamSat.IdProject;
            db.tbGiamSats.Remove(tbGiamSat);
            db.SaveChanges();
            return RedirectToAction("Index",new {id=id2 });
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
