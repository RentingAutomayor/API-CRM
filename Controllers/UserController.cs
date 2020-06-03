using API_RA_Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;
using System.Web.Http.Cors;

namespace API_RA_Forms.Controllers
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private BDRAEntities db = new BDRAEntities();
        [HttpPost]
        public IHttpActionResult AddUser([FromBody] UserViewModel usu) {
            try
            {
                users user = new users();
                user.kod_id = usu.kindOfDocument;
                user.usu_document = usu.id;
                user.usu_name = usu.name;
                user.usu_lastName = usu.lastName;
                user.usu_cellphone = usu.cellPhone;
                user.usu_email = usu.email;
                db.users.Add(user);
                db.SaveChanges();
                return Ok(user);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
           
        }


        [HttpGet]
        public List<UserViewModel> Get() {
            var lsUser = db.users
                .Select(u => new UserViewModel { id = u.usu_document, kindOfDocument = u.kod_id, name = u.usu_name, lastName = u.usu_lastName, cellPhone= u.usu_cellphone,email=u.usu_email })
                .ToList();
            return lsUser;
        }


        [HttpPost]
        public IHttpActionResult authUser(LoginViewModel loginUserRA)
        {
            try
            {
                ResponseViewModel resp = new ResponseViewModel();
                using (BDRAEntities db = new BDRAEntities()) {
                    var userAuth = db.logins.Where(lg => lg.log_userName == loginUserRA.userName && lg.log_password == loginUserRA.password)
                                            .FirstOrDefault();
                    
                    if (userAuth != null)
                    {
                        var user = db.users.Where(u => u.usu_document == userAuth.usu_document)
                                            .Select(u => new UserViewModel { 
                                                id = u.usu_document,
                                                kindOfDocument = u.kod_id,
                                                name = u.usu_name,
                                                lastName = u.usu_lastName,
                                                cellPhone = u.usu_cellphone,
                                                email = u.usu_email })        
                                            .FirstOrDefault();

                        var rolByUser = db.userByRol.Where(rl => rl.usu_document == user.id)
                                                    .Select(rl => new RolViewModel { id = rl.roles.rol_id, name = rl.roles.rol_name, description = rl.roles.rol_description })
                                                    .FirstOrDefault();

                        user.rol = rolByUser;

                        var permByRol = db.permissionByRole.Where(pm => pm.rol_id == rolByUser.id)
                                                           .Select(pm => new PermissionByModuleViewModel
                                                           {
                                                               id= pm.permByRol_id,
                                                               module = new ModuleViewModel { id = pm.permissionByModule.modules.mdl_id, name = pm.permissionByModule.modules.mdl_name, description = pm.permissionByModule.modules.mdl_description},
                                                               permission = new PermissionViewModel { id = pm.permissionByModule.permission.perm_id, name = pm.permissionByModule.permission.perm_name }
                                                           }).ToList();

                        user.rol.permissionByModule = permByRol;

                        

                        var permissionDetail = "";
                        foreach (var perm in permByRol) {
                            permissionDetail += perm.permission.name + " " + perm.module.name;
                        }

                        

                        resp.response = true;
                        resp.message = "Bienvenido " + user.name + " " + user.lastName;
                        resp.user = user;
                    }
                    else {
                        resp.response = false;
                        resp.message = "Usuario o contraseña inválidos";
                    }
                }       
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}
