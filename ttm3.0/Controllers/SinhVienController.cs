using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;

namespace ttm3._0.Controllers
{
    [Authorize]
    public class SinhVienController : Controller
    {
        private dbttm db = new dbttm();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public SinhVienController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        // GET: SinhVien
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var query = db.tbHocVienLopHocs.Where(p => p.IdHocVien == user.Id).ToList();         
            return View(query.Select(p=>p.tbLopHoc).ToList());
        }
        public ActionResult KichBan(int? IdLopHoc)
        {
            var query = db.tbLopHocKichBans.Where(p => p.IdLopHoc == IdLopHoc).ToList();
            var user = UserManager.FindById(User.Identity.GetUserId());
            tbHocVienLopHoc tbHocVienLopHoc = db.tbHocVienLopHocs.Where(p => p.IdHocVien == user.Id && p.IdLopHoc == IdLopHoc).FirstOrDefault();
            List<tbKichBan> lstKB = query.Select(p => p.tbKichBan).ToList();
            List<tbKetQua> lstKQ = db.tbKetQuas.Where(p => p.IdLopHoc == IdLopHoc && p.IdHocVien == user.Id).ToList();
            foreach(tbKichBan kb in lstKB)
            {
                tbKetQua tmp = lstKQ.Where(p => p.IdKichBan == kb.IdKichBan).FirstOrDefault();
                if (tmp != null)
                {
                    kb.Diem = tmp.Diem;
                    kb.IdKetQua = tmp.IdKetQua;
                }
            }
            if (tbHocVienLopHoc != null)
            {
                ViewBag.GhiChu = tbHocVienLopHoc.GhiChu;
                if (tbHocVienLopHoc.tbComputer != null)
                    ViewBag.VNC = tbHocVienLopHoc.tbComputer.VncUri;
                else
                    ViewBag.VNC = "#";
            }
            else
            {
                ViewBag.GhiChu = "";
                ViewBag.VNC = "#";
            }
            return View(lstKB);
        }
    }
}