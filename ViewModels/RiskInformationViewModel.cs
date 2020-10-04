using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class RiskInformationViewModel
    {
        public int id;
        public DateTime? dateSubmissionAnalysis;
        public DateTime? dateResponseAnalysis;
        public long ammountApproved;        
        public DateTime? dateApproved;
        public StateViewModel riskState;
        public DateTime? datefiling;
        public UserViewModel user;
        public bool state;
    }
}