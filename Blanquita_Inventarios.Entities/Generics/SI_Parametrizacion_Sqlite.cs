using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    [Table("Parametrizacion")]
    public class SI_Parametrizacion_Sqlite
    {
        [PrimaryKey]
        [Column("IdParametrizacion")]
        public int IdParametrizacion { get; set; }
        public string NombrePDA { get; set; }
        public int IdConfiguracion { get; set; }
        public string Almacen {  get; set; }
        public int Capturados { get; set; }
        public int Descargados { get; set; }
        public string NombreCatalogo { get; set; }
    }
}
