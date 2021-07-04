using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Security.Permissions;
using System.Text;
using System.Web.Http;
using API_RA_Forms.ViewModels;
using DAO;
using Newtonsoft.Json;

namespace API_RA_Forms.Controllers
{
		public class RequestController : ApiController
		{
				[HttpGet]
				public IHttpActionResult GetAllRequest()
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{

										var lsRequest = db.STRPRCD_GET_ALL_REQUEST();
										List<RequestViewModel> lsRequestResponse = new List<RequestViewModel>();
										foreach (var request in lsRequest)
										{
												RequestViewModel rqt = new RequestViewModel();
												rqt.id = request.code;
												rqt.client = new ClientViewModel();
												rqt.client.name = request.name;
												rqt.client.lastName = request.lastName;
												rqt.probability = new ProbabilityViewModel();
												rqt.probability.description = request.probability;
												rqt.parentState = new StateViewModel();
												rqt.parentState.description = request.primaryState;
												rqt.childState = new StateViewModel();
												rqt.childState.description = request.secondState;
												rqt.registrationDate = request.registrationDate;
												rqt.user = new UserViewModel();
												rqt.user.name = request.userName;
												rqt.user.lastName = request.userLastName;
												lsRequestResponse.Add(rqt);
										}


										return Ok(lsRequestResponse);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
								throw;
						}
				}
				[HttpGet]
				public IHttpActionResult GetRequestByFilter(string pKindOfFilter, string pValue)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsRequestFiltered = db.STRPRC_GET_REQUEST_BY_FILTER_VALUE(pKindOfFilter, pValue);
										List<RequestViewModel> lsRequestResponse = new List<RequestViewModel>();
										foreach (var request in lsRequestFiltered)
										{
												RequestViewModel rqt = new RequestViewModel();
												rqt.id = int.Parse(request.code);
												rqt.client = new ClientViewModel();
												rqt.client.name = request.name;
												rqt.client.lastName = request.lastName;
												rqt.probability = new ProbabilityViewModel();
												rqt.probability.description = request.probability;
												rqt.parentState = new StateViewModel();
												rqt.parentState.description = request.primaryState;
												rqt.childState = new StateViewModel();
												rqt.childState.description = request.secondState;
												rqt.registrationDate = request.registrationDate;
												rqt.user = new UserViewModel();
												rqt.user.name = request.userName;
												rqt.user.lastName = request.userLastName;
												lsRequestResponse.Add(rqt);
										}


										return Ok(lsRequestResponse);
								}

						}
						catch (Exception ex)
						{

								return BadRequest(ex.Message);
						}
				}

				[HttpGet]
				public IHttpActionResult GetRequestById(int pRequest_id)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var RequestDB = db.Request.Where(r => r.rqt_id == pRequest_id && r.rqt_state == true)
																						.Select(r => new RequestViewModel
																						{
																								id = r.rqt_id,
																								client = new ClientViewModel
																								{
																										id = r.Client.cli_document,
																										kindOfDocument = new KindOfDocumentViewModel
																										{
																												id = r.Client.kindOfDocument.kod_id,
																												description = r.Client.kindOfDocument.kod_description
																										},
																										name = r.Client.cli_name,
																										lastName = r.Client.cli_lastName,
																										phone = r.Client.cli_phone,
																										cellPhone = r.Client.cli_cellPhone,
																										email = r.Client.cli_email,
																										city = new CityViewModel
																										{
																												id = r.Client.Cities.cty_id,
																												name = r.Client.Cities.cty_name,
																												departmentId = r.Client.Cities.dpt_id
																										},
																										economicActivity = new EconomicActivityViewModel
																										{
																												id = r.Client.EconomicActivity.ea_id,
																												description = r.Client.EconomicActivity.ea_description
																										}
																								},
																								probability = new ProbabilityViewModel
																								{
																										id = r.probability.prb_id,
																										description = r.probability.prb_description
																								},
																								parentState = new StateViewModel
																								{
																										id = r.states.sta_id,
																										description = r.states.sta_description
																								},
																								childState = new StateViewModel
																								{
																										id = r.states1.sta_id,
																										description = r.states1.sta_description
																								},
																								user = new UserViewModel
																								{
																										id = r.users.usu_document,
																										name = r.users.usu_name,
																										lastName = r.users.usu_lastName
																								},
																								contact = new ContactViewModel
																								{
																										id = (r.Contact != null)? r.Contact.cnt_id : 0,
																										name = (r.Contact != null) ? r.Contact.cnt_name:"",
																										lastName = (r.Contact != null) ? r.Contact.cnt_lastName:"",
																										state = (r.Contact != null) ? r.Contact.cnt_state: false
																								},
																								canal = new CanalViewModel
																								{
																										id = r.cnl_id,
																										description = r.Canal.cnl_description
																								},
																								observation = r.rqt_observation,
																								initialDate = r.rqt_firstVisitDate,
																								lastDate = r.rqt_lastVisitDate,
																								registrationDate = r.rqt_registrationDate
																						}).FirstOrDefault();


										var riskInformation = db.riskInformationByRequest.Where(ri => ri.rqt_id == RequestDB.id && ri.ribr_state == true).FirstOrDefault();

										if (riskInformation.states != null)
										{
												var oState = new StateViewModel();
												oState.id = riskInformation.states.sta_id;
												oState.description = riskInformation.states.sta_description;

												RequestDB.riskInformation = new RiskInformationViewModel()
												{
														id = riskInformation.ribr_id,
														riskState = oState,
														ammountApproved = long.Parse(riskInformation.ribr_ammountApproved.ToString()),
														dateApproved = riskInformation.ribr_dateApproved,
														datefiling = riskInformation.ribr_dateFiling
												};
										}



										var operationalInformation = db.operationalInformationByRequest.Where(oi => oi.rqt_id == RequestDB.id && oi.oibr_state == true).FirstOrDefault();


										RequestDB.operationalInformation = new OperationalInformationViewModel()
										{
												id = operationalInformation.oibr_id,
												deliveredAmmount = Decimal.Parse(operationalInformation.oibr_deliveredAmmount.ToString()),
												deliveredVehicles = int.Parse(operationalInformation.oibr_deliveredVehicles.ToString()),
												deliveredDate = operationalInformation.oibr_deliveredDate,
												legalizationDate = operationalInformation.oibr_legalizationDate
										};




										try
										{

												var branch = db.branch.Where(br => br.cli_document == RequestDB.client.id).FirstOrDefault();
												var lsContactsByClient = db.Contact.Where(cnt => cnt.bra_id == branch.bra_id)
																											.Select(cnt => new ContactViewModel
																											{
																													id = cnt.cnt_id,
																													name = cnt.cnt_name,
																													lastName = cnt.cnt_lastName,
																													phone = cnt.cnt_phone,
																													cellPhone = cnt.cnt_cellPhone,
																													email = cnt.cnt_email,
																													jobTitle = new JobTitleViewModel { id = cnt.JobTitlesClient.jtcl_id, description = cnt.JobTitlesClient.jtcl_description },
																													adress = cnt.cnt_adress,
																													branch = new BranchViewModel { id = cnt.branch.bra_id, name = cnt.branch.bra_name }
																											}).ToList();


												RequestDB.client.lsContacts = lsContactsByClient;
										}
										catch (Exception ex)
										{
												Console.WriteLine("El cliente no tiene contactos");

										}
										return Ok(RequestDB);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

				[HttpPost]
				public IHttpActionResult addRequest(RequestViewModel pRequest)
				{
						try
						{
								ResponseViewModel response = new ResponseViewModel();
								using (BDRAEntities db = new BDRAEntities())
								{
										Request oRequest = new Request();
										oRequest.rqt_registrationDate = DateTime.Now;
										oRequest.rqt_firstVisitDate = pRequest.initialDate;
										oRequest.rqt_lastVisitDate = pRequest.lastDate;
										oRequest.prb_id = pRequest.probability.id;
										oRequest.cli_document = pRequest.client.id;
										oRequest.usu_document = pRequest.user.id;
										oRequest.rqt_primaryState = pRequest.parentState.id;
										oRequest.rqt_secondState = pRequest.childState.id;
										oRequest.rqt_state = true;
										if (pRequest.contact != null) {
												if (pRequest.contact.id != 0)
												{
														oRequest.cnt_id = pRequest.contact.id;
												}												
										}									
										oRequest.cnl_id = pRequest.canal.id;
										oRequest.rqt_observation = pRequest.observation;
										db.Request.Add(oRequest);
										db.SaveChanges();

										var lastRequest = db.Request.Where(r => r.cli_document == pRequest.client.id)
																								.OrderByDescending(r => r.rqt_registrationDate)
																								.Select(r => new RequestViewModel { id = r.rqt_id })
																								.FirstOrDefault();

										riskInformationByRequest riskInformation = new riskInformationByRequest();
										riskInformation.rqt_id = lastRequest.id;
										riskInformation.ribr_ammountApproved = 0;
										riskInformation.ribr_state = true;

										db.riskInformationByRequest.Add(riskInformation);
										db.SaveChanges();

										operationalInformationByRequest operationalInformation = new operationalInformationByRequest();
										operationalInformation.rqt_id = lastRequest.id;
										operationalInformation.oibr_deliveredAmmount = 0;
										operationalInformation.oibr_deliveredVehicles = 0;
										operationalInformation.oibr_state = true;

										db.operationalInformationByRequest.Add(operationalInformation);
										db.SaveChanges();

										response.response = true;
										response.message = "Se crea la solicitud N° " + lastRequest.id;
										return Ok(response);
								}
						}
						catch (Exception e)
						{
								return BadRequest(e.Message);
								throw;
						}
				}


				[HttpPost]
				public IHttpActionResult deleteRequest(RequestViewModel pRq)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var requestRa = db.Request.Where(rq => rq.rqt_id == pRq.id).FirstOrDefault();
										requestRa.rqt_state = false;

										db.SaveChanges();

										return Ok(true);

								}
						}
						catch (Exception e)
						{
								return BadRequest(e.Message);
								throw;
						}
				}

				[HttpGet]
				public IHttpActionResult getProbabilities()
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsProbabilities = db.probability.Where(p => p.prb_state == true)
																												.Select(p => new ProbabilityViewModel { id = p.prb_id, description = p.prb_description })
																												.ToList();
										return Ok(lsProbabilities);
								}

						}
						catch (Exception e)
						{
								return BadRequest(e.Message);
						}
				}


				[HttpGet]
				public IHttpActionResult getParentStates(string pDescription)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										List<StateViewModel> lsParentStates = new List<StateViewModel>();

										if (pDescription.ToUpper() == "TODOS")
										{
												lsParentStates = db.states.Where(st => st.sta_parentState == null && st.stateGroup.stGrp_id != 2 && st.stateGroup.stGrp_id != 4)
																									.Select(st => new StateViewModel { id = st.sta_id, description = st.sta_description })
																									.ToList();
										}
										else
										{
												var grpState = db.stateGroup.Where(sg => sg.stGrp_description.ToUpper() == pDescription.ToUpper()).FirstOrDefault();
												lsParentStates = db.states.Where(st => st.sta_parentState == null && st.stGrp_id == grpState.stGrp_id)
																										 .Select(st => new StateViewModel { id = st.sta_id, description = st.sta_description })
																										 .ToList();
										}

										return Ok(lsParentStates);
								}

						}
						catch (Exception e)
						{
								return BadRequest(e.Message);
						}
				}


				[HttpGet]
				public IHttpActionResult getStatesByParent(int parentState_id)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										List<StateViewModel> lsChildStates = new List<StateViewModel>();

										if (parentState_id == 0)
										{
												lsChildStates = db.states.Where(st => st.sta_parentState != null && st.stateGroup.stGrp_id != 2)
																							 .Select(st => new StateViewModel { id = st.sta_id, description = st.sta_description, parentState = st.sta_parentState })
																							 .ToList();
										}
										else
										{
												lsChildStates = db.states.Where(st => st.sta_parentState == parentState_id)
																									 .Select(st => new StateViewModel { id = st.sta_id, description = st.sta_description, parentState = st.sta_parentState })
																									 .ToList();
										}

										return Ok(lsChildStates);
								}
						}
						catch (Exception e)
						{
								return BadRequest(e.Message);
						}
				}

				[HttpPost]
				public IHttpActionResult updateRiskInformationPerRequest(RequestViewModel pRequest)
				{
						try
						{
								ResponseViewModel response = new ResponseViewModel();
								using (BDRAEntities db = new BDRAEntities())
								{
										var oRiskInformation = db.riskInformationByRequest.Where(ri => ri.rqt_id == pRequest.id && ri.ribr_state == true)
																																		.FirstOrDefault();

										oRiskInformation.sta_id = pRequest.riskInformation.riskState.id;

										if (pRequest.riskInformation.datefiling != null)
										{
												oRiskInformation.ribr_dateFiling = pRequest.riskInformation.datefiling;
										}

										if (pRequest.riskInformation.ammountApproved != 0)
										{
												oRiskInformation.ribr_ammountApproved = pRequest.riskInformation.ammountApproved;
										}

										if (pRequest.riskInformation.dateApproved != null) {
												oRiskInformation.ribr_dateApproved = pRequest.riskInformation.dateApproved;
										}

										oRiskInformation.ribr_dateUpdateRow = DateTime.Now;
										oRiskInformation.usu_document = pRequest.riskInformation.user.id;

										db.SaveChanges();
										response.response = true;
										response.message = "se ha actualizado la solicitud: " + pRequest.id;
								}
								return Ok(response);
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
								throw;
						}
				}

				[HttpPost]
				public IHttpActionResult updateOperationalInformationPerRequest(RequestViewModel pRequest)
				{
						try
						{
								ResponseViewModel response = new ResponseViewModel();

								using (BDRAEntities db = new BDRAEntities())
								{
										if (pRequest.parentState != null && pRequest.childState != null)
										{
												var RequestBD = db.Request.Where(r => r.rqt_id == pRequest.id).FirstOrDefault();
												RequestBD.rqt_primaryState = pRequest.parentState.id;
												RequestBD.rqt_secondState = pRequest.childState.id;

												db.SaveChanges();

										}

										var operationalInformation = db.operationalInformationByRequest.Where(oi => oi.rqt_id == pRequest.id && oi.oibr_state == true).FirstOrDefault();

										operationalInformation.oibr_deliveredVehicles = short.Parse(pRequest.operationalInformation.deliveredVehicles.ToString());
										operationalInformation.oibr_deliveredAmmount = long.Parse(pRequest.operationalInformation.deliveredAmmount.ToString());
										operationalInformation.oibr_legalizationDate = pRequest.operationalInformation.legalizationDate;
										operationalInformation.oibr_deliveredDate = pRequest.operationalInformation.deliveredDate;

										operationalInformation.usu_document = pRequest.operationalInformation.user.id;
										operationalInformation.oibr_dateUpdateRow = DateTime.Now;

										db.SaveChanges();
										response.response = true;
										response.message = "Se actualiza la solicitud: " + pRequest.id;

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
				public IHttpActionResult updateRequest(RequestViewModel pRequest)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{

										var oRequest = db.Request.Where(r => r.rqt_id == pRequest.id).FirstOrDefault();
										oRequest.rqt_firstVisitDate = pRequest.initialDate;
										oRequest.rqt_lastVisitDate = pRequest.lastDate;
										oRequest.prb_id = pRequest.probability.id;
										oRequest.rqt_primaryState = pRequest.parentState.id;
										oRequest.rqt_secondState = pRequest.childState.id;
										if (pRequest.contact != null)
										{
												if (pRequest.contact.id != 0) {
														oRequest.cnt_id = pRequest.contact.id;
												}												
										}
										oRequest.cnl_id = pRequest.canal.id;
										oRequest.rqt_observation = pRequest.observation;

										db.SaveChanges();

										ResponseViewModel response = new ResponseViewModel();
										response.response = true;
										response.message = "Se ha actualizado la solicitud: " + pRequest.id;

										return Ok(response);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
								throw;
						}
				}

				[HttpGet]
				public IHttpActionResult GetDataToExportFile()
				{
						try
						{
								List<DataStructureForFile> lsDataFile = new List<DataStructureForFile>();
								using (BDRAEntities db = new BDRAEntities())
								{
										db.Database.CommandTimeout = 300;
										var lsDataToExportFile = db.STRPRC_GetData_To_ExportFile();

										foreach (var data in lsDataToExportFile)
										{
												DataStructureForFile row = new DataStructureForFile();
												row.nit = data.NIT;
												row.consecutivo = data.Consecutivo;
												row.canal = data.Canal;
												row.cliente = data.Cliente;
												row.gerenteRenting = data.GerenteRenting;
												row.vp = data.VP;
												row.banca = data.Banca;
												row.segmento = data.Segmento;
												row.regional = data.Regional;
												row.cne = data.CNE;
												row.ccBanco = data.CCBanco;
												row.gerenteBanco = data.GerenteBanco;
												row.departamentoEmpresa = data.DepartamentoEmpresa;
												row.ciudadEmpresa = data.CiudadEmpresa;
												row.daneEmpresa = data.DaneEmpresa;
												row.contactoEmpresa = data.ContactoEmpresa;
												row.cargo = data.Cargo;
												row.telefono = data.Telefono;
												row.celular = data.Celular;
												row.direccion = data.Direccion;
												row.correo = data.Correo;
												row.actividadEconomica = data.ActividadEconomica;
												row.codigoActividadEconomica = data.CodigoActEconomica;
												row.observaciones = data.Observaciones;										
												row.fechaVisita = (data.FechaVisita != null)?data.FechaVisita.Substring(0, 10):"";											
												row.fechaUltimaVisita = (data.FechaUltimaVisita != null)?data.FechaUltimaVisita.Substring(0, 10):"";											
												row.estadoPrincipal = data.EstadoPrincipal;
												row.estadoSecundario = data.EstadoSecundario;
												row.tercerEstado = (data.TercerEstado != null)? data.TercerEstado:"" ;
												row.probabilidad = data.Probabilidad;
												row.decisionRiesgo = data.DecisionRiesgo;											
												row.fechaRadicacionRiesgo = (data.FechaRadicacionRiesgo != null)?data.FechaRadicacionRiesgo.Substring(0, 10):"";
												row.fechaAprobacion = (data.FechaAprobacion != null) ? data.FechaAprobacion.Substring(0, 10) : "";
												row.montoAprobado = data.MontoAprobado;
												row.vehiculosEntregados = data.VehiculosEntregados;
												row.montoActivosEntregado = data.MontoActivosEntregados;												
												row.fechaLegalizacion = (data.FechaLegalizacion != null)?data.FechaLegalizacion.Substring(0, 10):"";											
												row.fechaEntrega = (data.FechaEntrega != null)?data.FechaEntrega.Substring(0,10):"";												
												row.usuarioCreacionRegistro = data.UsuarioCreacionRegistro;												
												row.fechaCreacion = (data.FechaCreacion != null)?data.FechaCreacion.Substring(0, 10):"";												
												row.usuarioActualizacionRiesgo = data.UsuarioActualizacionRegistro;											
												row.fechaActualizacion = (data.FechaActualizacion != null)?data.FechaActualizacion.Substring(0, 10):"";											
												row.usuarioActualizacionRiesgoOP = data.UsuarioActualizacionRiesgoOP;												
												row.fechaActualizacionRiesgoOp = (data.FechaActualizacionRiesgoOP != null)? data.FechaActualizacionRiesgoOP.Substring(0, 10):"";
												lsDataFile.Add(row);
										}

										return Ok(lsDataFile);
								}

						}
						catch (Exception ex)
						{
								return BadRequest(ex.StackTrace);
						}
				}





		}
}
