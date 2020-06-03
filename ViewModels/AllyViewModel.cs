using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class AllyViewModel
    {
        public string document;
        public KindOfDocumentViewModel kindOfDociment;
        public string name;
        public string lastName;
        public float percentCommission;
        public DateTime? registrationDate;
        public bool state;

    }
}