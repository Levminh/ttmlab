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
    [Authorize(Roles = "Admin")]
    public class AspNetUserRolesController : Controller
    {
        private ApplicationUserManager _userManager;
        private dbttm db = new dbttm();
        // GET: AspNetUserRoles
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<AspNetUserRole> aspNetUserRoles = db.AspNetUserRoles.Where(p => p.UserId == id).ToList();
            ViewBag.Id = id;
            return View(aspNetUserRoles);
        }

        // GET: AspNetUserRoles/Create
        public ActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.Id = id;
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name");
            AspNetUserRole model = new AspNetUserRole();
            model.UserId = id;
            return View(model);
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,RoleId")] AspNetUserRole model)
        {
            if (ModelState.IsValid)
            {
                if (db.AspNetUserRoles.Where(p => p.UserId == model.UserId && p.RoleId == model.RoleId).Count() == 0)
                {
                    await UserManager.AddToRoleAsync(model.UserId, db.AspNetRoles.Find(model.RoleId).Name);
                }
                return RedirectToAction("Index", new { id = model.UserId });
            }

            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", model.RoleId);
            return View(model);
        }

        // GET: AspNetUserRoles/Delete/5
        public ActionResult Delete(string id, string role)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = db.AspNetUserRoles.Where(p => p.UserId == id && p.RoleId == role).FirstOrDefault();
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string role)
        {
            await UserManager.RemoveFromRoleAsync(id, db.AspNetRoles.Find(role).Name);
            return RedirectToAction("Index", new { id = id });
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
