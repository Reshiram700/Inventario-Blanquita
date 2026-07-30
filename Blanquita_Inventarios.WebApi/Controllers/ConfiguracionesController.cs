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
    public class ConfiguracionesController : ApiController
    {
        ConfiguracionesBL configuracionesBL = new ConfiguracionesBL();

        [HttpGet]
        [ResponseType(typeof(DBResponse<Configuraciones>))]
        [Route("api/GetConfiguracion/{idConfiguracion}")]
        public async Task<IHttpActionResult> Get_ConfiguracionById(int idConfiguracion)
        {
            DBResponse<Configuraciones> response = configuracionesBL.Search_ConfiguracionByID(idConfiguracion);
            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(DBResponse<Marbetes>))]
        [Route("api/UpdateMarbeteDescargado")]
        public async Task<IHttpActionResult> Update_MarbeteDescargado(MarbeteDescargado marbete)
        {
            DBResponse<Marbetes> response = configuracionesBL.Update_MarbeteDescargado(marbete);
            return Ok(response);
        }
    }
}