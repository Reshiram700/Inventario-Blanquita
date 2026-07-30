using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class ConexDIAPI
    {
        public string Server {  get; set; }
        public string Company { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public bool UseTrusted {  get; set; }
    }
}
