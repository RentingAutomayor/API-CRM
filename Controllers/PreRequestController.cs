using API_RA_Forms.ViewModels;
using DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace API_RA_Forms.Controllers
{
		public class PreRequestController : ApiController
		{
				private PreClientController objPreClientController;

				[HttpGet]
				public IHttpActionResult Get()
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsPreRequest = db.PreRequest.Where(pr => pr.preReq_state == true)
																			.Select(pr => new PreRequestViewModel
																			{
																					id = pr.preReq_id,
																					registrationDate = pr.preReq_registrationDate,
																					preClient = new PreClientViewModel
																					{
																							idPreClient = pr.preCli_id,
																							id = pr.PreClient.preCli_document,
																							kindOfDocument = new KindOfDocumentViewModel { id = pr.PreClient.kindOfDocument.kod_id, description =					  pr.PreClient.kindOfDocument.kod_description },
																							name = pr.PreClient.preCli_name,
																							lastName = pr.PreClient.preCli_lastName,
																							phone = pr.PreClient.preCli_phone,
																							cellPhone = pr.PreClient.preCli_cellPhone,
																							email = pr.PreClient.preCli_email,
																							city = new CityViewModel { id = pr.PreClient.Cities.cty_id, name = pr.PreClient.Cities.cty_name,								departmentId= pr.PreClient.Cities.dpt_id }
																					},
																					vehicleModel = new VehicleModelViewModel
																					{
																							id = pr.vehMdl_id,
																							name = pr.VehicleModel.vehMdl_name,
																							description = pr.VehicleModel.vehMdl_description
																					},
																					stateRequest = new StateViewModel
																					{
																							id = pr.sta_id,
																							description = pr.states.sta_description
																					},
																					state = pr.preReq_state,
																					user = new UserViewModel
																					{
																							id = pr.users.usu_document,
																							name = pr.users.usu_name,
																							lastName = pr.users.usu_lastName
																					},
																					firstCanal = new CanalViewModel
																					{
																							id = pr.firstCanal_id,
																							description = pr.Canal.cnl_description
																					},
																					secondCanal = new CanalViewModel
																					{
																							id = pr.secondCanal_id,
																							description = ""
																					}

																			}
																				)
																			.OrderByDescending(rqt => rqt.id)
																			.ToList();

										return Ok(lsPreRequest);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}


				[HttpPost]
				public IHttpActionResult AddNewPreRequest(PreRequestViewModel pPreRequest)
				{
						try
						{
								this.objPreClientController = new PreClientController();
								ResponseViewModel rta = new ResponseViewModel();

								using (BDRAEntities db = new BDRAEntities())
								{
										/*
												Validamos si el PreCliente de la solicitud ya existe en la base de datos.
										 */

										PreClient PreClientBD = new PreClient();

										PreClientBD = objPreClientController.GetPreClientBD(pPreRequest.preClient);

										if (PreClientBD == null) {
												PreClientBD = objPreClientController.SetDataToPreClient(pPreRequest.preClient, pPreRequest.user);
												var preClientCreated = objPreClientController.CreatePreClientInBD(PreClientBD);
												if (preClientCreated)
												{
														PreClientBD = objPreClientController.GetPreClientBD(pPreRequest.preClient);
												}
										}
							

										PreRequest oPreRequest = new PreRequest();
										oPreRequest.preReq_registrationDate = DateTime.Now;
										oPreRequest.preCli_id = PreClientBD.preCli_id;
										oPreRequest.preReq_state = true;
										oPreRequest.usu_document = pPreRequest.user.id;

										if (pPreRequest.vehicleModel != null)
										{
												oPreRequest.vehMdl_id = pPreRequest.vehicleModel.id;
										}
										if (pPreRequest.stateRequest != null)
										{
												oPreRequest.sta_id = pPreRequest.stateRequest.id;
										}
										if (pPreRequest.firstCanal != null)
										{
												oPreRequest.firstCanal_id = pPreRequest.firstCanal.id;
										}
										if (pPreRequest.secondCanal != null)
										{
												oPreRequest.secondCanal_id = pPreRequest.secondCanal.id;
										}


										db.PreRequest.Add(oPreRequest);
										db.SaveChanges();

										var lastRequest = GetTheLastPreRequestByPreClient(PreClientBD);

										rta.response = true;
										rta.message = "Se ha creado la pre solicitud: "+ lastRequest .preReq_id+ " de cliente de persona natural";

										return Ok(rta);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}


				public PreRequest GetTheLastPreRequestByPreClient(PreClient pPreClient) {
						try
						{
								using (BDRAEntities db = new BDRAEntities()) {
										var lastRequest = db.PreRequest.Where(p => p.preCli_id == pPreClient.preCli_id)
																										.OrderByDescending(p => p.preReq_registrationDate)
																										.FirstOrDefault();
										return lastRequest;
								}
										
						}
						catch (Exception)
						{
								return null;								
						}
				
				}

				[HttpPost]
				public IHttpActionResult UpdatePreRequest(PreRequestViewModel pPreRequest) {
						try
						{
								ResponseViewModel rta = new ResponseViewModel();

								using (BDRAEntities db = new BDRAEntities())
								{
										//TODO: Update client
										var preRequestBD = db.PreRequest.Where(p => p.preReq_id == pPreRequest.id).FirstOrDefault();
										preRequestBD.firstCanal_id = pPreRequest.firstCanal.id;
										preRequestBD.secondCanal_id = pPreRequest.secondCanal.id;
										preRequestBD.sta_id = pPreRequest.stateRequest.id;
										preRequestBD.vehMdl_id = pPreRequest.vehicleModel.id;
										preRequestBD.preCli_id = pPreRequest.preClient.idPreClient;

										db.SaveChanges();
										rta.response = true;
										rta.message = "Se ha actualizado la solicitud: " + pPreRequest.id;

										return Ok(rta);
								}								
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

				[HttpDelete]
				public IHttpActionResult DeleteRequestByID(string idPreRequest)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										int idPreRqt = int.Parse(idPreRequest);
										var rta = new ResponseViewModel();
										var objPreRequest = db.PreRequest.Where(p => p.preReq_id == idPreRqt).FirstOrDefault();
										objPreRequest.preReq_state = false;										
										db.SaveChanges();

										rta.response = true;
										rta.message = "Se ha eliminado correctamente la pre solicitud: " + objPreRequest.preReq_id;
										return Ok(rta);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

		}
}
