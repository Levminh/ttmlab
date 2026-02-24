using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenStack.Authentication.V3.Auth;
using OpenStack.Compute.v2_1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ttm3._0.App_Start;
using ttm3._0.Models;
using Newtonsoft.Json;
using Renci.SshNet;

namespace ttm3._0.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class tbLopHocsController : Controller
    {
        private dbttm db = new dbttm();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public tbLopHocsController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        
        public async Task<ActionResult> StartAll(int? IdLopHoc)
        {
            if(!IdLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbHocVienLopHoc> lstHV = db.tbHocVienLopHocs.Include(p=>p.tbComputer).Where(p => p.IdLopHoc == IdLopHoc).ToList();
            foreach (tbProjectOpenStack pro in db.tbProjectOpenStacks.ToList())
            {
                var identity = new OpenstackIdentityProvider(AppSettings.OpenStackIdentityUrl, AppSettings.OpenStackUserId, AppSettings.OpenStackPassword, scopeId: pro.ProjectId, scopType: AuthScopeType.Project);
                var compute = new ComputeService(identity, AppSettings.OpenStackRegion);
                var sr = await compute.ListServersAsync();
                foreach (var com in sr)
                {
                    tbHocVienLopHoc hv = lstHV.Where(p => p.tbComputer != null && p.tbComputer.IdOpenStack == com.Id).FirstOrDefault();
                    if (hv != null)
                    {
                        if (com.Status != ServerStatus.Active)
                        {
                            hv.tbComputer.Status = "ACTIVE";
                            await com.StartAsync();
                        }
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("HocVien", new { IdLopHoc = IdLopHoc });
        }
        public async Task<ActionResult> StopAll(int? IdLopHoc)
        {
            if (!IdLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbHocVienLopHoc> lstHV = db.tbHocVienLopHocs.Include(p => p.tbComputer).Where(p => p.IdLopHoc == IdLopHoc).ToList();
            foreach (tbProjectOpenStack pro in db.tbProjectOpenStacks.ToList())
            {
                var identity = new OpenstackIdentityProvider(AppSettings.OpenStackIdentityUrl, AppSettings.OpenStackUserId, AppSettings.OpenStackPassword, scopeId: pro.ProjectId, scopType: AuthScopeType.Project);
                var compute = new ComputeService(identity, AppSettings.OpenStackRegion);
                var sr = await compute.ListServersAsync();
                foreach (var com in sr)
                {
                    tbHocVienLopHoc hv = lstHV.Where(p => p.tbComputer != null && p.tbComputer.IdOpenStack == com.Id).FirstOrDefault();
                    if (hv != null)
                    {
                        if (com.Status != ServerStatus.Stopped)
                        {
                            hv.tbComputer.Status = "SHUTOFF";
                            await com.StopAsync();
                        }
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("HocVien", new { IdLopHoc = IdLopHoc });
        }
        public async Task<ActionResult> StartOrStop(int? IdHocVienLopHoc)
        {
            if(!IdHocVienLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbHocVienLopHoc hocvien = db.tbHocVienLopHocs.Where(p => p.IdHocVienLopHoc == IdHocVienLopHoc).FirstOrDefault();
            if(hocvien==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            foreach (tbProjectOpenStack pro in db.tbProjectOpenStacks.ToList())
            {
                var identity = new OpenstackIdentityProvider(AppSettings.OpenStackIdentityUrl, AppSettings.OpenStackUserId, AppSettings.OpenStackPassword, scopeId: pro.ProjectId, scopType: AuthScopeType.Project);
                var compute = new ComputeService(identity, AppSettings.OpenStackRegion);
                var sr = await compute.ListServersAsync();
                foreach (var com in sr)
                {
                    if(com.Id==hocvien.tbComputer.IdOpenStack)
                    {
                        if (com.Status!=ServerStatus.Active)
                        {
                            await com.StartAsync();
                            hocvien.tbComputer.Status = "ACTIVE";
                        }
                        else
                        {
                            hocvien.tbComputer.Status = "SHUTOFF";
                            await com.StopAsync();
                        }
                        db.SaveChanges();
                        return RedirectToAction("HocVien", new { IdLopHoc =hocvien.IdLopHoc});
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("HocVien", new { IdLopHoc = hocvien.IdLopHoc });
        }
        public ActionResult KichBan(int? IdLopHoc,string Loi)
        {
            ViewBag.Loi = Loi;
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.IdLopHoc = IdLopHoc;
            return View(lop.tbLopHocKichBans.ToList());
        }

        public ActionResult CaiDatMay(int? IdLopHoc)
        {
            if(IdLopHoc==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc });
        }

        public ActionResult BatMay(int? IdLopHoc)
        {
            if (IdLopHoc == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc });
        }

        public ActionResult TatMay(int? IdLopHoc)
        {
            if (IdLopHoc == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc });
        }

        public ActionResult CaiDatMayLe(int? IdKichBan,int? IdLopHoc)
        {
            if (IdKichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBan kichBan = db.tbKichBans.Find(IdKichBan);
            if (kichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbSetting setting = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if (setting == null)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc ,Loi="Thiếu cài đặt EVE"});
            }
            cleve eve = ConvertToEVE(setting);
            SshClient sshclient = new SshClient(eve.Ip, eve.Username, eve.Password);
            try
            {
                sshclient.Connect();
            }
            catch(Exception ex)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc,Loi="Không kết nối EVE" });
            }
            foreach (tbKichBanSoDoMang kbsdm in kichBan.tbKichBanSoDoMangs)
            {
                if(kbsdm.tbSoDoMang!=null)
                {
                    foreach(tbSoDoMangMay sdm in kbsdm.tbSoDoMang.tbSoDoMangMays)
                    {
                        try
                        {

                            if (sdm.Kieu == "Có snapshot")
                            {

                                string cmd = "qemu-img snapshot -a crc500 "+ eve.Path + kbsdm.tbSoDoMang.UUID + "/" + sdm.MaMay + "/hda.qcow2";
                                //string cmd = "qemu - img info " + eve.Path + kbsdm.tbSoDoMang.UUID + "/" + sdm.MaMay + "/hda.qcow2";
                                SshCommand sc = sshclient.CreateCommand(cmd);
                                sc.Execute();
                                //s += " | " + cmd;
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                }
            }
            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi="Thành công! " });
        }
        cleve ConvertToEVE(tbSetting setting)
        {
            cleve eve = new cleve();
            string[] lst = setting.Value.Split(';').ToArray();
            if (lst.Count() == 4)
            {
                eve.Id = 1;
                eve.Ip = lst[0];
                eve.Username = lst[1];
                eve.Password = lst[2];
                eve.RePassword = lst[2];
                eve.Path = lst[3];
            }
            return eve;
        }
        public ActionResult BatMayLe(int? IdKichBan, int? IdLopHoc)
        {
            if (IdKichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBan kichBan = db.tbKichBans.Find(IdKichBan);
            if (kichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbSetting setting = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if (setting == null)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Thiếu cài đặt EVE" });
            }
            cleve eve = ConvertToEVE(setting);
            SshClient sshclient = new SshClient(eve.Ip, eve.Username, eve.Password);
            try
            {
                sshclient.Connect();
            }
            catch (Exception ex)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Không kết nối được với máy chủ EVE" });
            }
            try
            {
                string cmd = "curl -s -b /tmp/cookie -c /tmp/cookie -X POST -d '{\"username\":\"luu\",\"password\":\"eve\"}' -k  https://127.0.0.1/api/auth/login";
                SshCommand sc = sshclient.CreateCommand(cmd);
                sc.Execute();
            }
            catch
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Đăng nhập EVE bị lỗi" });
            }
            foreach (tbKichBanSoDoMang kbsdm in kichBan.tbKichBanSoDoMangs)
            {
                if (kbsdm.tbSoDoMang != null)
                {
                    foreach (tbSoDoMangMay may in kbsdm.tbSoDoMang.tbSoDoMangMays)
                    {
                        try
                        {
                            string cmd = string.Format("curl -s -c /tmp/cookie -b /tmp/cookie -X GET -H 'Content-type: application/json' -k https://127.0.0.1/api/{0}/nodes/{1}/start",kbsdm.tbSoDoMang.Path,may.MaMay);
                                SshCommand sc = sshclient.CreateCommand(cmd);
                                sc.Execute();
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Quá trình bật máy bị lỗi" });
                        }
                    }
                }
            }
            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Bật máy thành công! " });
        }

        public ActionResult TatMayLe(int? IdKichBan, int? IdLopHoc)
        {
            if (IdKichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbKichBan kichBan = db.tbKichBans.Find(IdKichBan);
            if (kichBan == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbSetting setting = db.tbSettings.Where(p => p.Name == "eve").FirstOrDefault();
            if (setting == null)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Thiếu cài đặt EVE" });
            }
            cleve eve = ConvertToEVE(setting);
            SshClient sshclient = new SshClient(eve.Ip, eve.Username, eve.Password);
            try
            {
                sshclient.Connect();
            }
            catch (Exception ex)
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Không kết nối được với máy chủ EVE" });
            }
            try
            {
                string cmd = "curl -s -b /tmp/cookie -c /tmp/cookie -X POST -d '{\"username\":\"luu\",\"password\":\"eve\"}' -k  https://127.0.0.1/api/auth/login";
                SshCommand sc = sshclient.CreateCommand(cmd);
                sc.Execute();
            }
            catch
            {
                return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Đăng nhập EVE bị lỗi" });
            }
            foreach (tbKichBanSoDoMang kbsdm in kichBan.tbKichBanSoDoMangs)
            {
                if (kbsdm.tbSoDoMang != null)
                {
                    foreach (tbSoDoMangMay sdm in kbsdm.tbSoDoMang.tbSoDoMangMays)
                    {
                        try
                        {
                            string cmd = string.Format("curl -s -c /tmp/cookie -b /tmp/cookie -X GET -H 'Content-type: application/json' -k https://127.0.0.1/api/{0}/nodes/{1}/stop/stopmode=3",kbsdm.tbSoDoMang.Path,sdm.MaMay);
                            SshCommand sc = sshclient.CreateCommand(cmd);
                            sc.Execute();
                        }
                        catch (Exception ex)
                        {
                            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Quá trình tắt máy bị lỗi" });
                        }
                    }
                }
            }
            return RedirectToAction("KichBan", new { IdLopHoc = IdLopHoc, Loi = "Tắt máy thành công! " });
        }

        public ActionResult KichBanAdd(int? IdLopHoc)
        {
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbLopHocKichBan hv = new tbLopHocKichBan();
            hv.IdLopHoc = lop.IdLopHoc;

            ViewBag.IdKichBan = new SelectList(db.tbKichBans, "IdKichBan", "TenKichBan");
            return View(hv);
        }
        public ActionResult KichBanDelete(int? IdLopHocKichBan)
        {
            tbLopHocKichBan hv = db.tbLopHocKichBans.Find(IdLopHocKichBan);
            if (hv == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? idlop = hv.IdLopHoc;
            db.tbLopHocKichBans.Remove(hv);
            db.SaveChanges();
            return RedirectToAction("KichBan", new { IdLopHoc = idlop });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KichBanAdd(tbLopHocKichBan model)
        {
            if (ModelState.IsValid)
            {
                if (db.tbLopHocKichBans.Where(p => p.IdLopHoc == model.IdLopHoc && p.IdKichBan == model.IdKichBan).Count() == 0)
                {
                    db.tbLopHocKichBans.Add(model);
                    foreach (tbHocVienLopHoc lophocvien in db.tbHocVienLopHocs.Where(p => p.IdLopHoc == model.IdLopHoc).ToList())
                    {
                        tbKetQua kq = new tbKetQua();
                        kq.IdHocVien = lophocvien.IdHocVien;
                        kq.IdLopHoc = model.IdLopHoc;
                        kq.IdKichBan = model.IdKichBan;
                        db.tbKetQuas.Add(kq);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("KichBan", new { IdLopHoc = model.IdLopHoc });
            }

            return View(model);
        }

        public ActionResult HocVien(int? IdLopHoc)
        {
            string userId = User.Identity.GetUserId();
            //var user = UserManager.FindById(User.Identity.GetUserId());
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if(lop==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbLichPhongLab> lstLICH = db.tbLichPhongLabs.Where(p => p.IdUser == userId && lop.IdProjectOS==p.IdProject).ToList();
            DateTime dt = DateTime.Now;
            if (User.IsInRole("Admin"))
            {
                ViewBag.CoQuyen = true;
            }
            else
            {
                if (lstLICH.Where(p => p.TuNgay <= dt && p.DenNgay >= dt).Count() == 0)
                    ViewBag.CoQuyen = false;
                else
                    ViewBag.CoQuyen = true;
            }

            ViewBag.IdLopHoc = IdLopHoc;
            List<tbKetQua> lstKQ = db.tbKetQuas.Where(p => p.IdLopHoc == lop.IdLopHoc).ToList();
            foreach(tbHocVienLopHoc hv in lop.tbHocVienLopHocs.ToList())
            {
                hv.TongDiem = lstKQ.Where(p => p.IdHocVien == hv.IdHocVien).Sum(p => p.Diem);
            }
            
            return View(lop.tbHocVienLopHocs.ToList());
        }

        public ActionResult HocVienAdd(int? IdLopHoc)
        {
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbHocVienLopHoc hv = new tbHocVienLopHoc();
            hv.IdLopHoc = lop.IdLopHoc;
            
            ViewBag.IdHocVien = new SelectList(db.AspNetUsers, "Id", "Email");
            return View(hv);
        }
        public ActionResult HocVienDelete(int? IdHocVienLopHoc)
        {
            tbHocVienLopHoc hv = db.tbHocVienLopHocs.Find(IdHocVienLopHoc);
            if(hv==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? idlop = hv.IdLopHoc;
            db.tbHocVienLopHocs.Remove(hv);
            db.SaveChanges();
            return RedirectToAction("HocVien", new { IdLopHoc = idlop });

        }

        public ActionResult ResetPassword(int? IdHocVienLopHoc)
        {
            tbHocVienLopHoc hv = db.tbHocVienLopHocs.Find(IdHocVienLopHoc);
            if (hv == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? idlop = hv.IdLopHoc;
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            userManager.RemovePassword(hv.AspNetUser.Id);
            userManager.AddPassword(hv.AspNetUser.Id, "@Abc123");
            return RedirectToAction("HocVien", new { IdLopHoc = idlop });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HocVienAdd(tbHocVienLopHoc model)
        {
            if (ModelState.IsValid)
            {
                if (db.tbHocVienLopHocs.Where(p => p.IdLopHoc == model.IdLopHoc && p.IdHocVien == model.IdHocVien).Count() == 0)
                {
                    db.tbHocVienLopHocs.Add(model);
                    foreach (tbLopHocKichBan lopkichban in db.tbLopHocKichBans.Where(p => p.IdLopHoc == model.IdLopHoc).ToList())
                    {
                        tbKetQua kq = new tbKetQua();
                        kq.IdHocVien = model.IdHocVien;
                        kq.IdLopHoc = model.IdLopHoc;
                        kq.IdKichBan = lopkichban.IdKichBan;
                        db.tbKetQuas.Add(kq);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("HocVien",new {IdLopHoc=model.IdLopHoc });
            }

            return View(model);
        }
        public JsonResult LoadComputerAsync(int? Id)
        {
            if (!Id.HasValue)
                return Json("{}", JsonRequestBehavior.AllowGet);
            List<string> lstS = new List<string>();
            tbHocVienLopHoc hvlop = db.tbHocVienLopHocs.Find(Id);
            if(hvlop==null) return Json("{}", JsonRequestBehavior.AllowGet);
            lstS.Add(string.Format("\"{0}\":\"{1}\"", "0", "--Chọn--"));
            foreach (tbComputer com in db.tbComputers.Where(p=>p.IdProject==hvlop.tbLopHoc.IdProjectOS).ToList())
            {
                lstS.Add(string.Format("\"{0}\":\"{1}\"", com.Id.ToString(), com.Name));
            }
            return  Json("{"+string.Join(",",lstS.OrderBy(p=>p).ToArray())+"}", JsonRequestBehavior.AllowGet);
        }
        public string UpdateComputer(string id,string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int idHocVienLop;
            int IdMay;
            if (!int.TryParse(id, out idHocVienLop)) return "";
            tbHocVienLopHoc hvlop = db.tbHocVienLopHocs.Find(idHocVienLop);
            if (hvlop == null) return "";

            if (!int.TryParse(value, out IdMay)) return "";
            if(IdMay==0)
            {
                hvlop.IdComputer = null;
                hvlop.GhiChu = "";
                db.SaveChanges();
                return "";
            }
            tbComputer com = db.tbComputers.Find(IdMay);
            if (com == null) return "";
            hvlop.IdComputer = com.Id;
            hvlop.GhiChu = com.MoTa;
            db.SaveChanges();
            return com.Name;
        }
        public ActionResult KetQua(int? IdLopHoc)
        {
            tbLopHoc lop = db.tbLopHocs.Find(IdLopHoc);
            if (lop == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbHocVienLopHoc> lstKHV = lop.tbHocVienLopHocs.ToList();
            List<clKetQua> lstKQ = new List<clKetQua>();
            List<tbLopHocKichBan> lstKB = lop.tbLopHocKichBans.OrderBy(p => p.IdKichBan).ToList();
            List<tbKetQua> lstKQTong = lop.tbKetQuas.ToList();
            foreach (tbHocVienLopHoc u in lstKHV)
            {
                clKetQua kq = new clKetQua();
                kq.User = u.AspNetUser;
                kq.lsKQ.AddRange(lstKQTong.Where(p => p.IdHocVien == u.IdHocVien && lstKB.Where(o=>o.IdKichBan==p.IdKichBan).Count()>0).OrderBy(p => p.IdKichBan).ToList());
                lstKQ.Add(kq);
            }
            
            ViewBag.LstKB = lstKB;
            ViewBag.IdLopHoc = IdLopHoc;
            return View(lstKQ.OrderByDescending(o=>o.TongDiem));
        }

        public ActionResult tkSinhVien()
        {
            List<tbHocVienLopHoc> lstKHV = db.tbHocVienLopHocs.ToList();
            List<clTKSinhVien> lstKQ = new List<clTKSinhVien>();
            foreach (tbHocVienLopHoc u in lstKHV)
            {
                clTKSinhVien sv = lstKQ.Where(p => p.SinhVien.Id == u.AspNetUser.Id).FirstOrDefault();
                if (sv==null)
                {
                    sv = new clTKSinhVien();
                    sv.SinhVien = u.AspNetUser;
                    sv.TongDiem = 0;
                    lstKQ.Add(sv);
                }
                sv.Lop += u.tbLopHoc.LopHoc+"; ";
                sv.TongDiem += u.tbLopHoc.tbKetQuas.Sum(p => p.Diem);
            }
            return View(lstKQ.OrderByDescending(o => o.TongDiem));
        }
        // GET: tbLopHocs
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            AspNetUser query = db.AspNetUsers.Find(user.Id);
            if (User.IsInRole("Admin"))
            {
                return View(db.tbLopHocs.ToList());
            }else
            {
                return View(query.tbLopHocs.ToList());
            }          
        }
        public ActionResult VNC(int? IdLopHoc)
        {
            if(!IdLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbHocVienLopHoc> lstCOM = db.tbHocVienLopHocs.Include(p=>p.tbComputer).Include(p=>p.AspNetUser).Where(p => p.IdLopHoc == IdLopHoc).ToList();
            double sl = lstCOM.Count/(double)3;
            if (sl % 1 != 0)
                sl = sl + 1;
            ViewBag.SoLuong = (int)sl;
            ViewBag.Count = lstCOM.Count;
            int dem = 0;
            foreach(tbHocVienLopHoc com in lstCOM.OrderBy(p=>p.IdHocVienLopHoc))
            {
                com.TT = dem++;
            }
            ViewBag.IdLopHoc = IdLopHoc;
            return View(lstCOM.ToList());
        }

        public ActionResult CapMay(int? IdLopHoc)
        {
            if (!IdLopHoc.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbLopHoc lh = db.tbLopHocs.Find(IdLopHoc);
            if(lh==null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            List<tbHocVienLopHoc> lstHV = db.tbHocVienLopHocs.Where(p => p.IdLopHoc == IdLopHoc).ToList();
            tbComputer[] lstCom = db.tbComputers.Where(p => p.IdProject == lh.IdProjectOS).ToArray();
            int dem = 0;
            int Count = lstCom.Count();
            foreach (tbHocVienLopHoc hv in lstHV)
            {
                if (hv.IdComputer.HasValue) continue;
                if(dem<Count)
                {
                    tbComputer com = lstCom[dem];
                    hv.IdComputer = com.Id;
                    hv.GhiChu = com.MoTa;
                }else
                {
                    break;
                }
                dem++;
            }
            db.SaveChanges();
            return RedirectToAction("HocVien", new { IdLopHoc = IdLopHoc });
        }

        // GET: tbLopHocs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbLopHoc tbLopHoc = db.tbLopHocs.Find(id);
            if (tbLopHoc == null)
            {
                return HttpNotFound();
            }
            return View(tbLopHoc);
        }

        // GET: tbLopHocs/Create
        public ActionResult Create()
        {
            ViewBag.IdProjectOS = new SelectList(db.tbProjectOpenStacks.ToList(), "IdProject", "ProjectName", null);
            return View();
        }

        // POST: tbLopHocs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbLopHoc tbLopHoc)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());             
                db.tbLopHocs.Add(tbLopHoc);
                tbLopHoc.NguoiTao = user.Id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProjectOS = new SelectList(db.tbProjectOpenStacks.ToList(), "IdProject", "ProjectName", tbLopHoc.IdProjectOS);
            return View(tbLopHoc);
        }
        
        public string UpdateGhiChu(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int IdHocVienLopHoc;
            if (int.TryParse(id, out IdHocVienLopHoc))
            {
                tbHocVienLopHoc kq = db.tbHocVienLopHocs.Find(IdHocVienLopHoc);
                if (kq == null) return "";
                kq.GhiChu = value;
                db.SaveChanges();
                return value;
            }
            return "";
        }

        public string UpdateKetQua(string id, string value)
        {
            if (string.IsNullOrEmpty(id)) return "";
            int idKetQua;
            if (int.TryParse(id, out idKetQua))
            {
                tbKetQua kq = db.tbKetQuas.Find(idKetQua);
                if (kq == null) return "";
                double diem;
                if (!double.TryParse(value, out diem)) return "";
                kq.Diem = diem;
                db.SaveChanges();
                return value;
            }
            return "";
        }
        // GET: tbLopHocs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbLopHoc tbLopHoc = db.tbLopHocs.Find(id);
            if (tbLopHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProjectOS = new SelectList(db.tbProjectOpenStacks.ToList(), "IdProject", "ProjectName", tbLopHoc.IdProjectOS);
            return View(tbLopHoc);
        }

        // POST: tbLopHocs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbLopHoc tbLopHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbLopHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProjectOS = new SelectList(db.tbProjectOpenStacks.ToList(), "IdProject", "ProjectName", tbLopHoc.IdProjectOS);
            return View(tbLopHoc);
        }

        // GET: tbLopHocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbLopHoc tbLopHoc = db.tbLopHocs.Find(id);
            if (tbLopHoc == null)
            {
                return HttpNotFound();
            }
            return View(tbLopHoc);
        }

        // POST: tbLopHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbLopHoc tbLopHoc = db.tbLopHocs.Find(id);
            db.tbLopHocs.Remove(tbLopHoc);
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
