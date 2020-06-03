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
    public class ContactController : ApiController
    {
        [HttpPost]
        public IHttpActionResult addContactsByClient(List<ContactViewModel> lsContacts)
        {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    if (lsContacts.Count > 0) {
                        foreach (var oContact in lsContacts)
                        {

                            Contact bdContact = new Contact();
                            bdContact.cnt_name = oContact.name;
                            bdContact.cnt_lastName = oContact.lastName;
                            bdContact.cnt_email = oContact.email;
                            bdContact.cnt_phone = oContact.phone;
                            bdContact.cnt_cellPhone = oContact.cellPhone;
                            bdContact.cnt_adress = oContact.adress;
                            bdContact.jtcl_id = oContact.jobTitle.id;
                            bdContact.bra_id = oContact.branch.id;


                            db.Contact.Add(bdContact);
                            db.SaveChanges();
                        }
                    }
                    
                }

                return Ok(lsContacts);
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);

            }
        }

        [HttpGet]
        public List<ContactViewModel> getContactsByClient(string pClient_id)
        {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var countBranch = db.branch.Where(b => b.cli_document == pClient_id).Count();
                    var lsContacts = new List<ContactViewModel>();

                    if (countBranch > 0) {
                        var branch = db.branch.Where(b => b.cli_document == pClient_id).Select(b => new BranchViewModel { id = b.bra_id, name = b.bra_name }).FirstOrDefault();
                        var lsTmp = db.Contact.Where(ct => ct.bra_id == branch.id).Count();
                       
                        if (lsTmp > 0)
                        {

                            lsContacts = db.Contact.Where(ct => ct.bra_id == branch.id)
                                                        .Select(ct =>
                                                           new ContactViewModel
                                                           {
                                                               id = ct.cnt_id,
                                                               name = ct.cnt_name,
                                                               lastName = ct.cnt_lastName,
                                                               phone = ct.cnt_phone,
                                                               cellPhone = ct.cnt_cellPhone,
                                                               email = ct.cnt_email,
                                                               jobTitle = new JobTitleViewModel { id = ct.JobTitlesClient.jtcl_id, description = ct.JobTitlesClient.jtcl_description },
                                                               adress = ct.cnt_adress,
                                                               branch = new BranchViewModel { id = ct.branch.bra_id, name = ct.branch.bra_name }
                                                           }
                                                        ).ToList();

                        }

                       
                    }
                    return lsContacts;


                }


            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
