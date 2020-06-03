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
    public class jobTitleClientController : ApiController
    {
        [HttpGet]
        public List<JobTitleViewModel> Get(string description) {
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {
                    var lsJobTitles = db.JobTitlesClient.Where(jtc => jtc.jtcl_description.ToUpper().Contains(description.ToUpper()))
                                                        .Select(jtc => new JobTitleViewModel { id = jtc.jtcl_id, description = jtc.jtcl_description, state = jtc.jtcl_state })
                                                        .ToList();

                    return lsJobTitles;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost]
        public IHttpActionResult validateJobTitle(JobTitleViewModel pJobTitle) {           
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {
                    JobTitleViewModel jobTitle;
                    
                    jobTitle = db.JobTitlesClient.Where(j => j.jtcl_description.ToUpper() == pJobTitle.description.ToUpper())
                                                        .Select( jt => new JobTitleViewModel { id = jt.jtcl_id, description = jt.jtcl_description} )                             
                                                        .FirstOrDefault();
                    if (jobTitle == null) {
                        JobTitlesClient jt = new JobTitlesClient();
                        jt.jtcl_description = pJobTitle.description;
                        jt.jtcl_state = true;
                        db.JobTitlesClient.Add(jt);
                        db.SaveChanges();
                    }

                    var jtcl = db.JobTitlesClient.Where(j => j.jtcl_description.ToUpper() == pJobTitle.description.ToUpper())
                                                       .Select(jt => new JobTitleViewModel { id = jt.jtcl_id, description = jt.jtcl_description })
                                                       .FirstOrDefault();

                   

                    return Ok(jtcl);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }

           
        }

        

        
    }
}
