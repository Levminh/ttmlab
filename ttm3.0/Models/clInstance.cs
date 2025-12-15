using OpenStack.Compute.v2_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttm3._0.Models
{
    public class clInstance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IdProject { get; set; }
        public string Status { get; set; }
        public Server SR { get; set; }

    }
}