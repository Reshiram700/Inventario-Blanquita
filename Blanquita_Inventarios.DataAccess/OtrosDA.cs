using Blanquita_Inventarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.DataAccess
{
    public class OtrosDA
    {
        public DBResponse<List<ControlDDL>> Get_ListadoPerfiles(string textoInicial)
        {
            DBResponse<List<ControlDDL>> response = new DBResponse<List<ControlDDL>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {                    
                    var query = (from i in db.Perfiles                                 
                                 select new ControlDDL
                                 {
                                     Valor = i.IdPerfil,
                                     Texto = i.Perfil
                                 }).Union(from p in db.Usuarios
                                          select new ControlDDL
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<List<ControlDDL>> Get_ListadoInventarios(string textoInicial)
        {
            DBResponse<List<ControlDDL>> response = new DBResponse<List<ControlDDL>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from i in db.Configuraciones
                                 where i.Activo == true
                                    && i.Cerrado == false
                                    && i.Deshabilitado == false
                                 select new ControlDDL
                                 {
                                     Valor = i.IdConfiguracion,
                                     Texto = i.NombreInventario
                                 }).Union(from p in db.Usuarios
                                          select new ControlDDL
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<List<ControlDDL>> Get_ListadoZonasByConfiguracion(int idConfiguracion, string textoInicial)
        {
            DBResponse<List<ControlDDL>> response = new DBResponse<List<ControlDDL>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = (from i in db.Zonas
                                 where i.IdConfiguracion == idConfiguracion
                                    && i.Activo == true
                                 select new ControlDDL
                                 {
                                     Valor = i.IdZona,
                                     Texto = i.Zona
                                 }).Union(from p in db.Usuarios
                                          select new ControlDDL
                                          {
                                              Valor = 0,
                                              Texto = textoInicial
                                          })
                                 .Distinct()
                                 .OrderBy("Valor")
                                 .ToList();

                    response.Data = query;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }

        public DBResponse<List<ControlDDL2>> Get_ListadoCategorias(string textoInicial)
        {
            DBResponse<List<ControlDDL2>> response = new DBResponse<List<ControlDDL2>>();

            try
            {
                using (InventariosEntities db = new InventariosEntities())
                {
                    var query = db.Report_ListadoCategorias(textoInicial).ToList();

                    List<ControlDDL2> result = query.Select(s => new ControlDDL2
                    {
                        Valor = s.Valor,
                        Texto = s.Texto
                    }).ToList();

                    response.Data = result;
                    response.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                if (ex.InnerException != null)
                    response.Message += ex.InnerException.Message;
            }

            return response;
        }
    }
}
