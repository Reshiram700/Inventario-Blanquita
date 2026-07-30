using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    [Table("Barcodes")]
    public class SAP_Barcodes_Sqlite
    {
        public string ItemCode { get; set; }
        public string BcdCode { get; set; }
        public string UomCode { get; set; }
        public decimal BaseQty { get; set; }
    }
}
