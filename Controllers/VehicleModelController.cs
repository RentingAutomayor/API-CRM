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
        public IHttpActionResult Get() {
						try
						{
								using (BDRAEntities db = new BDRAEntities())
								{
										var lsVehicleModel = db.VehicleModel
																						.Select(
																										vm => new VehicleModelViewModel { 
																												id = vm.vehMdl_id,
																												name = vm.vehMdl_name,
																												description  = vm.vehMdl_description
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
    }
}
