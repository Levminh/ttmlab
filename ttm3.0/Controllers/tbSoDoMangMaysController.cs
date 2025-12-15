using Renci.SshNet;
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
    [Authorize(Roles = "Admin,Teacher")]
    public class tbSoDoMangMaysController : Controller
    {
        private dbttm db = new dbttm();

        // GET: tbSoDoMangMays
        public ActionResult Index(int? IdSoDoMang,string Loi)
        {
            ViewBag.Loi = Loi;
            if(IdSoDoMang==null) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var tbSoDoMangMays = db.tbSoDoMangMays.Include(t => t.tbSoDoMang).Where(p=>p.IdSoDoMang==IdSoDoMang);
            ViewBag.IdSoDoMang = IdSoDoMang;
            return View(tbSoDoMangMays.ToList());
        }
        private List<string> listFiles(string UUID)
        {
            tbSetting setting = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if (setting == null) return new List<string>();
            cleve eve = myHelper.ConvertToEVE(setting);
            List<string> lstRS = new List<string>();          
            string host = eve.Ip;
            string username = eve.Username;
            string password = eve.Password;
            string remoteDirectory = eve.Path+UUID+"/";
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(remoteDirectory);
                    foreach (var file in files)
                    {
                        if (file.IsDirectory)
                            lstRS.Add(file.Name);
                    }
                    sftp.Disconnect();
                }
                catch (Exception e)
                {
                    //Console.WriteLine("An exception has been caught " + e.ToString());
                }
            }
            return lstRS;
        }      
        public ActionResult Scan(int? IdSoDoMang)
        {
            if(IdSoDoMang==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbSoDoMang sdm = db.tbSoDoMangs.Find(IdSoDoMang);
            if(sdm==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if(string.IsNullOrEmpty(sdm.Path)) RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = "Chưa cài đặt Path cho sơ đồ mạng" });          
            tbSetting setting = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if (setting == null)
            {
                return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = "Thiếu cài đặt EVE" });
            }
            cleve eve = myHelper.ConvertToEVE(setting);
            SshClient sshclient = new SshClient(eve.Ip, eve.Username, eve.Password);
            try
            {
                sshclient.Connect();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = "Không kết nối được với máy chủ EVE" });
            }
            try
            {
                string cmd = "curl -s -b /tmp/cookie -c /tmp/cookie -X POST -d '{\"username\":\""+eve.UsernameEve+"\",\"password\":\""+eve.PasswordEve+"\"}' -k  https://127.0.0.1/api/auth/login";
                SshCommand sc = sshclient.CreateCommand(cmd);
                sc.Execute();
            }
            catch
            {
                return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = "Đăng nhập EVE bị lỗi" });
            }          
            try
            {
                string cmd = string.Format("curl -s -c /tmp/cookie -b /tmp/cookie -X GET -H 'Content-type: application/json' -k https://127.0.0.1/api/{0}/nodes",sdm.Path);
                SshCommand sc = sshclient.CreateCommand(cmd);
                sc.Execute();
                string re=sc.Result;
                string[] data = re.Split(new string[] { "\"data\":{"},StringSplitOptions.None).ToArray();
                if(data.Count()<2)
                    return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = "Đinh dạng oupt không đúng" });
                re = data[1];
                data = re.Split(new string[] { "}," }, StringSplitOptions.None);
                int count = 0;
                for (int i=0;i<data.Count();i++)
                {
                    string s = data[i];
                    string[] node=s.Split(new string[] { "\":{" }, StringSplitOptions.None);
                    string sid = node[0].Remove(0, 1);
                    int id;
                    if (int.TryParse(sid, out id))
                    {
                        if (sdm.tbSoDoMangMays.Where(p => p.Id == id).Count()==0)
                        {
                            tbSoDoMangMay may = new tbSoDoMangMay();
                            may.IdSoDoMang = sdm.Id;
                            may.MaMay = id.ToString();
                            string[] chitiet = node[1].Split(',');
                            if (chitiet.Count() > 10)
                            {
                                may.TenMay = chitiet[6].Split('"')[3];
                                may.Kieu = chitiet[10].Split('"')[3];
                            }
                            db.tbSoDoMangMays.Add(may);
                            count++;
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = string.Format("Scan thành công, thêm {0} máy",count.ToString()) });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang, Loi = ex.ToString() });
            }

        }

        public string UpdateTenMay(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMangMay kq = db.tbSoDoMangMays.Find(Id);
                if (kq == null) return "";
                kq.TenMay = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }

        public string UpdateMaMay(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMangMay kq = db.tbSoDoMangMays.Find(Id);
                if (kq == null) return "";
                kq.MaMay = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }

        public string UpdateGhiChu(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMangMay kq = db.tbSoDoMangMays.Find(Id);
                if (kq == null) return "";
                kq.GhiChu = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }
        public JsonResult LoadKieuAsync()
        {
            List<string> lstS = new List<string>();
            lstS.Add(string.Format("\"{0}\":\"{1}\"", "--Chọn--", "--Chọn--"));
            lstS.Add(string.Format("\"{0}\":\"{1}\"", "Có snapshot", "Có snapshot"));
            lstS.Add(string.Format("\"{0}\":\"{1}\"", "Không có snapshot", "Không có snapshot"));
            return Json("{" + string.Join(",", lstS.ToArray()) + "}", JsonRequestBehavior.AllowGet);
        }
        public string UpdateKieu(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int Id;
            if (int.TryParse(id, out Id))
            {
                tbSoDoMangMay kq = db.tbSoDoMangMays.Find(Id);
                if (kq == null) return "";
                kq.Kieu = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }
        // GET: tbSoDoMangMays/Create
        public ActionResult Create(int? IdSoDoMang)
        {
            if (IdSoDoMang == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbSoDoMangMay model = new tbSoDoMangMay();
            model.IdSoDoMang = IdSoDoMang;
            db.tbSoDoMangMays.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdSoDoMang = IdSoDoMang });
        }

        

        

        // GET: tbSoDoMangMays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSoDoMangMay tbSoDoMangMay = db.tbSoDoMangMays.Find(id);
            if (tbSoDoMangMay == null)
            {
                return HttpNotFound();
            }
            return View(tbSoDoMangMay);
        }

        // POST: tbSoDoMangMays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbSoDoMangMay tbSoDoMangMay = db.tbSoDoMangMays.Find(id);
            int? idSoDoMang = tbSoDoMangMay.IdSoDoMang;
            db.tbSoDoMangMays.Remove(tbSoDoMangMay);
            db.SaveChanges();
            return RedirectToAction("Index", new { IdSoDoMang = idSoDoMang });
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
