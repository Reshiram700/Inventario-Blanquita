using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.TestSAPHana
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Bienvenido");
            string msjError = string.Empty;

            HanaConnection conn = new HanaConnection("server=162.248.55.86:30015;userid=TEST_USER;password=CBlan@MIN*2025*;");
            
            try
            {
                conn.Open();              
                


                conn.Close();
            }
            catch (HanaException ex)
            {
                msjError = ex.Errors[0].Source + " : " +
                     ex.Errors[0].Message + " (" +
                     ex.Errors[0].NativeError.ToString() + ")";
            }
            finally
            {
                if(conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            if(!String.IsNullOrEmpty(msjError))
                Console.WriteLine(msjError);
        }
    }
}
