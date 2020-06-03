using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class BranchViewModel
    {
        public int id;
        public string name;
        public string adress;
        public string phone;
        public bool isMain;
        public bool state;
        public ClientViewModel client;
        public AllyViewModel ally;
    }
}