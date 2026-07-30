using Blanquita_Inventarios.DataAccess;
using Blanquita_Inventarios.Entities;
using Blanquita_Inventarios.Entities.Generics;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.BussinesLayer
{
    public class ConfiguracionesBL
    {
        ConfiguracionesDA conex = new ConfiguracionesDA();
        public DBResponse<List<Configuraciones>> Get_Configuraciones(DateTime fechaInicial, DateTime fechaFinal, string inventario, string servidor)
        {
            return conex.Get_Configuraciones(fechaInicial, fechaFinal, inventario, servidor);
        }

        public DBResponse<Configuraciones> Search_ConfiguracionByID(int idConfiguracion)
        {
            return conex.Search_ConfiguracionByID(idConfiguracion);
        }

        public DBResponse<Configuraciones> Upsert_Configuracion(Configuraciones configuracion, ConexSAP conexSAP)
        {
            return conex.Upsert_Configuracion(configuracion, conexSAP);
        }

        public DBResponse<Configuraciones> Delete_Configuracion(int idConfiguracion)
        {
            return conex.Delete_Configuracion(idConfiguracion);
        }

        public DBResponse<Configuraciones> Cerrar_Configuracion(int idConfiguracion)
        {
            return conex.Cerrar_Configuracion(idConfiguracion);
        }

        public DBResponse<Configuraciones> Deshabilitar_Configuracion(int idConfiguracion)
        {
            return conex.Deshabilitar_Configuracion(idConfiguracion);
        }

        public DBResponse<int> Generate_BackupInventario(int idConfiguracion, string directorioBackups)
        {
            return conex.Generate_BackupInventario(idConfiguracion, directorioBackups);
        }

        public DBResponse<List<Zonas>> Get_Zonas(int idConfiguracion, DateTime fechaInicial, DateTime fechaFinal, string zona)
        {
            return conex.Get_Zonas(idConfiguracion, fechaInicial, fechaFinal, zona);
        }

        public DBResponse<Zonas> Search_ZonaByID(int idZona)
        {
            return conex.Search_ZonaByID(idZona);
        }

        public DBResponse<Zonas> Insert_Zona(Zonas zona)
        {
            if (zona.MarbeteInicial < zona.MarbeteFinal)
            {
                return conex.Insert_Zona(zona);
            }
            else
            {
                return new DBResponse<Zonas> { 
                    ExecutionOK = false,
                    Message = "El Marbete Final no puede ser menor o igual al Marbete Inicial",
                };
            }
        }

        public DBResponse<Zonas> Delete_Zona(int idZona)
        {
            return conex.Delete_Zona(idZona);
        }

        public DBResponse<List<Codigos>> Get_CodigosByConfiguracion(int idConfiguracion)
        {
            return conex.Get_CodigosByConfiguracion(idConfiguracion);
        }

        public DBResponse<Codigos> Insert_Codigo(Codigos codigo)
        {
            return conex.Insert_Codigo(codigo);
        }

        public DBResponse<Zonas> Search_ZonaMarbeteInicial(int idConfiguracion)
        {
            return conex.Search_ZonaMarbeteInicial(idConfiguracion);
        }

        public DBResponse<Zonas> Create_ZonaMarbetes(int idZona)
        {
            return conex.Create_ZonaMarbetes(idZona);
        }

        public DBResponse<Zonas> Delete_ZonaMarbetes(int idZona)
        {
            return conex.Delete_ZonaMarbetes(idZona);
        }

        public DBResponse<int> Update_UsoCodigo(UseCodigo uso)
        {
            return conex.Update_UsoCodigo(uso);
        }

        public DBResponse<Marbetes> Update_MarbeteDescargado(MarbeteDescargado marbete)
        {
            return conex.Update_MarbeteDescargado(marbete);
        }

        public DBResponse<List<Listado_AvanceZona>> Get_MonitorAvanceZona(int idConfiguracion)
        {
            return conex.Get_MonitorAvanceZona(idConfiguracion);
        }

        public DBResponse<List<Listado_AvanceMarbete>> Get_MonitorAvanceMarbete(int idConfiguracion, int idZona, int marbete)
        {
            return conex.Get_MonitorAvanceMarbete(idConfiguracion, idZona, marbete);
        }

        public DBResponse<List<Listado_AvanceDetalleMarbete>> Get_MonitorAvanceDetalleMarbete(int idConfiguracion, int marbete)
        {
            return conex.Get_MonitorAvanceDetalleMarbete(idConfiguracion, marbete);
        }

        public DBResponse<int> Delete_ArticuloMarbete(int idConfiguracion, int idDetalle)
        {
            return conex.Delete_ArticuloMarbete(idConfiguracion, idDetalle);
        }

        public DBResponse<DatosBarcode> Get_DatosProducto(int idConfiguracion, string codigo)
        {
            return conex.Get_DatosProducto(idConfiguracion, codigo);
        }

        public DBResponse<int> Insert_Producto(int idConfiguracion, int marbete, string codigo, decimal cantidad, string usuario)
        {
            return conex.Insert_Producto(idConfiguracion, marbete, codigo, cantidad, usuario);
        }

        public DBResponse<int> Update_CantidadProducto(int idConfiguracion, int idDetalle, decimal cantidad)
        {
            return conex.Update_CantidadProducto(idConfiguracion, idDetalle, cantidad);
        }

        public DBResponse<List<DatosBarcode>> Get_MarbeteProductos(int idConfiguracion, int marbete)
        {
            return conex.Get_MarbeteProductos(idConfiguracion, marbete);
        }

        public DBResponse<List<Resultado_Capturas>> Get_Procesado(int idConfiguracion, int marbete, int idConteo, string codigoArticulo)
        {
            return conex.Get_Procesado(idConfiguracion, marbete, idConteo, codigoArticulo);
        }

        public DBResponse<List<Report_CostoCeroByInventario_Result>> Get_CostoCeroInactivos(int idConfiguracion)
        {
            return conex.Get_CostoCeroInactivos(idConfiguracion);
        }

        public DBResponse<List<Listado_DiferenciasMontosUno>> Get_MontosUno(int idConfiguracion)
        {
            return conex.Get_MontosUno(idConfiguracion);
        }

        public DBResponse<List<Listado_DiferenciasConteoUno>> Get_ConteoUno(int idConfiguracion, string busca, string itmsGrpNam)
        {
            return conex.Get_ConteoUno(idConfiguracion, busca, itmsGrpNam);
        }

        public DBResponse<List<Listado_DiferenciasAjustes>> Get_AjustesConteoDos(int idConfiguracion, string busca, string itmsGrpNam)
        {
            return conex.Get_AjustesConteoDos(idConfiguracion, busca, itmsGrpNam);
        }

        public DBResponse<int> Aply_Ajustes(int idConfiguracion, ConexDIAPI conDiApi, string directorioLogs)
        {
            return conex.Aply_Ajustes(idConfiguracion, conDiApi, directorioLogs);
        }

        public DBResponse<List<Listado_ArticulosPDA>> Get_PDAArticulos(int idConfiguracion)
        {
            return conex.Get_PDAArticulos(idConfiguracion);
        }

        public DBResponse<int> Update_AjustesConteoDos(int idConfiguracion, List<Listado_AjustesConteoDos> listadoAjustes)
        {
            return conex.Update_AjustesConteoDos(idConfiguracion, listadoAjustes);
        }

        public DBResponse<List<Diferencias_Reporte>> Get_DiferenciasReporte(decimal idConfiguracion, decimal montoVarianza, string categoria)
        {
            return conex.Get_DiferenciasReporte(idConfiguracion, montoVarianza, categoria);
        }

        public DBResponse<List<Listado_DiferenciasMontosUno>> Get_MontosDos(int idConfiguracion)
        {
            return conex.Get_MontosDos(idConfiguracion);
        }

        public DBResponse<Zonas> Search_ZonaByIdConfiguracionAndIdZona(int idConfiguracion, int idZona)
        {
            return conex.Search_ZonaByIdConfiguracionAndIdZona(idConfiguracion, idZona);
        }

        public DBResponse<List<Diferencias_Reporte>> Get_DiferenciasReporteZona(int idConfiguracion, decimal montoVarianza, int idZona)
        {
            return conex.Get_DiferenciasReporteZona(idConfiguracion, montoVarianza, idZona);
        }
    }
}
