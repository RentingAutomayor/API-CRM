using API_RA_Forms.ViewModels;
using DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_RA_Forms.Controllers
{
    public class PreClientController : ApiController
    {
				[HttpGet]
				public IHttpActionResult GetPreClientsByDescription(string description) {
						try
						{
								var aDesc = description.Split('|');
								string campo = aDesc[0];
								string valor = aDesc[1];
								List<PreClientViewModel> lsPreClients = new List<PreClientViewModel>();

								using (BDRAEntities db = new BDRAEntities()) {
										switch (campo)
										{
												case "cellphone":
														lsPreClients = db.PreClient.Where(pcl => pcl.preCli_cellPhone.Contains(valor))
																												.Select(pcl => new PreClientViewModel
																												{
																														idPreClient = pcl.preCli_id,
																														id = pcl.preCli_document,
																														kindOfDocument = new KindOfDocumentViewModel { id = pcl.kod_id, description = pcl.kindOfDocument.kod_description},
																														name = pcl.preCli_name,
																														lastName = pcl.preCli_lastName,
																														cellPhone = pcl.preCli_cellPhone,
																														email = pcl.preCli_email,
																														city = new CityViewModel { id = pcl.cty_id, departmentId = pcl.Cities.Departments.dpt_id, name = pcl.Cities.cty_name }
																												}).Take(10)
																												.ToList();
														break;
												case "email":
														lsPreClients = db.PreClient.Where(pcl => pcl.preCli_email.Contains(valor))
																												.Select(pcl => new PreClientViewModel
																												{
																														idPreClient = pcl.preCli_id,
																														id = pcl.preCli_document,
																														kindOfDocument = new KindOfDocumentViewModel { id = pcl.kod_id, description = pcl.kindOfDocument.kod_description },
																														name = pcl.preCli_name,
																														lastName = pcl.preCli_lastName,
																														cellPhone = pcl.preCli_cellPhone,
																														email = pcl.preCli_email,
																														city = new CityViewModel { id = pcl.cty_id, departmentId = pcl.Cities.Departments.dpt_id, name = pcl.Cities.cty_name }
																												}).Take(10)
																												.ToList();
														break;
												default:
														break;
										}
								}

								return Ok(lsPreClients);
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

				[HttpGet]
        public IHttpActionResult Get() {
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsPreClients = db.PreClient
																				 .Where(pre => pre.preCli_state == true)
																				 .Select(
																								pre => new PreClientViewModel { 
																										idPreClient = pre.preCli_id,
																										id = pre.preCli_document,
																										kindOfDocument = new KindOfDocumentViewModel { id = pre.kod_id,description = pre.kindOfDocument.kod_description },
																										name = pre.preCli_name,
																										lastName = pre.preCli_lastName,
																										cellPhone = pre.preCli_cellPhone,
																										phone = pre.preCli_phone,
																										email = pre.preCli_email,
																										registrationDate = pre.preCli_registrationDate,
																										user = new UserViewModel { id = pre.usu_document, name = pre.users.usu_name, lastName = pre.users.usu_lastName }
																										
																								}

																						).ToList();
										return Ok(lsPreClients);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
        }

	

				public PreClient GetPreClientBD(PreClientViewModel pPreClient)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var PreClientBD = new PreClient();

										if ((pPreClient.email != null && pPreClient.email != "") && (pPreClient.cellPhone != null && pPreClient.cellPhone != ""))
										{
												PreClientBD = db.PreClient.Where(p => p.preCli_email == pPreClient.email && p.preCli_cellPhone == pPreClient.cellPhone).FirstOrDefault();

												if (PreClientBD == null)
												{
														if (pPreClient.cellPhone != null && pPreClient.cellPhone != "")
														{
																PreClientBD = db.PreClient.Where(p => p.preCli_cellPhone == pPreClient.cellPhone).FirstOrDefault();
														}
														else if (pPreClient.email != null && pPreClient.email != "")
														{
																PreClientBD = db.PreClient.Where(p => p.preCli_email == pPreClient.email).FirstOrDefault();
														}

												}
										}
										//Se deben validar los casos excluyentes  si viene el celular y no el coreo y viceversa
										else if (pPreClient.cellPhone != null && pPreClient.cellPhone != "")
										{
												PreClientBD = db.PreClient.Where(p => p.preCli_cellPhone == pPreClient.cellPhone).FirstOrDefault();
										}
										else if (pPreClient.email != null && pPreClient.email != "")
										{
												PreClientBD = db.PreClient.Where(p => p.preCli_email == pPreClient.email).FirstOrDefault();
										}

										if (PreClientBD != null)
										{
												return PreClientBD;
										}
										else
										{
												return null;
										}
								}

						}
						catch (Exception ex)
						{
								return null;
						}
				}


				public PreClient SetDataToPreClient(PreClientViewModel pPreClient, UserViewModel pUser)
				{
						try
						{
								PreClient oPreClient = new PreClient();

								if (pPreClient.idPreClient != null)
								{
										oPreClient.preCli_id = (int)pPreClient.idPreClient;
								}

								if (pPreClient.id != null && pPreClient.id != "")
								{
										oPreClient.preCli_document = pPreClient.id;
								}
								else
								{
										oPreClient.preCli_document = null;
								}

								if (pPreClient.kindOfDocument != null)
								{
										oPreClient.kod_id = pPreClient.kindOfDocument.id;
								}
								else
								{
										oPreClient.kod_id = null;
								}

								if (pPreClient.name != null && pPreClient.name != "")
								{
										oPreClient.preCli_name = pPreClient.name;
								}
								else
								{
										oPreClient.preCli_name = null;
								}

								if (pPreClient.lastName != null && pPreClient.lastName != "")
								{
										oPreClient.preCli_lastName = pPreClient.lastName;
								}
								else
								{
										oPreClient.preCli_lastName = null;
								}

								if (pPreClient.phone != null && pPreClient.phone != "")
								{
										oPreClient.preCli_phone = pPreClient.phone;
								}
								else
								{
										oPreClient.preCli_phone = null;
								}

								if (pPreClient.cellPhone != null && pPreClient.cellPhone != "")
								{
										oPreClient.preCli_cellPhone = pPreClient.cellPhone;
								}
								else
								{
										oPreClient.preCli_cellPhone = null;
								}

								if (pPreClient.email != null && pPreClient.email != "")
								{
										oPreClient.preCli_email = pPreClient.email;
								}
								else
								{
										oPreClient.preCli_email = null;
								}

								if (pPreClient.city != null)
								{
										oPreClient.cty_id = pPreClient.city.id;
								}

								oPreClient.preCli_registrationDate = DateTime.Now;

								oPreClient.usu_document = pUser.id;


								oPreClient.preCli_state = true;

								return oPreClient;
						}
						catch (Exception ex)
						{
								return null;
						}

				}

				[HttpPost]
				public IHttpActionResult UpdatePreClient(PreClientViewModel pPreClient) {
						try
						{
								ResponseViewModel rta = new ResponseViewModel();
								using (BDRAEntities db = new BDRAEntities())
								{

										var preClientBd = db.PreClient.Where(p => p.preCli_id == pPreClient.idPreClient).FirstOrDefault();
										preClientBd.preCli_document = pPreClient.id;
										preClientBd.kod_id = pPreClient.kindOfDocument.id;
										preClientBd.preCli_name = pPreClient.name;
										preClientBd.preCli_lastName = pPreClient.lastName;										
										preClientBd.preCli_cellPhone = pPreClient.cellPhone;
										preClientBd.preCli_phone = pPreClient.phone;
										preClientBd.preCli_email = pPreClient.email;
										preClientBd.cty_id = pPreClient.city.id;

										db.SaveChanges();

										rta.response = true;
										rta.message = "Se ha actualizado el pre-cliente " + pPreClient.idPreClient;

										return Ok(rta);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				
				}


				public bool CreatePreClientInBD(PreClient pPreClient)
				{
						try
						{
								PreClient oPreClientBd = new PreClient();
								using (BDRAEntities db = new BDRAEntities())
								{
										db.PreClient.Add(pPreClient);
										db.SaveChanges();
										return true;
								}
						}
						catch (Exception ex)
						{
								return false;
						}
				}

			

		}
}
