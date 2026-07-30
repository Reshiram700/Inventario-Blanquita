using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    [Table("Marbetes")]
    public class SI_Marbetes_Sqlite
    {
        [PrimaryKey]
        [Column("IdMarbete")]
        public int IdMarbete { get; set; }
        public int IdZona { get; set; }
        public int Marbete { get; set; }
        public int Estatus { get; set; }
        public string Capturo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin {  get; set; }
    }
}
