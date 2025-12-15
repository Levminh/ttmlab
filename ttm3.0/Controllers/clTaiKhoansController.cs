using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;

namespace ttm3._0.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class clTaiKhoansController : Controller
    {
        private dbttm db = new dbttm();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public clTaiKhoansController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: clTaiKhoans/Create
        public ActionResult Create(int? IdLopHoc)
        {
            if(!IdLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            clTaiKhoan tk = new clTaiKhoan();
            tk.IdLop = IdLopHoc.Value;
            return View(tk);
        }

        // POST: clTaiKhoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(clTaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                int demThanhCong = 0, demLoi=0;
                for(int i=0;i<model.SoTaiKhoan;i++)
                {
                    string username = model.TiepDauNgu + (model.BatDau.Value + i).ToString("000");
                    var user = new ApplicationUser { UserName = username, Email = username + "@c500.edu.vn", PhoneNumber = "" };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        AspNetUser tmp = db.AspNetUsers.Where(p => p.UserName == username).FirstOrDefault();
                        tmp.FullName = username;
                        await UserManager.AddToRoleAsync(tmp.Id, "Student");
                        tbHocVienLopHoc hv = new tbHocVienLopHoc();
                        hv.IdLopHoc = model.IdLop;
                        hv.IdHocVien = tmp.Id;
                        db.tbHocVienLopHocs.Add(hv);
                        foreach (tbLopHocKichBan lopkichban in db.tbLopHocKichBans.Where(p => p.IdLopHoc == model.IdLop).ToList())
                        {
                            tbKetQua kq = new tbKetQua();
                            kq.IdHocVien = hv.IdHocVien;
                            kq.IdLopHoc = hv.IdLopHoc;
                            kq.IdKichBan = lopkichban.IdKichBan;
                            db.tbKetQuas.Add(kq);
                        }
                        demThanhCong++;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Join(";", result.Errors.ToArray()));
                        demLoi++;
                    }
                }
                db.SaveChanges();
                if(demLoi>0)
                    return View(model);
                return RedirectToAction("HocVien","tbLopHocs",new {IdLopHoc=model.IdLop });
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
