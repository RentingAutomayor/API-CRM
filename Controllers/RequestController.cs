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

        public IHttpActionResult GetRequestByFilter(string pKindOfFilter, string pValue) {
						try
						{
                using (BDRAEntities db = new BDRAEntities()) {
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
        public IHttpActionResult GetAllRequest() {
            var clientController = new ClientController();
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {

                    var lsRequest = db.STRPRCD_GET_ALL_REQUEST();
                    List<RequestViewModel> lsRequestResponse = new List<RequestViewModel>();
                    foreach (var request in lsRequest) {
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
                //    var lsRequest = db.Request.Where(r => r.rqt_state == true)
                //                            .Select(r => new RequestViewModel { 
                //                                id = r.rqt_id,
                //                                client =  new ClientViewModel { 
                //                                                    id = r.Client.cli_document,
                //                                                    kindOfDocument = new KindOfDocumentViewModel { 
                //                                                                            id = r.Client.kindOfDocument.kod_id,
                //                                                                            description= r.Client.kindOfDocument.kod_description},
                //                                                    name = r.Client.cli_name,
                //                                                    lastName = r.Client.cli_lastName,
                //                                                    cellPhone = r.Client.cli_cellPhone,
                //                                                    email = r.Client.cli_email,
                //                                                    city  = new CityViewModel { 
                //                                                                id = r.Client.Cities.cty_id,
                //                                                                name = r.Client.Cities.cty_name,
                //                                                                departmentId = r.Client.Cities.dpt_id},
                //                                                    economicActivity = new EconomicActivityViewModel { 
                //                                                                id = r.Client.EconomicActivity.ea_id,
                //                                                                description = r.Client.EconomicActivity.ea_description
                //                                                                },
                //                                                    canal = new CanalViewModel { 
                //                                                                id = r.Client.Canal.cnl_id,
                //                                                                description = r.Client.Canal.cnl_description
                //                                                                }
                //                                },
                //                                probability = new ProbabilityViewModel { id = r.probability.prb_id,description=r.probability.prb_description},
                //                                parentState = new StateViewModel { id = r.states.sta_id,description = r.states.sta_description},
                //                                childState = new StateViewModel { id = r.states1.sta_id,description= r.states1.sta_description},
                //                                user = new UserViewModel {  id= r.users.usu_document,name = r.users.usu_name, lastName = r.users.usu_lastName},
                //                                contact = new ContactViewModel {id = r.Contact.cnt_id, name = r.Contact.cnt_name, lastName = r.Contact.cnt_lastName},
                //                                initialDate = r.rqt_firstVisitDate,
                //                                lastDate = r.rqt_lastVisitDate,
                //                                registrationDate = r.rqt_registrationDate
                //                            }).OrderByDescending(r => r.registrationDate)
                //                            .ToList();

                //    foreach (var rqt in lsRequest) {
                //        var riskInformation = db.riskInformationByRequest.Where(ri => ri.rqt_id == rqt.id && ri.ribr_state == true).FirstOrDefault();

                //        if (riskInformation.states != null) {
                //            var oState = new StateViewModel();
                //            oState.id = riskInformation.states.sta_id;
                //            oState.description = riskInformation.states.sta_description;

                //            rqt.riskInformation = new RiskInformationViewModel()
                //            {
                //                id = riskInformation.ribr_id,
                //                riskState = oState,
                //                ammountApproved = long.Parse(riskInformation.ribr_ammountApproved.ToString()),
                //                datefiling = riskInformation.ribr_dateFiling
                //            };
                //        }                     

                        

                //        var operationalInformation = db.operationalInformationByRequest.Where(oi => oi.rqt_id == rqt.id && oi.oibr_state == true).FirstOrDefault();


                //        rqt.operationalInformation = new OperationalInformationViewModel()
                //        {
                //            id = operationalInformation.oibr_id,
                //            deliveredAmmount = Decimal.Parse(operationalInformation.oibr_deliveredAmmount.ToString()),
                //            deliveredVehicles = int.Parse(operationalInformation.oibr_deliveredVehicles.ToString()),
                //            deliveredDate = operationalInformation.oibr_deliveredDate,
                //            legalizationDate = operationalInformation.oibr_legalizationDate
                //        };




                //        try
                //        {

                //            var branch = db.branch.Where(br => br.cli_document == rqt.client.id).FirstOrDefault();
                //            var lsContactsByClient = db.Contact.Where(cnt => cnt.bra_id == branch.bra_id)
                //                                          .Select(cnt => new ContactViewModel
                //                                          {
                //                                              id = cnt.cnt_id,
                //                                              name = cnt.cnt_name,
                //                                              lastName = cnt.cnt_lastName,
                //                                              phone = cnt.cnt_phone,
                //                                              cellPhone = cnt.cnt_cellPhone,
                //                                              email = cnt.cnt_email,
                //                                              jobTitle = new JobTitleViewModel { id = cnt.JobTitlesClient.jtcl_id, description = cnt.JobTitlesClient.jtcl_description },
                //                                              adress = cnt.cnt_adress,
                //                                              branch = new BranchViewModel { id = cnt.branch.bra_id, name = cnt.branch.bra_name }
                //                                          }).ToList();


                //            rqt.client.lsContacts = lsContactsByClient;
                //        }
                //        catch (Exception ex) {
                //            Console.WriteLine("El cliente no tiene contactos");
                //            continue;
                            
                //        }
                       
                //    }

                //    return Ok(lsRequest);
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }


        [HttpPost]
        public IHttpActionResult addRequest(RequestViewModel pRequest) {
            try
            {
                ResponseViewModel response = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities()) {
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
                    oRequest.cnt_id = pRequest.contact.id;

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
        public IHttpActionResult deleteRequest(RequestViewModel pRq) {
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {
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
        public IHttpActionResult getProbabilities() {
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {
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
                        lsParentStates = db.states.Where( st=>st.sta_parentState == null && st.stateGroup.stGrp_id != 2 && st.stateGroup.stGrp_id != 4)
                                                  .Select(st => new StateViewModel { id = st.sta_id, description = st.sta_description })
                                                  .ToList();
                    }
                    else {
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
                    else {
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
        public IHttpActionResult updateRiskInformationPerRequest(RequestViewModel pRequest) {
            try
            {
                ResponseViewModel response = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities()) {
                    var oRiskInformation = db.riskInformationByRequest.Where(ri => ri.rqt_id == pRequest.id && ri.ribr_state == true)
                                                                    .FirstOrDefault();

                    oRiskInformation.sta_id = pRequest.riskInformation.riskState.id;

                    if(pRequest.riskInformation.datefiling != null) { 
                        oRiskInformation.ribr_dateFiling = pRequest.riskInformation.datefiling;
                    }

                    if (pRequest.riskInformation.ammountApproved != 0) {
                        oRiskInformation.ribr_ammountApproved = pRequest.riskInformation.ammountApproved;
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
        public IHttpActionResult updateOperationalInformationPerRequest(RequestViewModel pRequest) {
            try
            {
                ResponseViewModel response = new ResponseViewModel();

                using (BDRAEntities db = new BDRAEntities())
                {
                    if (pRequest.parentState != null && pRequest.childState != null) {
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
        public IHttpActionResult updateRequest(RequestViewModel pRequest) {
            try
            {
                using (BDRAEntities db = new BDRAEntities()) {

                    var oRequest = db.Request.Where(r => r.rqt_id == pRequest.id).FirstOrDefault();                    
                    oRequest.rqt_firstVisitDate = pRequest.initialDate;
                    oRequest.rqt_lastVisitDate = pRequest.lastDate;
                    oRequest.prb_id = pRequest.probability.id;
                    oRequest.rqt_primaryState = pRequest.parentState.id;
                    oRequest.rqt_secondState = pRequest.childState.id;                    
                    oRequest.cnt_id = pRequest.contact.id;
                    
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

        [HttpPost]
        public IHttpActionResult generateArchive()
        {
            var path = ConfigurationManager.AppSettings["FilePath"];
            var fName = ConfigurationManager.AppSettings["FileName"];
            var fileName = "";

            try
            {
                ResponseViewModel response = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities())
                {
                    var archiveContent = db.RA_SP_GetDataToFile();
                    var date = DateTime.Now.ToString().Substring(0, 10);
                    date = date.Replace("-", "");
                    date = date.Replace("/", "");

                    //var fileName = "\\\\Rabgti02dssn\\prueba\\prueba_" + date + ".csv";
                 
                    fileName = path + fName + "_" + date + ".csv";

                    FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.Read, fileName);
                    fp.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, fileName);

                    fp.Demand();

                    using (var archive = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8))
                    {

                        var enc = string.Format("NIT;Consecutivo;Canal;Cliente;Gerente Renting;VP;Banca;Segmento;Regional;CNE/Zona;CC Banco;Gerente a cargo banco;Departamento Empresa;Ciudad Empresa;Dane empresa;Contacto Empresa;Cargo;Telefono;Celular;Direccion;Correo;Actividad económica;Código Act Económica;Fecha Visita;Fecha Última Visita;Estado Principal;Estado Secundario;Tercer Estado;Probabilidad;Decision Riesgo;Fecha de radicacion Riesgo;Monto Aprobado;# Vehiculos Entregados;Monto Activos Entregados;Fecha de legalización;Fecha de entrega;Usuario Creación Registro;Fecha creación;Usuario Actualización Registro;Fecha Actualización;Usuario Actualización RiesgoOP;Fecha Actualización Riesgo OP");

                        archive.WriteLine(enc);
                        archive.Flush();

                        foreach (var content in archiveContent)
                        {

                            var line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32};{33};{34};{35};{36};{37};{38};{39};{40};{41}",
                                content.NIT,
                                content.Consecutivo,
                                content.Canal,
                                content.Cliente,
                                content.Gerente_Renting,
                                content.VP,
                                content.Banca,
                                content.Segmento,
                                content.Regional,
                                content.CNE_Zona,
                                content.CC_Banco,
                                content.Gerente_a_cargo_banco,
                                content.Departamento_Empresa,
                                content.Ciudad_Empresa,
                                content.Dane_empresa,
                                content.Contacto_Empresa,
                                content.Cargo,
                                content.Telefono,
                                content.Celular,
                                content.Direccion,
                                content.Correo,
                                content.Actividad_económica,
                                content.Código_Act_Económica,
                                content.Fecha_Visita,
                                content.Fecha_Última_Visita,
                                content.Estado_Principal,
                                content.Estado_Secundario,
                                content.Tercer_Estado,
                                content.Probabilidad,
                                content.Decision_Riesgo,
                                content.Fecha_de_radicacion_Riesgo,
                                content.Monto_Aprobado,
                                content.C__Vehiculos_Entregados,
                                content.Monto_Activos_Entregados,
                                content.Fecha_de_legalización,
                                content.Fecha_de_entrega,
                                content.Usuario_Creación_Registro,
                                content.Fecha_creación,
                                content.Usuario_Actualización_Registro,
                                content.Fecha_Actualización,
                                content.Usuario_Actualización_RiesgoOP,
                                content.Fecha_Actualización_Riesgo_OP

                                );
                            archive.WriteLine(line);
                            archive.Flush();
                        }
                    }

                }
                //prueba

                response.response = true;
                response.message = "Se ha creado el archivo correctamente;"+ path;

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
                throw;
            }
        }

    }
}
