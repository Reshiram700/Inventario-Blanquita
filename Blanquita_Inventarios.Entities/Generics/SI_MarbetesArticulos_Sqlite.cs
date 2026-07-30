using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    [Table("MarbetesArticulos")]
    public class SI_MarbetesArticulos_Sqlite
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("IdDetalle")]
        public int IdDetalle {  get; set; }
        public int IdMarbete { get; set; }
        public string Barcode {  get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Uom {  get; set; }
        public decimal BaseQty { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public decimal Cantidad { get; set; }
        public string Capturo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
