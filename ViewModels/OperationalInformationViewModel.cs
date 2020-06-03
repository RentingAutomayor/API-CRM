using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class OperationalInformationViewModel
    {
        public int id;
        public int deliveredVehicles;
        public decimal deliveredAmmount;
        public DateTime? legalizationDate;
        public DateTime? deliveredDate;
        public UserViewModel user;
        public bool state;
    }
}