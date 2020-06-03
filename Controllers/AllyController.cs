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
                    var branch = db.branch.Where(br => br.ally_document == "860035827" && br.bra_isMain == true)
                                               .Select(br => new BranchViewModel { id = br.bra_id, name = br.bra_name}).FirstOrDefault();
                    var lsAccountManager = db.Contact.Where(cnt => cnt.bra_id == branch.id)
                                                     .Select(
                                                            cnt => new ContactViewModel { 
                                                            id = cnt.cnt_id,
                                                            name = cnt.cnt_name,
                                                            lastName = cnt.cnt_lastName
                                                            }).ToList();

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
