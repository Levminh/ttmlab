using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttm3._0.App_Start
{
    public static class AppSettings
    {
        public static string OpenStackIdentityUrl { get { return "http://10.0.60.12:5000/v3"; } }
        public static string OpenStackUsername { get { return "tantd"; } }
        public static string OpenStackUserId{ get { return "b496531c04104109a02bfb9b2971385a"; } }     
        public static string OpenStackPassword { get { return "@Abc123456789"; } }
        public static string OpenStackRegion { get { return "RegionOne"; } }
    }
}