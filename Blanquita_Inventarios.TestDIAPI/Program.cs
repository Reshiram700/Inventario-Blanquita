using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace Blanquita_Inventarios.TestDIAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Company oCompany = new Company();
                oCompany.Server = "NDB@BLANQUITADB02:30013"; // Servidor HANA
                oCompany.CompanyDB = "TEST_CONSTRURAMABLANQUITA"; // Base de datos de SAP B1
                oCompany.UserName = "AddonTI"; // Usuario SAP
                oCompany.Password = "Qwerty01+"; // Contraseña SAP
                oCompany.DbUserName = "SYSTEM"; // Usuario HANA
                oCompany.DbPassword = "MaS21swU/is1A8uphiqA"; // Contraseña HANA
                oCompany.DbServerType = BoDataServerTypes.dst_HANADB; // Tipo de base de datos
                oCompany.UseTrusted = false; // Desactiva autenticación confiable
                int connectionResult = oCompany.Connect();


                if (connectionResult != 0)
                {
                    int errorCode;
                    string errorDescription;

                    // Obtener el código y la descripción del error
                    oCompany.GetLastError(out errorCode, out errorDescription);

                    Console.WriteLine($"Error al conectar: Código {errorCode} - {errorDescription}");
                }
                else
                {
                    Console.WriteLine("Conexión exitosa a SAP Business One.");


                    //Afectaciones
                    Documents oInventoryEntry = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);
                    oInventoryEntry.DocDate = DateTime.Now;
                    oInventoryEntry.Comments = "Ajuste por conteo físico - Entrada de inventario";
                    oInventoryEntry.Lines.ItemCode = "20171031012";
                    oInventoryEntry.Lines.WarehouseCode = "1011";
                    oInventoryEntry.Lines.Quantity = 5; // Cantidad a ingresar
                    oInventoryEntry.Lines.UnitPrice = 29.49; // Precio promedio del almacén

                    int result = oInventoryEntry.Add();
                    if (result != 0)
                    {
                        string errorMsg;
                        int errorCode;
                        oCompany.GetLastError(out errorCode, out errorMsg);
                        Console.WriteLine($"Error {errorCode}: {errorMsg}");
                    }
                    else
                    {
                        Console.WriteLine("Entrada de inventario creada exitosamente.");
                    }

                    Documents oInventoryExit = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);
                    oInventoryExit.DocDate = DateTime.Now;
                    oInventoryExit.Comments = "Ajuste por conteo físico - Salida de inventario";
                    oInventoryExit.Lines.ItemCode = "20231018009";
                    oInventoryExit.Lines.WarehouseCode = "1011";
                    oInventoryExit.Lines.Quantity = 2; // Cantidad a retirar
                    oInventoryExit.Lines.UnitPrice = 100.24; // Precio promedio del almacén
                    int result2 = oInventoryExit.Add();

                    if (result2 != 0)
                    {
                        string errorMsg;
                        int errorCode;
                        oCompany.GetLastError(out errorCode, out errorMsg);
                        Console.WriteLine($"Error {errorCode}: {errorMsg}");
                    }
                    else
                    {
                        Console.WriteLine("Salida de inventario creada exitosamente.");
                    }
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                if (ex.InnerException != null)
                    msj += "......" + ex.InnerException.Message;

                Console.WriteLine(msj);
            }
        }
    }
}
