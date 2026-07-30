using Blanquita_Inventarios.Entities;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Blanquita_Inventarios.Site.Helpers
{
    public class ExportExcel
    {
        /// <summary>
        /// Genera un archivo de excel con un listado de datos usando la plantilla ListadoSimple.xls
        /// </summary>
        /// <param name="dataTable">DataTable con el conjunto de datos a exportar.</param>
        /// <param name="nombreArchivo">Nombre sugerido para el archivo resultante.</param>
        /// <param name="tituloReporte">Título que tendrá el reporte.</param>
        /// <param name="filtrosReporte">Texto con los filtros aplicados al reporte, para ser mostrado al usuario.</param>
        /// <param name="usuario">Nombre de la persona que solicitó el reporte.</param>
        /// <param name="formatoEntrega">Formato para pantalla o para archivo en servidor.</param>
        public static DBResponse<string> GrabaArchivoExcelSimple(DataTable dataTable, string tituloExcel, string nombreArchivo)
        {
            var response = new DBResponse<string>();

            string plantillaArchivo = "ListadoSimple.xls";
            string templateFile = Config.DirectorioPlantillas + "\\" + plantillaArchivo;
            string pathDescargas = Config.DirectorioDocumentos + "\\Descargas";

            if (!Directory.Exists(pathDescargas))
                Directory.CreateDirectory(pathDescargas);

            string fileSave = pathDescargas + "\\" + nombreArchivo;

            int constanteFIRSTROW = String.IsNullOrEmpty(tituloExcel) ? 1 : 2; // Primer renglón del listado (corresponde al encabezado)

            string reportLegend = string.Empty;

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            FileStream file = new FileStream(templateFile, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Development TI";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;

            ISheet sheet1 = hssfworkbook.GetSheet("Listado");

            if (!String.IsNullOrEmpty(tituloExcel))
            {
                IFont fontTitulo = hssfworkbook.CreateFont();
                fontTitulo.Color = HSSFColor.Blue.Index;
                fontTitulo.Boldweight = (short)FontBoldWeight.Bold;
                fontTitulo.FontHeightInPoints = 20;
                ICellStyle estiloTitulo = hssfworkbook.CreateCellStyle();
                estiloTitulo.SetFont(fontTitulo);
                ICell celdaTitulo = sheet1.CreateRow(0).CreateCell(0);
                celdaTitulo.SetCellValue(tituloExcel);
                celdaTitulo.CellStyle = estiloTitulo;
                //sheet1.CreateRow(1).CreateCell(0).SetCellValue(reportLegend + filtrosReporte);
            }

            IFont fontEncabezado = hssfworkbook.CreateFont();
            fontEncabezado.Color = HSSFColor.Blue.Index;
            fontEncabezado.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle estiloEncabezado = hssfworkbook.CreateCellStyle();
            estiloEncabezado.SetFont(fontEncabezado);
            IRow rowEncabezados = sheet1.CreateRow(constanteFIRSTROW - 1);
            for (int c = 0; c < dataTable.Columns.Count; c++)
            {
                string value = dataTable.Columns[c].ColumnName;
                ICell celda = rowEncabezados.CreateCell(c);
                celda.SetCellValue(value);
                celda.CellStyle = estiloEncabezado;
            }

            ICellStyle estiloRenglonNormal = hssfworkbook.CreateCellStyle();
            estiloRenglonNormal.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            estiloRenglonNormal.FillPattern = FillPattern.SolidForeground;
            ICellStyle estiloRenglonAlterno = hssfworkbook.CreateCellStyle();
            estiloRenglonAlterno.FillForegroundColor = HSSFColor.White.Index;
            estiloRenglonAlterno.FillPattern = FillPattern.SolidForeground;

            // Coloca los datos del data table
            bool esRenglonNormal = true;
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                IRow row = sheet1.CreateRow(constanteFIRSTROW + r);
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    string value;
                    ICell celda = row.CreateCell(c);

                    if (dataTable.Columns[c].DataType == Type.GetType("System.DateTime"))
                    {
                        if (dataTable.Rows[r][c].ToString() != "")
                            //value = DateTime.Parse(dataTable.Rows[r][c].ToString()).ToString("dd/MM/yyyy"); -- Se cambio para que en el historico saliera con la hora del registro
                            value = dataTable.Rows[r][c].ToString();
                        else
                            value = "";
                    }
                    else if (dataTable.Columns[c].DataType == Type.GetType("System.Boolean"))
                        value = dataTable.Rows[r][c].ToString().Equals("True") ? "Sí" : "No";
                    else
                        value = dataTable.Rows[r][c].ToString();

                    celda.SetCellValue(value);

                    if (esRenglonNormal)
                    {
                        celda.CellStyle = estiloRenglonNormal;
                    }
                    else
                    {
                        celda.CellStyle = estiloRenglonAlterno;
                    }


                }
                esRenglonNormal = !esRenglonNormal;
            }

            // Pone firma
            IFont fontFirma = hssfworkbook.CreateFont();
            fontFirma.Color = HSSFColor.Grey50Percent.Index;
            ICellStyle estiloFirma = hssfworkbook.CreateCellStyle();
            estiloFirma.SetFont(fontFirma);

            FileStream sw = System.IO.File.Create(fileSave);
            hssfworkbook.Write(sw);
            sw.Close();
            sw.Dispose();

            response.ExecutionOK = true;
            response.Data = nombreArchivo;

            return response;
        }
    }
}