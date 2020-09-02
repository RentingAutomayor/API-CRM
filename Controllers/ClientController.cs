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
    public class ClientController : ApiController
    {
        [HttpGet]
        public bool existsClient(string client_id)
        {
            bool rta = false;
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var cli = db.Client.Where(cl => cl.cli_document == client_id).FirstOrDefault();
                    if (cli != null)
                    {
                        rta = true;
                    }
                    else
                    {
                        rta = false;
                    }
                }

            }
            catch (Exception)
            {
                rta = false;
                throw;
            }
            return rta;
        }

        [HttpGet]

        public List<ClientViewModel> Get()
        {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var lsClients = db.Client
                                       .Select(cl => new ClientViewModel { id = cl.cli_document, name = cl.cli_name, lastName = cl.cli_lastName })
                                       .ToList();

                    return lsClients;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet]
        public List<ClientViewModel> GetClientsByDescription(string description)
        {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {

                    var aDesc = description.Split('|');
                    string campo = aDesc[0];
                    string valor = aDesc[1];
                    List<ClientViewModel> lsClients = new List<ClientViewModel>();
                    if (campo == "id")
                    {
                        lsClients = db.Client.Where(cl => cl.cli_document.Contains(valor))
                                            .Select(cl => new ClientViewModel
                                            {
                                                kindOfDocument = new KindOfDocumentViewModel { id = cl.kindOfDocument.kod_id, description = cl.kindOfDocument.kod_description },
                                                id = cl.cli_document,
                                                name = cl.cli_name,
                                                lastName = cl.cli_lastName,
                                                cellPhone = cl.cli_cellPhone,
                                                phone = cl.cli_phone,
                                                email = cl.cli_email,
                                                city = new CityViewModel { id = cl.Cities.cty_id, name = cl.Cities.cty_name, departmentId = cl.Cities.dpt_id },
                                                economicActivity = new EconomicActivityViewModel {
                                                    id = cl.EconomicActivity.ea_id,
                                                    description = cl.EconomicActivity.ea_description 
                                                }                                             
                                            }).Take(10)
                                            .ToList();
                    }
                    else if (campo == "name")
                    {
                        lsClients = db.Client.Where(cl => cl.cli_name.Contains(valor))
                                                .Select(cl => new ClientViewModel
                                                {
                                                    kindOfDocument = new KindOfDocumentViewModel { id = cl.kindOfDocument.kod_id, description = cl.kindOfDocument.kod_description },
                                                    id = cl.cli_document,
                                                    name = cl.cli_name,
                                                    lastName = cl.cli_lastName,
                                                    cellPhone = cl.cli_cellPhone,
                                                    phone = cl.cli_phone,
                                                    email = cl.cli_email,
                                                    city = new CityViewModel { id = cl.Cities.cty_id, name = cl.Cities.cty_name, departmentId = cl.Cities.dpt_id },
                                                    economicActivity = new EconomicActivityViewModel 
                                                    { 
                                                        id = cl.EconomicActivity.ea_id,
                                                        description = cl.EconomicActivity.ea_description 
                                                    },
                                                   
                                                }).Take(10)
                                                .ToList();
                    }


                    return lsClients;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet]
        public IHttpActionResult getMainBranchByClient(string pClient_id)
        {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var branch = db.branch.Where(b => b.cli_document == pClient_id && b.bra_isMain == true)
                                           .Select(b => new BranchViewModel
                                           {
                                               id = b.bra_id,
                                               name = b.bra_name,
                                               adress = b.bra_adress,
                                               phone = b.bra_phone,
                                               client = new ClientViewModel { id = b.Client.cli_document, name = b.Client.cli_name, lastName = b.Client.cli_lastName }
                                           }).FirstOrDefault();
                    return Ok(branch);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [HttpPost]
        public IHttpActionResult AddClient([FromBody] ClientViewModel pClient)
        {
            try
            {
                ResponseViewModel response = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities())
                {
                    Client oClient = new Client();
                    oClient.cli_document = pClient.id;
                    oClient.kod_id = pClient.kindOfDocument.id;
                    oClient.cli_name = pClient.name;
                    oClient.cli_lastName = pClient.lastName;
                    oClient.cli_cellPhone = pClient.cellPhone;
                    oClient.cli_phone = pClient.phone;
                    oClient.cty_id = pClient.city.id;
                    oClient.ea_id = pClient.economicActivity.id;
                    oClient.cli_state = true;
                    oClient.cli_registrationDate = DateTime.Now;
                    oClient.cli_email = pClient.email;
                    


                    db.Client.Add(oClient);
                    db.SaveChanges();

                    branch bdBranch = new branch();
                    if (pClient.kindOfDocument.description.ToUpper() == "NIT")
                    {
                        bdBranch.bra_name = "Principal " + pClient.name;
                    }
                    else
                    {
                        bdBranch.bra_name = "Principal " + pClient.name + " " + pClient.lastName;
                    }

                    bdBranch.bra_isMain = true;
                    bdBranch.cli_document = pClient.id;
                    bdBranch.bra_state = true;

                    db.branch.Add(bdBranch);
                    db.SaveChanges();

                    var branch = db.branch.Where(b => b.cli_document == pClient.id && b.bra_isMain == true)
                                        .Select(b => new BranchViewModel { id = b.bra_id, name = b.bra_name })
                                        .FirstOrDefault();

                    if (pClient.lsContacts != null)
                    {
                        foreach (var cont in pClient.lsContacts)
                        {
                            Contact bdContact = new Contact();
                            bdContact.bra_id = branch.id;
                            bdContact.cnt_name = cont.name;
                            bdContact.cnt_lastName = cont.lastName;
                            bdContact.cnt_cellPhone = cont.cellPhone;
                            bdContact.cnt_phone = cont.phone;
                            bdContact.cnt_email = cont.email;
                            bdContact.cnt_adress = cont.adress;
                            bdContact.jtcl_id = cont.jobTitle.id;

                            db.Contact.Add(bdContact);
                            db.SaveChanges();
                        }
                    }

                    response.response = true;
                    response.message = "Se crea el cliente: " + pClient.name + " " + pClient.lastName;



                    return Ok(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public IHttpActionResult updateClient(ClientViewModel pClient)
        {
            try
            {
                ResponseViewModel response = new ResponseViewModel();

                using (BDRAEntities db = new BDRAEntities())
                {
                    var oClientBd = db.Client.Where(cl => cl.cli_document == pClient.id).FirstOrDefault();
                    oClientBd.kod_id = pClient.kindOfDocument.id;
                    oClientBd.cli_name = pClient.name;
                    oClientBd.cli_lastName = pClient.lastName;
                    oClientBd.cli_cellPhone = pClient.cellPhone;
                    oClientBd.cli_phone = pClient.phone;
                    oClientBd.cli_email = pClient.email;
                    oClientBd.ea_id = pClient.economicActivity.id;                   
                    oClientBd.cty_id = pClient.city.id;

                    db.SaveChanges();

                    var branch = db.branch.Where(b => b.cli_document == pClient.id && b.bra_isMain == true)
                                         .Select(b => new BranchViewModel { id = b.bra_id, name = b.bra_name })
                                         .FirstOrDefault();


                    if (branch == null) {
                        branch bdBranch = new branch();
                        if (pClient.kindOfDocument.description.ToUpper() == "NIT")
                        {
                            bdBranch.bra_name = "Principal " + pClient.name;
                        }
                        else
                        {
                            bdBranch.bra_name = "Principal " + pClient.name + " " + pClient.lastName;
                        }

                        bdBranch.bra_isMain = true;
                        bdBranch.cli_document = pClient.id;
                        bdBranch.bra_state = true;

                        db.branch.Add(bdBranch);
                        db.SaveChanges();

                        branch = db.branch.Where(b => b.cli_document == pClient.id && b.bra_isMain == true)
                                        .Select(b => new BranchViewModel { id = b.bra_id, name = b.bra_name })
                                        .FirstOrDefault();
                    }



                    if (pClient.lsContacts != null)
                    {
                       

                        foreach (var contact in pClient.lsContacts)
                        {
                            if (contact.id == null || contact.id == 0)
                            {
                                Contact bdContact = new Contact();
                                bdContact.bra_id = branch.id;
                                bdContact.cnt_name = contact.name;
                                bdContact.cnt_lastName = contact.lastName;
                                bdContact.cnt_cellPhone = contact.cellPhone;
                                bdContact.cnt_phone = contact.phone;
                                bdContact.cnt_email = contact.email;
                                bdContact.cnt_adress = contact.adress;
                                bdContact.jtcl_id = contact.jobTitle.id;
                                db.Contact.Add(bdContact);
                                db.SaveChanges();
                            }
                            
                        }
                    }

                    response.response = true;
                    response.message = "Se ha actualizado el cliente: " + pClient.name + " " + pClient.lastName;

                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public IHttpActionResult deleteContact(ContactViewModel pContact) {
            try {
                ResponseViewModel rta = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities())
                {
                    var contact = db.Contact.Where(cnt => cnt.cnt_id == pContact.id).FirstOrDefault();
                    db.Contact.Remove(contact);
                    db.SaveChanges();
                    rta.response = true;
                    rta.message = "Se ha eliminado el contacto correctamente";
                    return Ok(rta);
                }

            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

    }
}
