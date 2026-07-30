using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Blanquita_Inventarios.WebApi.Controllers
{
    public class CodigosController : ApiController
    {
        ConfiguracionesBL configuracionesBL = new ConfiguracionesBL();

        [HttpPost]
        [ResponseType(typeof(DBResponse<int>))]
        [Route("api/UsarCodigo")]
        public async Task<IHttpActionResult> Update_UsoCodigo(UseCodigo uso)
        {
            DBResponse<int> response = configuracionesBL.Update_UsoCodigo(uso);
            return Ok(response);
        }
    }
}
