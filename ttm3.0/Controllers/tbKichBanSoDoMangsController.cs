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
    public class tbKichBanSoDoMangsController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbKichBanSoDoMangs
        public ActionResult Index(int? IdKichBan)
        {
            if(IdKichBan==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBan tbkichBan = db.tbKichBans.Find(IdKichBan);
            if(tbkichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.TieuDe = "Danh sách sơ đồ mạng trong kịch bản: " + tbkichBan.TenKichBan;
            var tbKichBanSoDoMangs = db.tbKichBanSoDoMangs.Include(t => t.tbKichBan).Include(t => t.tbSoDoMang).Where(p=>p.IdKichBan==IdKichBan).ToList();
            ViewBag.IdKichBan = IdKichBan;
            return View(tbKichBanSoDoMangs.ToList());
        }       

        // GET: tbKichBanSoDoMangs/Create
        public ActionResult Create(int? IdKichBan)
        {
            if (IdKichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBanSoDoMang model = new tbKichBanSoDoMang();
            ViewBag.IdKichBan = IdKichBan;
            model.IdKichBan = IdKichBan;
            db.tbKichBanSoDoMangs.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdKichBan = IdKichBan });
        }            

        // GET: tbKichBanSoDoMangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbKichBanSoDoMang tbKichBanSoDoMang = db.tbKichBanSoDoMangs.Find(id);
            if (tbKichBanSoDoMang == null)
            {
                return HttpNotFound();
            }
            int? IdKichBan = tbKichBanSoDoMang.IdKichBan;
            ViewBag.IdKichBan = IdKichBan;
            db.tbKichBanSoDoMangs.Remove(tbKichBanSoDoMang);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdKichBan = IdKichBan });
        }
        public JsonResult LoadSoDoMangAsync()
        {
            List<string> lstS = new List<string>();
            lstS.Add(string.Format("\"{0}\":\"{1}\"", "0", "--Chọn--"));
            foreach (tbSoDoMang com in db.tbSoDoMangs.ToList())
            {
                lstS.Add(string.Format("\"{0}\":\"{1}\"", com.Id.ToString(), com.Ten));
            }
            return Json("{" + string.Join(",", lstS.ToArray()) + "}", JsonRequestBehavior.AllowGet);
        }
        public string UpdateSoDoMang(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int idKichBanSoDoMang;
            int IdSoDoMang;
            if (!int.TryParse(id, out idKichBanSoDoMang)) return "";
            if (!int.TryParse(value, out IdSoDoMang)) return "";
            tbKichBanSoDoMang kichbansodo = db.tbKichBanSoDoMangs.Find(idKichBanSoDoMang);
            if (kichbansodo == null) return "";
            if (IdSoDoMang==0)
            {
                kichbansodo.IdSoDoMang = null;
                db.SaveChanges();
                return "";
            }
            tbSoDoMang sodo = db.tbSoDoMangs.Find(IdSoDoMang);                   
            if (sodo == null) return "";
            kichbansodo.IdSoDoMang = IdSoDoMang;
            db.SaveChanges();
            return sodo.Ten;
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
