using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class UserViewModel
    {
        
        public int? kindOfDocument;
        public string id;
        public string name;
        public string lastName;
        public string cellPhone;
        public string email;
        public LoginViewModel login;
        public RolViewModel rol;
    }
}