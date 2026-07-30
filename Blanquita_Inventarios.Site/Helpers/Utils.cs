using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Blanquita_Inventarios.Site.Helpers
{
    public static class Utils
    {
        public static string CreateCode(int length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}