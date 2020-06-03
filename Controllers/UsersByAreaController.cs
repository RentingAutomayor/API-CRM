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
    public class UsersByAreaController : ApiController
    {
        [HttpGet]
        public List<UserViewModel> Get(string area_name) {
            try
            {
                using (BDRAEntities db = new BDRAEntities())
                {
                    var area = db.areas.Where(a => a.area_description.ToUpper() == area_name.ToUpper()).FirstOrDefault();
                    var jobTitles = db.jobTitle.Where(jt => jt.area_id == area.area_id).ToList();

                    List<UserViewModel> lsUserByArea = new List<UserViewModel>();
                    foreach (var jobTitle in jobTitles)
                    {
                        var lsUsers = db.users.Where(u => u.jt_id == jobTitle.jt_id)
                                        .Select(u => new UserViewModel { id = u.usu_document, name = u.usu_name, lastName = u.usu_lastName})
                                        .ToList();

                        lsUserByArea.AddRange(lsUsers);
                    }
                    return lsUserByArea.OrderBy( u => u.name).ToList();
                }

                                   
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }
        
    }
}
