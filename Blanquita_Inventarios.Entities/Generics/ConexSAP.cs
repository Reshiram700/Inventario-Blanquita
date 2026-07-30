using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities.Generics
{
    public class ConexSAP
    {
        public string Server {  get; set; }
        public string Puerto { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Schema { get; set; }
    }
}
