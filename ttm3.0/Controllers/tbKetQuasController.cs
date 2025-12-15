using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;

namespace ttm3._0.Controllers
{
    public class tbKetQuasController : Controller
    {
        private dbttm db = new dbttm();
        
        public ActionResult ctf(int? IdKetQua)
        {
            if(IdKetQua==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKetQua kq = db.tbKetQuas.Find(IdKetQua);
            if(kq==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.Title = "Submit "+kq.tbKichBan.TenKichBan;
            return View(kq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ctf(tbKetQua model)
        {
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKetQua kq = db.tbKetQuas.Find(model.IdKetQua);
            if(kq==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBan kb = db.tbKichBans.Find(model.IdKichBan);
            if(kb==null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if(kb.DapAn==model.DapAn)
            {
                ModelState.AddModelError(string.Empty, "Đáp án chính xác!");
                kq.Diem = 10;
                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đáp án không đúng!");
                kq.DapAn = model.DapAn;
                db.SaveChanges();
            }
            
            ViewBag.Title = "Submit " + kq.tbKichBan.TenKichBan;
            return View(kq);
        }

        public ActionResult UploadFiles(int? IdKetQua)
        {
            if (IdKetQua == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKetQua kq = db.tbKetQuas.Find(IdKetQua);
            if (kq == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.Title = "Submit " + kq.tbKichBan.TenKichBan;
            FileModel model = new FileModel();
            model.Id = kq.IdKetQua;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(FileModel model)
        {
            if (ModelState.IsValid)
            {
                string folder = Server.MapPath("~/UploadedFiles/") + model.Id.ToString();
                List<string> lstFileExit = new List<string>();
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                if (model.files != null)
                {
                    var InputFileName = Path.GetFileName(model.files.FileName);
                    var ServerSavePath = Path.Combine(folder + @"/" + InputFileName);
                    model.files.SaveAs(ServerSavePath);
                    tbKetQua kq = db.tbKetQuas.Find(model.Id);                  
                    kq.UrlFileDapAn = "/UploadedFiles/" + model.Id.ToString() + @"/" + InputFileName;
                    ViewBag.Title = "Submit " + kq.tbKichBan.TenKichBan;
                    db.SaveChanges();
                    ModelState.AddModelError(string.Empty, "Upload thành công!");
                }else
                {
                    ModelState.AddModelError(string.Empty, "File rỗng!");
                }
            }
            return View(model);
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
