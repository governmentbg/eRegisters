using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartRegistry.Web.Models.SelectAdminBody
{
    public class UserAdminBody
    {
        public long UserId { get; set; }

        public string AdminBodyName { get; set; }

        public bool IsCurrent { get; set; }
    }
}