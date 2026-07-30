using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class Listado_AjustesConteoDos
    {
        public int IdDetalle {  get; set; }
        public decimal Cantidad { get; set; }
        public decimal Contado1 { get; set; }
        public decimal Contado2 { get; set; }
        public decimal Contado3 { get; set; }
        public decimal Contado4 { get; set; }
        public int Modificado1 { get; set; }
        public int Modificado2 { get; set; }
        public int Modificado3 { get; set; }
        public int Modificado4 { get; set; }
    }
}
