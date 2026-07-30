using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_DiferenciasAjustes
    {
        public int IdConfiguracion {  get; set; }
        public int IdDetalle {  get; set; }
        public string WhsCode { get; set; }
        public string ItmsGrpNam {  get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Uom1 { get; set; }
        public decimal Onhand { get; set; }
        public decimal PorProcesar { get; set; }
        public decimal Contado { get; set; }
        public decimal Contado1 { get; set; }
        public decimal Contado2 { get; set; }
        public decimal Contado3 { get; set; }
        public decimal Contado4 { get; set; }
        public decimal DifPesosNeto { get; set; }
        public decimal Precio { get; set; }

        public decimal TotalContadoR { get; set; }
        public decimal TotalContadoA { get; set; }
        public decimal DiferenciaContado { get; set; }
        public decimal DiferenciaInventario { get; set; }
        public int Modificado1 { get; set; }
        public int Modificado2 { get; set; }
        public int Modificado3 { get; set; }
        public int Modificado4 { get; set; }
    }
}
