using Blanquita_Inventarios.BussinesLayer;
using Blanquita_Inventarios.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Blanquita_Inventarios.WebApi.Controllers
{
    public class UsuariosController : ApiController
    {
        UsuariosBL usuariosBL = new UsuariosBL();

        [HttpPost]
        [ResponseType(typeof(DBResponse<UsuarioSesion>))]
        [Route("api/Login")]
        public async Task<IHttpActionResult> Login(UserLogin login)
        {
            DBResponse<UsuarioSesion> response = usuariosBL.LoginApp(login);
            return Ok(response);
        }
    }
}