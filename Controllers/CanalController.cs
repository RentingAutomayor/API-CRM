using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;
using API_RA_Forms.ViewModels;
namespace API_RA_Forms.Controllers
{
    public class CanalController : ApiController
    {
        [HttpGet]
        public List<CanalViewModel> Get() {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var lsCanal = db.Canal
                                    .Select(cnl => new CanalViewModel { id = cnl.cnl_id, description = cnl.cnl_description, state = cnl.cnl_state })
                                    .ToList();
                    return lsCanal;
                }
            }
            catch (Exception e)
            {
                throw;
            }
           
        }
    }
}
