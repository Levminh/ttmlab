using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;

namespace ttm3._0.Controllers
{
    public class tbLichPhongLabsController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbLichPhongLabs
        public ActionResult Index(int? IdProject)
        {
            if(IdProject==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var tbLichPhongLabs = db.tbLichPhongLabs.Where(p=>p.IdProject==IdProject).Include(t => t.AspNetUser).Include(t => t.tbProjectOpenStack);
            ViewBag.IdProject = IdProject;
            return View(tbLichPhongLabs.ToList());
        }

        // GET: tbLichPhongLabs/Create
        public ActionResult Create(int? IdProject)
        {
            if (IdProject == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Username");
            ViewBag.IdProject = IdProject;
            tbLichPhongLab model = new tbLichPhongLab();
            model.IdProject = IdProject;
            return View(model);
        }
        public DateTime? ConvertToDatetime(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            string[] formats = { "H:mm dd/MM/yyyy", "H:mm dd/M/yyyy", "H:mm d/M/yyyy", "H:mm d/MM/yyyy",
                    "H:mm dd/MM/yy", "H:mm dd/M/yy", "H:mm d/M/yy", "H:mm d/MM/yy"};
            DateTime dt = new DateTime();
            if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }
        // POST: tbLichPhongLabs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbLichPhongLab tbLichPhongLab)
        {
            if (ModelState.IsValid)
            {
                tbLichPhongLab.TuNgay = ConvertToDatetime(tbLichPhongLab.TuGio + " " + tbLichPhongLab.stTuNgay);
                tbLichPhongLab.DenNgay = ConvertToDatetime(tbLichPhongLab.DenGio + " " + tbLichPhongLab.stDenNgay);
                if(tbLichPhongLab.TuNgay>=tbLichPhongLab.DenNgay)
                {
                    ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Username", tbLichPhongLab.IdUser);
                    ViewBag.Loi = "Thời gian kết kết thúc phải lớn hơn thời gian bắt đầu";
                    return View(tbLichPhongLab);
                }
                ITimePeriodCollection periods = new TimePeriodCollection();
                List<tbLichPhongLab> lstLich = db.tbLichPhongLabs.Where(p => p.IdProject == tbLichPhongLab.IdProject).ToList();
                foreach(tbLichPhongLab l in lstLich)
                {
                    periods.Add(new TimeRange(l.TuNgay.Value,l.DenNgay.Value));
                }
                TimeRange searchRange = new TimeRange(tbLichPhongLab.TuNgay.Value, tbLichPhongLab.DenNgay.Value);

                foreach (TimeRange period in periods)
                {
                    if (period.IntersectsWith(searchRange))
                    {
                        ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Username", tbLichPhongLab.IdUser);
                        ViewBag.Loi = "Thời gian bạn vừa đặt đã có 1 lịch khác! Vui lòng kiểm tra lại!";
                        return View(tbLichPhongLab);
                    }
                }
                db.tbLichPhongLabs.Add(tbLichPhongLab);             
                db.SaveChanges();
                return RedirectToAction("Index",new { IdProject = tbLichPhongLab.IdProject });
            }
            ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Username", tbLichPhongLab.IdUser);
            return View(tbLichPhongLab);
        }

        // GET: tbLichPhongLabs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbLichPhongLab tbLichPhongLab = db.tbLichPhongLabs.Find(id);
            if (tbLichPhongLab == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Email", tbLichPhongLab.IdUser);
            ViewBag.IdProject = new SelectList(db.tbProjectOpenStacks, "IdProject", "ProjectName", tbLichPhongLab.IdProject);
            return View(tbLichPhongLab);
        }

        // POST: tbLichPhongLabs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdProject,IdUser,TuNgay,DenNgay")] tbLichPhongLab tbLichPhongLab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbLichPhongLab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Email", tbLichPhongLab.IdUser);
            ViewBag.IdProject = new SelectList(db.tbProjectOpenStacks, "IdProject", "ProjectName", tbLichPhongLab.IdProject);
            return View(tbLichPhongLab);
        }

        // GET: tbLichPhongLabs/Delete/5
        public ActionResult Delete(int? id)
        {
            tbLichPhongLab tbLichPhongLab = db.tbLichPhongLabs.Find(id);
            if(tbLichPhongLab==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? IdProject = tbLichPhongLab.IdProject;
            db.tbLichPhongLabs.Remove(tbLichPhongLab);
            db.SaveChanges();
            return RedirectToAction("Index",new { IdProject = IdProject });
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
