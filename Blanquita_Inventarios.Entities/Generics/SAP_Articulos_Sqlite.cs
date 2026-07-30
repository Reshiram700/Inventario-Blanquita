using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Blanquita_Inventarios.Entities
{
    [Table("Articulos")]
    public class SAP_Articulos_Sqlite
    {
        public string WshCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
    }
}
