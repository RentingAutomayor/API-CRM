using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAO;
using API_RA_Forms.ViewModels;
using System.Web.Http.Cors;

namespace API_RA_Forms.Controllers
{
    [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]    
    public class KindOfDocumentController : ApiController
    {
        [HttpGet]
        public List<KindOfDocumentViewModel> Get() {
            using (BDRAEntities db = new BDRAEntities()) {
                var lsKindOfDocuments = db.kindOfDocument
                                                    .Select(k => new KindOfDocumentViewModel { id = k.kod_id, description = k.kod_description })
                                                    .ToList();
                return lsKindOfDocuments;
            }
        }
    }
}
