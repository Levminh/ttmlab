using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
    [Authorize(Roles ="Admin")]
    public class AspNetUsersController : Controller
    {
        private dbttm db = new dbttm();
        private ApplicationUserManager _userManager;
        public AspNetUser currentUser;
        // GET: AspNetUsers

        public ActionResult Index()
        {
            var uid = User.Identity.GetUserId();
            currentUser = db.AspNetUsers.Find(uid);
            List<AspNetUser> lstU = new List<AspNetUser>();
            lstU = db.AspNetUsers.ToList();
            ViewBag.Title = "Danh sách tài khoản trong toàn hệ thống";
            var lsrR = db.AspNetRoles.ToList();
            foreach (var u in lstU)
            {
                var tmp = UserManager.FindById(u.Id);
                if (tmp != null)
                {
                    List<string> s = new List<string>();
                    foreach (IdentityUserRole r in tmp.Roles)
                    {
                        AspNetRole tmpr = lsrR.Where(t => t.Id == r.RoleId).FirstOrDefault();
                        if (tmpr != null)
                            s.Add(tmpr.Name);
                    }
                    if (s.Count > 0)
                        u.Roles = string.Join(";", s.ToArray());
                }
            }
            return View(lstU.ToList());
        }
        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }
        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AspNetUser aspNetUser)
        {
            ViewBag.Error = "";
            var user = new ApplicationUser { UserName = aspNetUser.UserName, Email = aspNetUser.UserName + "@gmail.com", PhoneNumber = aspNetUser.PhoneNumber };
            var result = await UserManager.CreateAsync(user, aspNetUser.Password);


            if (result.Succeeded)
            {
                AspNetUser tmp = db.AspNetUsers.Where(p => p.UserName == aspNetUser.UserName).FirstOrDefault();
                tmp.FullName = aspNetUser.FullName;
                await UserManager.AddToRoleAsync(tmp.Id, "Student");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Join(";", result.Errors.ToArray()));
                return View(aspNetUser);
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AspNetUser model)
        {
            if (!string.IsNullOrEmpty(model.Password))
            {
                var user = UserManager.FindById(model.Id);
                var code = UserManager.GeneratePasswordResetToken(user.Id);
                var rs = UserManager.ResetPassword(user.Id, code, model.Password);
                if (rs.Succeeded)
                {
                    dbttm db2 = new dbttm();
                    AspNetUser tmp = db2.AspNetUsers.Find(model.Id);
                    if(tmp==null) return RedirectToAction("Index");
                    tmp.PhoneNumber = model.PhoneNumber;
                    tmp.LockoutEnabled = model.LockoutEnabled;
                    tmp.FullName = model.FullName;
                    db2.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Join(";", rs.Errors.ToArray()));
                    return View(model);
                }
            }
            else
                return View(model);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
