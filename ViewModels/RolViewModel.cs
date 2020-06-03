using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class RolViewModel
    {
        public int id;
        public string name;
        public string description;
        public List<PermissionByModuleViewModel> permissionByModule;

    }
}