using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    [Table("Zonas")]
    public class SI_Zonas_Sqlite
    {
        [PrimaryKey]        
        [Column("IdZona")]
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public int MarbeteInicial { get; set; }
        public int MarbeteFinal { get; set; }
    }
}
