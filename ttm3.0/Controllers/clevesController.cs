using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ttm3._0.Models;
using TVA.Helper;

namespace ttm3._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class clevesController : Controller
    {
        private dbttm db = new dbttm();
       

        // GET: cleves/Edit/5
        public ActionResult Edit()
        {
            tbSetting eve = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if(eve==null)
            {
                eve = new tbSetting();
                eve.Name = "eve";
                eve.Value = "";
                db.tbSettings.Add(eve);
                db.SaveChanges();
            }
            return View(myHelper.ConvertToEVE(eve));
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(cleve cleve)
        {
            if (ModelState.IsValid)
            {
                tbSetting eve = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
                if (eve == null)
                {
                    eve = new tbSetting();
                    eve.Name = "eve";
                    eve.Value = "";
                    db.tbSettings.Add(eve);
                }
                eve.Value = cleve.Ip + ";" + cleve.Username + ";" + cleve.Password + ";" + cleve.Path+";"+cleve.UsernameEve+";"+cleve.PasswordEve;
                db.SaveChanges();
                return RedirectToAction("Index", "tbSoDoMangs");
            }
            return View(cleve);
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
