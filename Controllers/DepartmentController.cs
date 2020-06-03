using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using API_RA_Forms.ViewModels;
using DAO;

namespace API_RA_Forms.Controllers
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
    public class DepartmentController : ApiController
    {
        private BDRAEntities db = new BDRAEntities();

        [System.Web.Http.HttpGet]
        public List<DepartmentViewModel> Get() {
            using (BDRAEntities dbra = new BDRAEntities()) {
                var lsDepartments = dbra.Departments
                                        .Select(dpt => new DepartmentViewModel { id = dpt.dpt_id, name = dpt.dpt_name,countyId = dpt.ctry_id })
                                        .ToList();
                return lsDepartments;
            }
        }
    }
}
