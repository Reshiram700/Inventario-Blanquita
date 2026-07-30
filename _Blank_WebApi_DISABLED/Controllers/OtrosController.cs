using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;
using System.Web.Http;
using Blanquita_Inventarios.BussinesLayer;


namespace Blanquita_Inventarios.WebApi.Controllers
{
    public class OtrosController : ApiController
    {
        OtrosBL otrosBL = new OtrosBL();

        [HttpGet]
        [ResponseType(typeof(DBResponse<List<ControlDDL>>))]
        [Route("api/GetInventarios")]
        public async Task<IHttpActionResult> Get_Inventarios()
        {
            DBResponse<List<ControlDDL>> response = otrosBL.Get_ListadoInventarios("- Seleccione -");
            return Ok(response);
        }
    }
}