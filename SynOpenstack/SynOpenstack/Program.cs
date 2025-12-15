using OpenStack.Authentication.V3.Auth;
using OpenStack.Compute.v2_1;
using SynOpenstack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Data.Entity;

namespace SynOpenstack
{
    class Program
    { 
        static void Main(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer(50000);
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
            Console.Write("Press any key to exit... ");
            Console.ReadKey();
        }

        private static async Task  HandleTimer()
        {
            //try
            //{
                await GetListIntance();
            //}
            //catch { }
            
        }

        public static async Task GetListIntance()
        {
            Console.Clear();
            Console.WriteLine("Starting......");
            dbttm db = new dbttm();
            
            foreach (tbProjectOpenStack pro in db.tbProjectOpenStacks.ToList())
            {
                try
                {
                    Console.WriteLine("SCANNING PROJECT: " + pro.ProjectName);
                    List<tbComputer> lstCOM = pro.tbComputers.ToList();
                    var identity = new OpenstackIdentityProvider(AppSettings.OpenStackIdentityUrl, AppSettings.OpenStackUserId, AppSettings.OpenStackPassword, scopeId: pro.ProjectId, scopType: AuthScopeType.Project);
                    var compute = new ComputeService(identity, AppSettings.OpenStackRegion);
                    var sr = await compute.ListServersAsync();
                    List<string> lstID = sr.Select(p => p.Id.ToString()).ToList();
                    foreach (tbComputer com in lstCOM)
                    {
                        if (lstID.Where(p => p == com.IdOpenStack).Count() == 0)
                            db.tbComputers.Remove(com);
                    }
                    db.SaveChanges();
                    foreach (var com in sr)
                    {
                        try
                        {
                            Console.WriteLine("    COMPUTER: " + com.Name);
                            tbComputer computer = lstCOM.Where(p => p.IdOpenStack != null && p.IdOpenStack == com.Id).FirstOrDefault();
                            if (computer == null)
                            {
                                computer = new tbComputer();
                                computer.IdProject = pro.IdProject;
                                computer.IdOpenStack = com.Id;
                                db.tbComputers.Add(computer);
                            }
                            RemoteConsole x = await com.GetVncConsoleAsync(RemoteConsoleType.NoVnc);
                            computer.VncUri = @"http://openstack.firmware.vn:6080" + x.Url.PathAndQuery + @"&title=" + com.Name;
                            computer.MoTa = "Mật khẩu Admin: " + com.AdminPassword;
                            computer.Name = com.Name;
                            computer.Status = com.Status.DisplayName;
                            db.SaveChanges();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("    ERROR: " + ex.ToString());
                            continue;
                        }
                    }
                }
                catch (Exception ex){
                    Console.WriteLine("    ERROR: " + ex.ToString());
                    continue;
                }
            }
            db.SaveChanges();
            Console.WriteLine("END......");
        }
    }
}
