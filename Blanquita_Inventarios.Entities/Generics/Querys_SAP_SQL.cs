using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public static class Querys_SAP_SQL
    {
        public static string Get_SQL_Connection(string server, string instancia, string puerto, string baseDatos, string usuario, string password)
        {
            string strCon = "";

            if (String.IsNullOrEmpty(server) && !String.IsNullOrEmpty(instancia) && String.IsNullOrEmpty(puerto))
            {
                return "Server=" + instancia + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }

            if (!String.IsNullOrEmpty(server) && String.IsNullOrEmpty(instancia) && String.IsNullOrEmpty(puerto))
            {
                return "Server=" + server + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }

            if (!String.IsNullOrEmpty(server) && !String.IsNullOrEmpty(instancia) && String.IsNullOrEmpty(puerto))
            {
                return "Server=" + server + "\\" + instancia + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }

            if (String.IsNullOrEmpty(server) && !String.IsNullOrEmpty(instancia) && !String.IsNullOrEmpty(puerto))
            {
                return "Server=" + instancia + "," + puerto + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }

            if (!String.IsNullOrEmpty(server) && String.IsNullOrEmpty(instancia) && !String.IsNullOrEmpty(puerto))
            {
                return "Server=" + server + "," + puerto + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }

            if (!String.IsNullOrEmpty(server) && !String.IsNullOrEmpty(instancia) && !String.IsNullOrEmpty(puerto))
            {
                return "Server=" + server + "\\" + instancia + "," + puerto + "; " +
                        "database= " + baseDatos + "; User Id= " + usuario + ";" +
                        "Password= " + password + "; Connect Timeout=1200";
            }


            return strCon;
        }


        public static string Get_SAP_Connection(string SAPHostname, string SAPPuerto, string SAPUser, string SAPPwd)
        {
            return "server=" + SAPHostname + ":" + SAPPuerto + ";" +
                "userid=" + SAPUser + ";password=" + SAPPwd + ";";
        }

        public static string Get_SQL_CrearBaseDatos(string baseDatos)
        {
            return "CREATE DATABASE " + baseDatos;
        }

        public static string Get_SQL_CrearTabla_Zonas()
        {
            return "CREATE TABLE Zonas (IdZona INT NOT NULL, Zona VARCHAR(150) NOT NULL, MarbeteInicial INT NOT NULL, MarbeteFinal INT NOT NULL, Activo BIT NOT NULL);";
        }

        public static string Get_SQL_CrearTabla_Marbetes()
        {
            return "CREATE TABLE Marbetes (IdMarbete INT PRIMARY KEY IDENTITY (1, 1), IdZona INT NOT NULL, Marbete INT NOT NULL, Capturo VARCHAR(50) NULL, FechaInicio DATETIME NULL, FechaCierre DATETIME NULL, Estatus INT NOT NULL);";
        }

        public static string Get_SQL_CrearTabla_MarbetesArticulos()
        {
            return "CREATE TABLE MarbetesArticulos (IdDetalle INT PRIMARY KEY IDENTITY (1, 1), IdMarbete INT, CodigoArticulo VARCHAR(20) NOT NULL, CodigoBarras VARCHAR(50) NOT NULL, Descripcion VARCHAR(500) NOT NULL, Grupo VARCHAR(100) NOT NULL, Uom VARCHAR(20) NOT NULL, BaseQty DECIMAL (18,5) NOT NULL, Precio DECIMAL(18,2) NOT NULL, Costo DECIMAL(18,2) NOT NULL, Cantidad DECIMAL(18,2) NOT NULL, Cantidad2 DECIMAL(18,2) NOT NULL, Capturo VARCHAR(50) NOT NULL, FechaCaptura DATETIME NOT NULL);";
        }

        public static string Get_SQL_CrearTabla_Procesado()
        {
            return "CREATE TABLE Procesado (IdDetalle INT PRIMARY KEY IDENTITY (1, 1), WhsCode VARCHAR(20) NOT NULL, ItmsGrpNam VARCHAR(500) NOT NULL, ItemCode VARCHAR(500) NOT NULL, ItemName VARCHAR(1000) NOT NULL, Cant1 DECIMAL(18,2) NOT NULL, Cant2 DECIMAL(18,2) NOT NULL, Contado1 DECIMAL(18,2) NOT NULL, Contado2 DECIMAL(18,2) NOT NULL, Contado3 DECIMAL(18,2) NOT NULL, Contado4 DECIMAL(18,2) NOT NULL, TotalSAP DECIMAL (18,2) NOT NULL, TotalContado DECIMAL(18,2) NOT NULL, TotalDesviacion DECIMAL(18,2) NOT NULL, PorcDif DECIMAL(18,2) NOT NULL, Uom VARCHAR(100) NOT NULL, Onhand DECIMAL(18,2) NOT NULL, PorProcesar DECIMAL(18,2) NOT NULL, DifPesoNeto DECIMAL(18,2) NOT NULL, Marbetes VARCHAR(1000) NOT NULL, Precio DECIMAL(18,2) NOT NULL, VarianzaNeta DECIMAL(18,2) NOT NULL, VarianzaNetaMonto DECIMAL(18,2) NOT NULL, TotalContado2 DECIMAL(18,2) NOT NULL, TotalDesviacion2 DECIMAL(18,2) NOT NULL, PorcDif2 DECIMAL(18,2) NOT NULL, Ajustado INT NOT NULL, IdUom INT NOT NULL, TotalContadoR DECIMAL(18,2) NOT NULL, TotalContadoA DECIMAL(18,2) NOT NULL, DiferenciaContado DECIMAL(18,2) NOT NULL, DiferenciaInventario DECIMAL(18,2) NOT NULL, Modificado1 INT NOT NULL, Modificado2 INT NOT NULL, Modificado3 INT NOT NULL, Modificado4 INT NOT NULL);";
        }

        public static string Get_SQL_CrearTabla_SAPArticulos()
        {
            return "CREATE TABLE SAP_Articulos (WhsCode VARCHAR(4) NOT NULL, ItemCode VARCHAR(50), ItemName VARCHAR(500), Price DECIMAL(18,2) NOT NULL);";
        }

        public static string Get_SQL_CrearTabla_SAPArticulosBarCode()
        {
            return "CREATE TABLE SAP_Barcodes (ItemCode VARCHAR(50) NOT NULL, BcdCode VARCHAR(50) NOT NULL, UomCode VARCHAR(10) NOT NULL, BaseQty DECIMAL (18,5) NOT NULL);";
        }

        public static string Get_SQL_CreateSPItemsSAP()
        {
            return "CREATE PROCEDURE Insert_ItemsSAP " +
                   "     @XmlStr	varchar(max) " +
                   "AS " +
                   "BEGIN " +
                   "     SET NOCOUNT ON; " +
                   "     DECLARE @Xml	xml = @XmlStr; " +
                   "     SELECT " +
                   "         doc.col.value('(WhsCode/text())[1]', 'varchar(max)') as WhsCode, " +
                   "         doc.col.value('(ItemCode/text())[1]', 'varchar(max)') as ItemCode, " +
                   "         doc.col.value('(ItemName/text())[1]', 'varchar(max)') as ItemName, " +
                   "         doc.col.value('(Price/text())[1]', 'decimal(18,2)') as Price " +
                   "     INTO #tempItems " +
                   "     FROM @Xml.nodes('/Procesar/Item') doc(col)	" +
                   "     INSERT INTO SAP_Articulos(WhsCode, ItemCode, ItemName, Price) " +
                   "     SELECT " +
                   "         WhsCode, " +
                   "         ItemCode, " +
                   "         ItemName, " +
                   "         Price " +
                   "     FROM #tempItems " +
                   " END";
        }

        public static string Get_SQL_CreateSPBarcodesSAP()
        {
            return "CREATE PROCEDURE Insert_BarcodesSAP " +
                   "     @XmlStr varchar(max) " +
                   " AS " +
                   " BEGIN " +
                   "     SET NOCOUNT ON; " +
                   "     DECLARE @Xml    xml = @XmlStr; " +
                   "     SELECT " +
                   "         doc.col.value('(ItemCode/text())[1]', 'varchar(max)') as ItemCode, " +
		           "         doc.col.value('(BcdCode/text())[1]', 'varchar(max)') as BcdCode, " +
		           "         doc.col.value('(UomCode/text())[1]', 'varchar(max)') as UomCode, " +
		           "         doc.col.value('(BaseQty/text())[1]', 'decimal(18,5)') as BaseQty " +
                   "     INTO #tempItems " +
	               "     FROM @Xml.nodes('/Procesar/Item') doc(col) " +
                   "     INSERT INTO SAP_Barcodes(ItemCode, BcdCode, UomCode, BaseQty) " +
                   "     SELECT " +
                   "         ItemCode, " +
                   "         BcdCode, " +
                   "         UomCode, " +
                   "         BaseQty " +
                   "     FROM #tempItems " +
                   " END";
        }

        public static string Get_SQL_ExecuteInsertItemsSAP()
        {
            return "Exec Insert_ItemsSAP @XmlStr";
        }

        public static string Get_SQL_ExecuteInsertBarcodesSAP()
        {
            return "Exec Insert_BarcodesSAP @XmlStr";
        }

        public static string Insert_SQL_SAPArticulo()
        {
            string strCommand = "INSERT INTO SAP_Articulos(WhsCode, ItemCode, ItemName, Price) Values(@WhsCode, @ItemCode, @ItemName, @Price)";

            return strCommand;
        }

        public static string Insert_SQL_SAPArticulosBarCodes()
        {
            string strCommand = "INSERT INTO SAP_Barcodes(ItemCode, BcdCode, UomCode, BaseQty) Values(@ItemCode, @BcdCode, @UomCode, @BaseQty)";

            return strCommand;
        }

        public static string Insert_SQL_Zona()
        {
            string strCommand = "INSERT INTO Zonas (IdZona, Zona, MarbeteInicial, MarbeteFinal, Activo) VALUES (@IdZona, @Zona, @MarbeteInicial, @MarbeteFinal, 1)";

            return strCommand;
        }

        public static string Delete_SQL_Zona()
        {
            string strCommand = "DELETE FROM Zonas WHERE IdZona = @IdZona";

            return strCommand;
        }

        public static string Insert_MarbeteArticulos()
        {
            string strCommand = "INSERT INTO MarbetesArticulos (IdMarbete, CodigoArticulo, CodigoBarras, Descripcion, Grupo, Uom, BaseQty, Precio, Costo, Cantidad, Cantidad2, Capturo, FechaCaptura) VALUES (@IdMarbete, @CodigoArticulo, @CodigoBarras, @Descripcion, @Grupo, @Uom, @BaseQty, @Precio, @Costo, @Cantidad, @Cantidad2, @Capturo, @FechaCaptura)";

            return strCommand;
        }

        public static string Update_MarbeteCerrado()
        {
            string strCommand = "UPDATE Marbetes SET Capturo = @Capturo, FechaInicio = @Inicio, FechaCierre = @FechaCierre, Estatus = 3 WHERE IdMarbete = @IdMarbete";

            return strCommand;
        }

        public static string Get_SQL_MarbetesGeneradosByIdZona()
        {
            string strCommand = "SELECT COUNT(Marbete) as Marbetes FROM Marbetes WHERE IdZona = @IdZona";

            return strCommand;
        }

        public static string Get_SQL_MarbetesCerradosByIdZona()
        {
            string strCommand = "SELECT COUNT(Marbete) as Cerrados FROM Marbetes WHERE Estatus >= 2 AND IdZona = @IdZona";

            return strCommand;
        }

        public static string Get_SQL_MarbetesByIdZona()
        {
            string strCommand = "SELECT IdMarbete, IdZona, Marbete, Capturo, FechaInicio, FechaCierre, Estatus FROM Marbetes WHERE IdZona = @IdZona";

            return strCommand;
        }

        public static string Get_SQL_InfoMarbetesByIdZona()
        {
            string strCommand = "SELECT MA.IdDetalle FROM MarbetesArticulos MA LEFT JOIN Marbetes M ON MA.IdMarbete = M.IdMarbete LEFT JOIN Zonas Z ON M.IdZona = Z.IdZona WHERE Z.IdZona = @IdZona";

            return strCommand;
        }

        public static string Delete_SQL_Marbete()
        {
            string strCommand = "DELETE FROM Marbetes WHERE IdMarbete = @IdMarbete";

            return strCommand;
        }

        public static string Get_SQL_Marbetes()
        {
            string strCommand = "SELECT IdMarbete, IdZona, Marbete, Capturo, FechaInicio, FechaCierre, Estatus FROM Marbetes";

            return strCommand;
        }

        public static string Get_SQL_MarbetesCerrados()
        {
            string strCommand = "SELECT COUNT(IdMarbete) FROM Marbetes WHERE Estatus = 3";

            return strCommand;
        }

        public static string Get_SQL_MarbetesByIdZonaMarbete()
        {
            string strCommand = "SELECT IdMarbete, IdZona, Marbete, Capturo, FechaInicio, FechaCierre, Estatus FROM Marbetes WHERE IdZona = @IdZona AND (@Marbete = 0 OR (@Marbete > 0 AND Marbete = @Marbete))";

            return strCommand;
        }

        public static string Get_SQL_MarbeteUltimaCaptura()
        {
            string strCommand = "SELECT MAX(FechaCaptura) Ultima FROM MarbetesArticulos WHERE FechaCaptura IS NOT NULL AND IdMarbete = @IdMarbete";

            return strCommand;
        }

        public static string Get_SQL_CostoArticulo()
        {
            string strCommand = "SELECT ItemCode, BcdCode, UomCode, BaseQty FROM SAP_Barcodes WHERE BcdCode = @BcdCode";

            return strCommand;
        }

        public static string Get_SQL_MarbeteArticulos()
        {
            string strCommand = "SELECT IdDetalle, IdMarbete, CodigoArticulo, CodigoBarras, Descripcion, Uom, BaseQty, Precio, Costo, Cantidad, Capturo, FechaCaptura FROM MarbetesArticulos WHERE IdMarbete = @IdMarbete";

            return strCommand;
        }

        public static string Get_SQL_IdMarbeteByMarbete()
        {
            string strCommand = "SELECT IdMarbete FROM Marbetes WHERE Marbete = @Marbete";

            return strCommand;
        }

        public static string Get_SQL_MarbeteById()
        {
            string strCommand = "SELECT IdMarbete, Marbete, Estatus FROM Marbetes WHERE IdMarbete = @IdMarbete";

            return strCommand;
        }

        public static string Get_SQL_MarbeteByNoMarbete()
        {
            string strCommand = "SELECT IdMarbete, Marbete, Estatus FROM Marbetes WHERE Marbete = @Marbete";

            return strCommand;
        }

        public static string Delete_SQL_MarbeteDetalle()
        {
            string strCommand = "DELETE FROM MarbetesArticulos WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Get_InfoBarcode()
        {
            string strCommand = "SELECT ItemCode, BcdCode, UomCode, BaseQty FROM SAP_Barcodes WHERE BcdCode = @BcdCode";

            return strCommand;
        }

        public static string Get_InfoProducto()
        {
            string strCommand = "SELECT WhsCode, ItemCode, ItemName, Price FROM SAP_Articulos WHERE ItemCode = @ItemCode";

            return strCommand;
        }

        public static string Get_InfoProductoMarbete()
        {
            string strCommand = "SELECT Precio FROM MarbetesArticulos WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Update_CantidadProducto()
        {
            string strCommand = "UPDATE MarbetesArticulos SET Cantidad = @Cantidad, Cantidad2 = @Cantidad, Costo = @Costo WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Get_SQL_Procesado()
        {
            string strCommand = "SELECT IdDetalle, WhsCode, ItmsGrpNam, ItemCode, ItemName, Cant1, Cant2, TotalSAP, TotalContado, TotalDesviacion, PorcDif, Uom, Onhand, PorProcesar, DifPesoNeto, Marbetes, Precio, VarianzaNeta, VarianzaNetaMonto, TotalContado2, TotalDesviacion2, PorcDif2, Ajustado, IdUom, Contado1, Contado2, Contado3, Contado4, TotalContadoR, TotalContadoA, DiferenciaContado, DiferenciaInventario, Modificado1, Modificado2, Modificado3, Modificado4  FROM Procesado";

            return strCommand;
        }

        public static string Get_SQL_ProductoProcesado()
        {
            string strCommand = "SELECT IdDetalle, WhsCode, ItmsGrpNam, ItemCode, ItemName, Cant1, Cant2, TotalSAP, TotalContado, TotalDesviacion, PorcDif, Uom, Onhand, PorProcesar, DifPesoNeto, Marbetes, Precio, VarianzaNeta, VarianzaNetaMonto, TotalContado2, TotalDesviacion2, PorcDif2, Ajustado FROM Procesado WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Get_SQL_Existe_ItemProcesado()
        {
            string strCommand = "SELECT COUNT(IdDetalle) FROM Procesado WHERE ItemCode = @ItemCode";

            return strCommand;
        }


        /// <summary>
        /// 2025-08-25: Nueva Instruccion proporcionada por Construrama para optimizar el proceso de ajustes (Sobrantes)
        /// </summary>
        /// <returns></returns>
        public static string Get_SQL_Procesado_Sobrante()
        {
            return @"
                SELECT P.IdDetalle, P.WhsCode, P.ItmsGrpNam, P.ItemCode, P.ItemName, 
                    P.Cant1, P.TotalContadoA as Cant2, P.TotalSAP, P.TotalContado, P.TotalDesviacion, 
                    P.PorcDif, P.Uom, P.Onhand, P.PorProcesar, P.DifPesoNeto, 
                    P.Marbetes, P.VarianzaNeta, P.VarianzaNetaMonto, 
                    P.TotalContado2, P.TotalDesviacion2, P.PorcDif2, P.Ajustado, IdUom=UM.UomEntry,
                    IW.AvgPrice AS Precio
                FROM Procesado P
	                LEFT JOIN [SAPSERVER]..SBO_CONSTRURAMABLANQUITA.VW_COSTOS_POR_ALMACEN IW 
		            ON P.ItemCode = IW.ItemCode AND P.WhsCode = IW.WhsCode
	                INNER JOIN [SAPSERVER]..SBO_CONSTRURAMABLANQUITA.OUOM UM
		                ON P.Uom=UM.UomCode 
                WHERE P.TotalContadoA > P.Onhand AND (P.TotalContadoA - P.Onhand) > 0 AND ISNULL(IW.AvgPrice, 0) > 0
                ORDER BY P.ItemCode";
        }

        /// <summary>
        /// 2025-08-25: Nueva Instruccion proporcionada por Construrama para optimizar el proceso de ajustes (Faltantes)
        /// </summary>
        /// <returns></returns>
        public static string Get_SQL_Procesado_Faltante()
        {
            return @"
                SELECT P.IdDetalle, P.WhsCode, P.ItmsGrpNam, P.ItemCode, P.ItemName, 
                    P.Cant1, P.TotalContadoA as Cant2, P.TotalSAP, P.TotalContado, P.TotalDesviacion, 
                    P.PorcDif, P.Uom, P.Onhand, P.PorProcesar, P.DifPesoNeto, 
                    P.Marbetes, P.VarianzaNeta, P.VarianzaNetaMonto, 
                    P.TotalContado2, P.TotalDesviacion2, P.PorcDif2, P.Ajustado, IdUom=UM.UomEntry,
                    IW.AvgPrice AS Precio
                FROM Procesado P
	                LEFT JOIN [SAPSERVER]..SBO_CONSTRURAMABLANQUITA.VW_COSTOS_POR_ALMACEN IW 
		            ON P.ItemCode = IW.ItemCode AND P.WhsCode = IW.WhsCode
	                INNER JOIN [SAPSERVER]..SBO_CONSTRURAMABLANQUITA.OUOM UM
		                ON P.Uom=UM.UomCode 
                WHERE P.TotalContadoA < P.Onhand AND (P.Onhand - P.TotalContadoA) > 0 AND ISNULL(IW.AvgPrice, 0) > 0
                ORDER BY P.ItemCode";
        }


        public static string Get_ProcesadoReporte2()
        {
            string strCommand = "SELECT IdDetalle, WhsCode, ItmsGrpNam, ItemCode, ItemName, Cant1, Cant2, TotalSAP, TotalContado, TotalDesviacion, PorcDif, Uom, Onhand, PorProcesar, DifPesoNeto, Marbetes, Precio, VarianzaNeta, VarianzaNetaMonto, TotalContado2, TotalDesviacion2, PorcDif2, Ajustado FROM Procesado WHERE ItmsGrpNam = @ItmsGrpNam AND VarianzaNetaMonto > @VarianzaNetaMonto";

            return strCommand;
        }

        public static string Get_ProcesadoReporteBaseZona()
        {
            string strCommand = "SELECT IdDetalle, WhsCode, ItmsGrpNam, ItemCode, ItemName, Cant1, Cant2, TotalSAP, TotalContado, TotalDesviacion, PorcDif, Uom, Onhand, PorProcesar, DifPesoNeto, Marbetes, Precio, VarianzaNeta, VarianzaNetaMonto, TotalContado2, TotalDesviacion2, PorcDif2, Ajustado FROM Procesado WHERE VarianzaNetaMonto > @VarianzaNetaMonto";

            return strCommand;
        }

        public static string Get_Reporte2ProductosZona()
        {
            string strCommand = "SELECT Z.IdZona, A.CodigoArticulo, A.Descripcion, SUM((A.Cantidad2 * A.BaseQty)) as Contado, A.Precio FROM MarbetesArticulos A LEFT JOIN Marbetes M ON A.IdMarbete = M.IdMarbete LEFT JOIN Zonas Z ON M.IdZona = Z.IdZona GROUP BY Z.IdZona, A.CodigoArticulo, A.Descripcion, A.Precio";

            return strCommand;
        }

        public static string Get_Reporte2ProductosZonaByIdZona()
        {
            string strCommand = "SELECT Z.IdZona, A.CodigoArticulo, A.Descripcion, SUM((A.Cantidad2 * A.BaseQty)) as Contado, A.Precio FROM MarbetesArticulos A LEFT JOIN Marbetes M ON A.IdMarbete = M.IdMarbete LEFT JOIN Zonas Z ON M.IdZona = Z.IdZona WHERE M.IdZona = @IdZona GROUP BY Z.IdZona, A.CodigoArticulo, A.Descripcion, A.Precio";

            return strCommand;
        }

        public static string Get_Reporte2ProductoZonaMarbetes()
        {
            string strCommand = "SELECT Marbetes = STUFF((SELECT ',' + CONVERT(VARCHAR(20), Marbete)  FROM (SELECT DISTINCT(Marbete) FROM MarbetesArticulos A LEFT JOIN Marbetes M ON M.IdMarbete = A.IdMarbete WHERE A.CodigoArticulo = @CodigoArticulo AND M.IdZona = @IdZona) as ListMarbetes FOR XML PATH ('')), 1, 1, '')";

            return strCommand;
        }

        public static string Get_SQL_DifMontosUno()
        {
            string strCommand = "SELECT WhsCode, ItmsGrpNam, SUM(TotalSAP) as TotalSAP, SUM(TotalContado) as TotalContado, SUM(TotalDesviacion) as TotalDesviacion, CASE WHEN SUM(TotalSAP) = 0 THEN 0 ELSE ((SUM(TotalDesviacion)/SUM(TotalSAP)) * 100) END as PorcDif FROM Procesado GROUP BY ItmsGrpNam, WhsCode";

            return strCommand;
        }

        public static string Get_SQL_DifMontosDos()
        {
            string strCommand = "SELECT WhsCode, ItmsGrpNam, SUM(TotalSAP) as TotalSAP, SUM(TotalContado2) as TotalContado, SUM(TotalDesviacion2) as TotalDesviacion, CASE WHEN SUM(TotalSAP) = 0 THEN 0 ELSE ((SUM(TotalDesviacion2)/SUM(TotalSAP)) * 100) END as PorcDif FROM Procesado GROUP BY ItmsGrpNam, WhsCode";

            return strCommand;
        }

        public static string Get_SQL_Articulos()
        {
            string strCommand = "SELECT MA.IdDetalle, MA.IdMarbete, M.Marbete, MA.CodigoArticulo, MA.CodigoBarras, MA.Descripcion, MA.Uom, MA.BaseQty, MA.Precio, MA.Costo, MA.Cantidad, MA.Cantidad2, MA.Capturo FROM MarbetesArticulos MA LEFT JOIN Marbetes M ON MA.IdMarbete = M.IdMarbete";
            return strCommand;
        }

        public static string Get_SQL_UomBaseArticulo()
        {
            string strCommand = "SELECT TOP(1) UomCode FROM SAP_Barcodes WHERE BaseQty = 1 AND ItemCode = @ItemCode";

            return strCommand;
        }

        public static string Insert_Procesado()
        {
            string strCommand = "INSERT INTO Procesado (WhsCode, ItmsGrpNam, ItemCode, ItemName, Cant1, Cant2, TotalSAP, TotalContado, TotalDesviacion, PorcDif, Uom, Onhand, PorProcesar, DifPesoNeto, Marbetes, Precio, VarianzaNeta, VarianzaNetaMonto, TotalContado2, TotalDesviacion2, PorcDif2, Ajustado, IdUom, Contado1, Contado2, Contado3, Contado4, TotalContadoR, TotalContadoA, DiferenciaContado, DiferenciaInventario, Modificado1, Modificado2, Modificado3, Modificado4) " +
                "VALUES (@WhsCode, @ItmsGrpNam, @ItemCode, @ItemName, @Cant1, @Cant2, @TotalSAP, @TotalContado, @TotalDesviacion, @PorcDif, @Uom, @Onhand, @PorProcesar, @DifPesoNeto, @Marbetes, @Precio, @VarianzaNeta, @VarianzaNetaMonto, @TotalContado2, @TotalDesviacion2, @PorcDif2, 0, @IdUom, @Contado1, @Contado2, @Contado3, @Contado4, @TotalContadoR, @TotalContadoA, @DiferenciaContado, @DiferenciaInventario, @Modificado1, @Modificado2, @Modificado3, @Modificado4)";

            return strCommand;
        }

        public static string Update_SQL_ArticuloAjustado()
        {
            string strCommand = "UPDATE Procesado SET Ajustado = 1 WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Get_SQL_PDAArticulos()
        {
            string strCommand = "SELECT Capturo, COUNT(*) as Cantidad FROM MarbetesArticulos GROUP BY Capturo";

            return strCommand;
        }

        public static string Update_SQL_AjusteConteoDos()
        {
            //string strCommand = "UPDATE Procesado SET Cant2 = @Cant2, VarianzaNeta = @VarianzaNeta, VarianzaNetaMonto = @VarianzaNetaMonto, TotalContado2 = @TotalContado2, TotalDesviacion2 = @TotalDesviacion2, PorcDif2 = @PorcDif2, Contado1 = @Contado1, Contado2 = @Contado2, Contado3 = @Contado3, Contado4 = @Contado4, TotalContadoR = @TotalContadoR, TotalContadoA = @TotalContadoA, DiferenciaContado = @DiferenciaContado, DiferenciaInventario = @DiferenciaInventario, Modificado1 = @Modificado1, Modificado2 = @Modificado2, Modificado3 = @Modificado3, Modificado4 = @Modificado4, DifPesoNeto = @DifPesoNeto WHERE IdDetalle = @IdDetalle";
            string strCommand = "UPDATE Procesado SET Contado1 = @Contado1, Contado2 = @Contado2, Contado3 = @Contado3, Contado4 = @Contado4, TotalContadoR = @TotalContadoR, TotalContadoA = @TotalContadoA, DiferenciaContado = @DiferenciaContado, DiferenciaInventario = @DiferenciaInventario, Modificado1 = @Modificado1, Modificado2 = @Modificado2, Modificado3 = @Modificado3, Modificado4 = @Modificado4, DifPesoNeto = @DifPesoNeto WHERE IdDetalle = @IdDetalle";

            return strCommand;
        }

        public static string Search_SQL_ExisteArticuloRegistrado()
        {
            string strCommand = "SELECT COUNT(*) FROM MarbetesArticulos WHERE IdMarbete = @IdMarbete AND CodigoArticulo = @ItemCode AND Capturo = @Capturo AND FechaCaptura = @Fecha";

            return strCommand;
        }

        public static string Get_SQL_TotalArticulosByMarbete()
        {
            string strCommand = "SELECT COUNT(*) FROM MarbetesArticulos WHERE IdMarbete = @IdMarbete";

            return strCommand;
        }
    }
}
