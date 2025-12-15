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
    public class tbKichBansController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbKichBans
        public ActionResult Index()
        {
            var tbKichBans = db.tbKichBans;
            return View(tbKichBans.ToList());
        }
        public ActionResult Report()
        {
            var tbKichBans = db.tbKichBans;
            List<clThongKeKichBan> lstTK = new List<clThongKeKichBan>();
            foreach(var kb in tbKichBans)
            {
                clThongKeKichBan tk = new clThongKeKichBan();
                lstTK.Add(tk);
                tk.tbKichBan = kb;
                int solan = kb.tbKetQuas.Count;
                if (solan == 0)
                {
                    tk.DiemTB = 0;
                    continue;
                }
                double? tongDiem = kb.tbKetQuas.Sum(o => o.Diem);
                tk.DiemTB = tongDiem / solan;
            }
            return View(lstTK.OrderByDescending(p=>p.DiemTB));
        }
        // GET: tbKichBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbKichBan tbKichBan = db.tbKichBans.Find(id);
            if (tbKichBan == null)
            {
                return HttpNotFound();
            }
            return View(tbKichBan);
        }

        // GET: tbKichBans/Create
        public ActionResult Create()
        {
            ViewBag.IdKieuDanhGia = new SelectList(db.tbKieuDanhGias, "IdKieuDanhGia", "KieuDanhGia");
            ViewBag.Nhom = new MultiSelectList(db.tbNhoms, "IdNhom", "Nhom");
            return View();
        }

        // POST: tbKichBans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tbKichBan tbKichBan)
        {
            if (ModelState.IsValid)
            {
                db.tbKichBans.Add(tbKichBan);
                foreach (int idNhom in tbKichBan.Nhom)
                {

                    tbKichBanNhom tmp = new tbKichBanNhom();
                    tmp.IdKichBan = tbKichBan.IdKichBan;
                    tmp.IdNhom = idNhom;
                    db.tbKichBanNhoms.Add(tmp);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKieuDanhGia = new SelectList(db.tbKieuDanhGias, "IdKieuDanhGia", "KieuDanhGia",tbKichBan.IdKieuDanhGia);
            ViewBag.Nhom = new MultiSelectList(db.tbNhoms, "IdNhom", "Nhom",tbKichBan.Nhom);
            return View(tbKichBan);
        }

        // GET: tbKichBans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbKichBan tbKichBan = db.tbKichBans.Find(id);
            if (tbKichBan == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKieuDanhGia = new SelectList(db.tbKieuDanhGias, "IdKieuDanhGia", "KieuDanhGia",tbKichBan.IdKieuDanhGia);
            ViewBag.Nhom = new MultiSelectList(db.tbNhoms, "IdNhom", "Nhom", tbKichBan.GetNhom);
            return View(tbKichBan);
        }

        // POST: tbKichBans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbKichBan tbKichBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbKichBan).State = EntityState.Modified;               
                foreach (int IdNhom in tbKichBan.Nhom)
                {
                    tbKichBanNhom nhom = db.tbKichBanNhoms.Where(p =>p.IdKichBan==tbKichBan.IdKichBan && p.IdNhom == IdNhom).FirstOrDefault();
                    if (nhom == null)
                    {
                        tbKichBanNhom tmp = new tbKichBanNhom();
                        tmp.IdKichBan = tbKichBan.IdKichBan;
                        tmp.IdNhom = IdNhom;
                        db.tbKichBanNhoms.Add(tmp);
                    }
                }
                List<tbKichBanNhom> lstXoa = new List<tbKichBanNhom>();
                foreach(tbKichBanNhom nhom in db.tbKichBanNhoms.Where(p=>p.IdKichBan==tbKichBan.IdKichBan).ToList())
                {
                    if (tbKichBan.Nhom.Where(p => p == nhom.IdNhom).Count() == 0)
                        lstXoa.Add(nhom);
                }
                if(lstXoa.Count>0)
                    db.tbKichBanNhoms.RemoveRange(lstXoa);
                db.SaveChanges();              
                return RedirectToAction("Index");
            }
            ViewBag.IdKieuDanhGia = new SelectList(db.tbKieuDanhGias, "IdKieuDanhGia", "KieuDanhGia", tbKichBan.IdKieuDanhGia);
            ViewBag.Nhom = new MultiSelectList(db.tbNhoms, "IdNhom", "Nhom", tbKichBan.Nhom);
            return View(tbKichBan);
        }

        // GET: tbKichBans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbKichBan tbKichBan = db.tbKichBans.Find(id);
            if (tbKichBan == null)
            {
                return HttpNotFound();
            }
            return View(tbKichBan);
        }

        // POST: tbKichBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbKichBan tbKichBan = db.tbKichBans.Find(id);
            db.tbKichBans.Remove(tbKichBan);
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
