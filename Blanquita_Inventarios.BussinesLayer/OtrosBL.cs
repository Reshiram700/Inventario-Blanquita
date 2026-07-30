using Blanquita_Inventarios.DataAccess;
using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.BussinesLayer
{
    public class OtrosBL
    {
        OtrosDA conex = new OtrosDA();

        public DBResponse<List<ControlDDL>> Get_ListadoPerfiles(string textoInicial)
        {
            return conex.Get_ListadoPerfiles(textoInicial);
        }

        public DBResponse<List<ControlDDL>> Get_ListadoInventarios(string textoInicial)
        {
            return conex.Get_ListadoInventarios(textoInicial);
        }

        public DBResponse<List<ControlDDL>> Get_ListadoZonasByConfiguracion(int idConfiguracion, string textoInicial)
        {
            return conex.Get_ListadoZonasByConfiguracion(idConfiguracion, textoInicial);
        }

        public DBResponse<List<ControlDDL>> Get_ListadoConteos()
        {
            DBResponse<List<ControlDDL>> response = new DBResponse<List<ControlDDL>>();

            List<ControlDDL> listado = new List<ControlDDL> {
                new ControlDDL { Valor = 0, Texto = "- Seleccione -"},
                new ControlDDL { Valor = 1, Texto = "Conteo 1"},
                new ControlDDL { Valor = 2, Texto = "Conteo 2"},
            };

            response.Data = listado;

            return response;
        }

        public DBResponse<List<ControlDDL2>> Get_ListadoCategorias(string textoInicial)
        {
            return conex.Get_ListadoCategorias(textoInicial);
        }
    }
}
