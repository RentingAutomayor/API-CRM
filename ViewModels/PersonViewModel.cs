using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class PersonViewModel
    {
      
        public string id;
        public KindOfDocumentViewModel kindOfDocument;
        public string name;
        public string lastName;
        public string cellPhone;
        public string phone;
        public string email;
        public CityViewModel city;
    }
}