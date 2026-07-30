using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Models
{
    public class Listado_DiferenciasCapturas
    {
        public int Folio {  get; set; }
        public int Marbete { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public string Conteo { get; set; }
        public string NombreMarbete { get; set; }       
    }
}