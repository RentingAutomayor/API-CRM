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
										var lsPreRequest = db.STRPRC_GET_ALL_PRE_REQUEST();
										var lsPreRequestResponse = new List<PreRequestViewModel>();

										foreach (var preRequestBD in lsPreRequest)
										{
												PreRequestViewModel preRequest = new PreRequestViewModel();
												preRequest.id = preRequestBD.code;
												preRequest.preClient = new PreClientViewModel();
												preRequest.preClient.name = preRequestBD.name;
												preRequest.preClient.lastName = preRequestBD.lastName;
												preRequest.preClient.cellPhone = preRequestBD.cellphone;
												preRequest.preClient.email = preRequestBD.email;
												preRequest.vehicleModel = new VehicleModelViewModel();
												preRequest.vehicleModel.name = preRequestBD.vehicleModel;
												preRequest.stateRequest = new StateViewModel();
												preRequest.stateRequest.description = preRequestBD.state;
												preRequest.firstCanal = new CanalViewModel();
												preRequest.firstCanal.description = preRequestBD.firstCanal;
												preRequest.secondCanal = new CanalViewModel();
												preRequest.secondCanal.description = preRequestBD.secondCanal;
												preRequest.registrationDate = preRequestBD.registrationDate;

												lsPreRequestResponse.Add(preRequest);
										}

										return Ok(lsPreRequestResponse);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}
				[HttpGet]
				public IHttpActionResult GetPreRequestByFilter(string pKindOfFilter, string pValue)
				{
						try
						{
								List<PreRequestViewModel> lsPreRequestResponse = new List<PreRequestViewModel>();
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsPreRequestBd = db.STRPRC_GET_PRE_REQUEST_BY_FILTER_VALUE(pKindOfFilter, pValue);

										foreach (var preRequestDB in lsPreRequestBd)
										{
												PreRequestViewModel preRequest = new PreRequestViewModel();
												preRequest.id = int.Parse(preRequestDB.code);
												preRequest.preClient = new PreClientViewModel();
												preRequest.preClient.name = preRequestDB.name;
												preRequest.preClient.lastName = preRequestDB.lastName;
												preRequest.preClient.cellPhone = preRequestDB.cellphone;
												preRequest.preClient.email = preRequestDB.email;
												preRequest.vehicleModel = new VehicleModelViewModel();
												preRequest.vehicleModel.name = preRequestDB.vehicleModel;
												preRequest.stateRequest = new StateViewModel();
												preRequest.stateRequest.description = preRequestDB.state;
												preRequest.firstCanal = new CanalViewModel();
												preRequest.firstCanal.description = preRequestDB.firstCanal;
												preRequest.secondCanal = new CanalViewModel();
												preRequest.secondCanal.description = preRequestDB.secondCanal;
												preRequest.registrationDate = preRequestDB.registrationDate;
												lsPreRequestResponse.Add(preRequest);
										}
								}
								return Ok(lsPreRequestResponse);
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

										if (PreClientBD == null)
										{
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

										if (pPreRequest.vehicleModel.id != null && pPreRequest.vehicleModel.id != 0)
										{
												oPreRequest.vehMdl_id = pPreRequest.vehicleModel.id;
										}
										else 
										{
												var vechicleModel = VehicleModelController.SetDataToVehicleModel(pPreRequest.vehicleModel);
												var vehicleModelDB = VehicleModelController.CreateVehicleModelInDB(vechicleModel);
												oPreRequest.vehMdl_id = vehicleModelDB.id;
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

										if (pPreRequest.lsObservation[0].observation != "") {
												PreRequest_Observation observation = this.SetDataToObservation(lastRequest.preReq_id, pPreRequest.lsObservation[0].observation, pPreRequest.user.id);
												this.InsertObservationInDB(observation);
										}
										

										rta.response = true;
										rta.message = "Se ha creado la pre solicitud: " + lastRequest.preReq_id + " de cliente de persona natural";

										return Ok(rta);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}


				public PreRequest GetTheLastPreRequestByPreClient(PreClient pPreClient)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
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
				public IHttpActionResult UpdatePreRequest(PreRequestViewModel pPreRequest)
				{
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

										if (pPreRequest.vehicleModel.id != null && pPreRequest.vehicleModel.id != 0)
										{
												preRequestBD.vehMdl_id = pPreRequest.vehicleModel.id;
										}
										else
										{
												var vechicleModel = VehicleModelController.SetDataToVehicleModel(pPreRequest.vehicleModel);
												var vehicleModelDB = VehicleModelController.CreateVehicleModelInDB(vechicleModel);
												preRequestBD.vehMdl_id = vehicleModelDB.id;
										}						

										preRequestBD.preCli_id = pPreRequest.preClient.idPreClient;								
										db.SaveChanges();

										if (pPreRequest.lsObservation[0].observation != "")
										{
												int idPreRequest = int.Parse(pPreRequest.id.ToString());
												PreRequest_Observation observation = this.SetDataToObservation(idPreRequest, pPreRequest.lsObservation[0].observation, pPreRequest.user.id);
												this.InsertObservationInDB(observation);
										}
												
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


				[HttpGet]
				public IHttpActionResult GetPreRequestById(int pPreRequest_id)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var preRequestDB = db.PreRequest.Where(pr => pr.preReq_state == true && pr.preReq_id == pPreRequest_id)
																			.Select(pr => new PreRequestViewModel
																			{
																					id = pr.preReq_id,
																					registrationDate = pr.preReq_registrationDate,
																					preClient = new PreClientViewModel
																					{
																							idPreClient = pr.preCli_id,
																							id = pr.PreClient.preCli_document,
																							kindOfDocument = new KindOfDocumentViewModel { id = pr.PreClient.kindOfDocument.kod_id, description = pr.PreClient.kindOfDocument.kod_description },
																							name = pr.PreClient.preCli_name,
																							lastName = pr.PreClient.preCli_lastName,
																							phone = pr.PreClient.preCli_phone,
																							cellPhone = pr.PreClient.preCli_cellPhone,
																							email = pr.PreClient.preCli_email,
																							city = new CityViewModel { id = pr.PreClient.Cities.cty_id, name = pr.PreClient.Cities.cty_name, departmentId = pr.PreClient.Cities.dpt_id }
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
																				).FirstOrDefault();
										
										preRequestDB.lsObservation = new List<PreRequestObservationViewModel>();
										preRequestDB.lsObservation = this.GetObservationsByPreRequest(pPreRequest_id);

										return Ok(preRequestDB);

								}
								
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

				private List<PreRequestObservationViewModel> GetObservationsByPreRequest(int pPreRequest_id) {
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsObservationDB = db.PreRequest_Observation
																										.Where(obs => obs.preReq_id == pPreRequest_id)
																										.Select(obs => new PreRequestObservationViewModel
																										{
																												id = obs.prObs_id,
																												observation = obs.prObs_observation,
																												user = new UserViewModel
																												{
																														id = obs.usu_document,
																														name = obs.users.usu_name,
																														lastName = obs.users.usu_lastName
																												},
																												registrationDate = obs.prObs_registrationDate

																										}).ToList();
										return lsObservationDB;

								}
								

						}
						catch (Exception ex)
						{
								return null;	
						}
				}

				private PreRequest_Observation SetDataToObservation(int pPreRequest_id,string pObservation,string pUsu_document) {
						PreRequest_Observation Observation = new PreRequest_Observation();
						Observation.preReq_id = pPreRequest_id;
						Observation.prObs_observation = pObservation;
						Observation.usu_document = pUsu_document;
						Observation.prObs_registrationDate = DateTime.Now;
						return Observation;
				}

				private bool InsertObservationInDB(PreRequest_Observation pPbservation) {
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										db.PreRequest_Observation.Add(pPbservation);
										db.SaveChanges();
										return true;
								}
								
						}
						catch (Exception ex)
						{
								return false;
						}
				}

				[HttpGet]
				public IHttpActionResult GetDataToExport() {
						try
						{
								using (BDRAEntities db = new BDRAEntities()) {
										List<DataStructureForFilePN> lsDataPN = new List<DataStructureForFilePN>();
										var lsPrReqeust = db.STRPRC_GET_DATA_TO_EXPORT_FILE_PN();

										foreach (var dtPrRequest in lsPrReqeust)
										{
												DataStructureForFilePN dataPN = new DataStructureForFilePN();
												dataPN.consecutivo = dtPrRequest.Consecutivo;
												dataPN.fechaRegistro = dtPrRequest.Fecha_de_registro;
												dataPN.cliente = dtPrRequest.Cliente;
												dataPN.email = dtPrRequest.Email;
												dataPN.celular = dtPrRequest.Celular;
												dataPN.ciudad = dtPrRequest.Ciudad;
												dataPN.estado = dtPrRequest.Estado;
												dataPN.lineaDeVehiculo = dtPrRequest.Linea_del_vehiculo;
												dataPN.canalPrimario = dtPrRequest.Canal_primario;
												dataPN.canalSecundario = dtPrRequest.Canal_secundario;
												dataPN.observaciones = dtPrRequest.Observaciones;
												dataPN.gerenteDeCuenta = dtPrRequest.Gerente_de_cuenta;

												lsDataPN.Add(dataPN);

										}

										return Ok(lsDataPN);
								}
								
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);								
						}
				}
		}
}
