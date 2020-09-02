using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_RA_Forms.ViewModels
{
    public class RequestViewModel
    {
        public int id;
        public DateTime? initialDate;
        public DateTime? lastDate;
        public DateTime? registrationDate;
        public ClientViewModel client;
        public ProbabilityViewModel probability;
        public StateViewModel parentState;
        public StateViewModel childState;
        public UserViewModel user;
        public ContactViewModel contact;
        public RiskInformationViewModel riskInformation;
        public OperationalInformationViewModel operationalInformation;
        public CanalViewModel canal;
        public string observation;
    }
}