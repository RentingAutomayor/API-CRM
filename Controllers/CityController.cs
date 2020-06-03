using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API_RA_Forms.ViewModels;
using DAO;

namespace API_RA_Forms.Controllers
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
    public class CityController : ApiController
    {
        [HttpGet]
        public List<CityViewModel>Get() {
            using (BDRAEntities db = new BDRAEntities()) {
                var lsCities = db.Cities
                                .Select(ct => new CityViewModel { id = ct.cty_id, name = ct.cty_name, departmentId = ct.dpt_id })
                                .ToList();

                return lsCities;
            }
        }


        [HttpGet]
        public List<CityViewModel> Get(int departmentId)
        {
            using (BDRAEntities db = new BDRAEntities())
            {
                var lsCities = db.Cities
                                .Where(ct => ct.dpt_id == departmentId)
                                .Select(ct => new CityViewModel { id = ct.cty_id, name = ct.cty_name, departmentId = ct.dpt_id })
                                .ToList();

                return lsCities;
            }
        }
    }
}
