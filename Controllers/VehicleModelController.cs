using API_RA_Forms.ViewModels;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_RA_Forms.Controllers
{
		public class VehicleModelController : ApiController
		{
				public IHttpActionResult Get()
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsVehicleModel = db.VehicleModel
																						.Select(
																										vm => new VehicleModelViewModel
																										{
																												id = vm.vehMdl_id,
																												name = vm.vehMdl_name,
																												description = vm.vehMdl_description
																										}
																										).ToList();

										return Ok(lsVehicleModel);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}

				[HttpGet]
				public IHttpActionResult GetVehicleModelByDescription(string pDescription)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsVehicleModel = db.VehicleModel.Where(vm => vm.vehMdl_name.ToUpper().Contains(pDescription.ToUpper()))
																												.Select(vm => new VehicleModelViewModel
																												{
																														id = vm.vehMdl_id,
																														name = vm.vehMdl_name,
																														description = vm.vehMdl_description
																												})
																												.Take(10)
																												.ToList();
										return Ok(lsVehicleModel);
								}
						}
						catch (Exception ex)
						{
								return BadRequest(ex.Message);
						}
				}


				public static VehicleModel SetDataToVehicleModel(VehicleModelViewModel pVehicleModel)
				{
						VehicleModel oVehicleModel = new VehicleModel();
						int idVehicleModel = int.Parse(pVehicleModel.id.ToString());
						oVehicleModel.vehMdl_id = idVehicleModel;
						oVehicleModel.vehMdl_name = pVehicleModel.name;
						return oVehicleModel;
				}

				public static  VehicleModelViewModel CreateVehicleModelInDB(VehicleModel pVehicleModel)
				{
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										db.VehicleModel.Add(pVehicleModel);
										db.SaveChanges();

										var vehiclemodelDB = db.VehicleModel.Where(vm => vm.vehMdl_name == pVehicleModel.vehMdl_name)
																												.Select( vm => new VehicleModelViewModel { id = vm.vehMdl_id, name = vm.vehMdl_name, description = vm.vehMdl_description })
																												.FirstOrDefault();
										return vehiclemodelDB;
								}
						}
						catch (Exception ex)
						{
								return null;
						}
				}
		}
}
