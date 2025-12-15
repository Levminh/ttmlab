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
    [Authorize(Roles = "Admin,Teacher")]
    public class tbSoDoMangsController : Controller
    {
        private dbttm db = new dbttm();
        public ActionResult Index()
        {
            var tbSoDoMangs = db.tbSoDoMangs.ToList();
            return View(tbSoDoMangs.ToList());
        }
        public string UpdateTen(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMang kq = db.tbSoDoMangs.Find(Id);
                if (kq == null) return "";
                kq.Ten = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }

        public string UpdateUUID(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMang kq = db.tbSoDoMangs.Find(Id);
                if (kq == null) return "";
                kq.UUID = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }

        public string UpdatePath(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMang kq = db.tbSoDoMangs.Find(Id);
                if (kq == null) return "";
                kq.Path = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }
        public ActionResult Create()
        {
            tbSoDoMang model = new tbSoDoMang();
            db.tbSoDoMangs.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }      

        // GET: tbSoDoMangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSoDoMang tbSoDoMang = db.tbSoDoMangs.Find(id);
            if (tbSoDoMang == null)
            {
                return HttpNotFound();
            }
            return View(tbSoDoMang);
        }

        // POST: tbSoDoMangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            tbSoDoMang tbSoDoMang = db.tbSoDoMangs.Find(id);
            db.tbSoDoMangs.Remove(tbSoDoMang);
            db.SaveChanges();
            return RedirectToAction("Index");
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
