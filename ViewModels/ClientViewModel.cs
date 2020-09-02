using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class ClientViewModel : PersonViewModel
    {
        public EconomicActivityViewModel economicActivity;        
        public List<ContactViewModel> lsContacts;
    }
}