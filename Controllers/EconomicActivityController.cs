using API_RA_Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;

namespace API_RA_Forms.Controllers
{
    public class EconomicActivityController : ApiController
    {
        [HttpGet]
        public List<EconomicActivityViewModel> Get()
        {
            List<EconomicActivityViewModel> lsEconomicActivities = new List<EconomicActivityViewModel>();
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    lsEconomicActivities = db.EconomicActivity
                                            .Select(ea => new EconomicActivityViewModel { id = ea.ea_id, description = ea.ea_description, state = ea.ea_state })
                                            .OrderByDescending(ea => ea.description)
                                            .ToList();                                                         }
            }
            catch (Exception e)
            {

                throw;
            }
            return lsEconomicActivities;


        }
    }
}
