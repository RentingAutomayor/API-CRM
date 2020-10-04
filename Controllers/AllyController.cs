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
    public class AllyController : ApiController
    {
        [HttpGet]
        public IHttpActionResult getAccountManager() {
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {
                    var AV_VILLAS_NIT = "860035827";
                    var branch = db.branch.Where(br => br.ally_document == AV_VILLAS_NIT && br.bra_isMain == true)
                                               .Select(br => new BranchViewModel { id = br.bra_id, name = br.bra_name}).FirstOrDefault();
                    var lsAccountManager = db.Contact.Where(cnt => cnt.bra_id == branch.id && cnt.cnt_state == true)
                                                     .Select(
                                                            cnt => new ContactViewModel { 
                                                            id = cnt.cnt_id,
                                                            name = cnt.cnt_name,
                                                            lastName = (cnt.cnt_lastName!=null)?cnt.cnt_lastName:"",
                                                            })
                                                     .ToList().OrderBy(cnt => cnt.name);

                    return Ok(lsAccountManager);
                }
                    
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

    }
}
