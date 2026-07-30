using Blanquita_Inventarios.Entities;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace Blanquita_Inventarios.Site.Helpers
{
    public class ExportExcel
    {
        public static DBResponse<string> GenerarExcel(DataTable dataTable, string tituloExcel, string nombreArchivo)
        {
            var response = new DBResponse<string>();

            try
            {
                // Obtener la ruta física completa
                string carpetaDescargas = Path.Combine(Config.DirectorioDocumentos, "Descargas");
                // Crear la carpeta si no existe
                if (!Directory.Exists(carpetaDescargas))
                {
                    Directory.CreateDirectory(carpetaDescargas);
                }

                string rutaCompleta = Path.Combine(carpetaDescargas, nombreArchivo);

                // Crear el libro Excel
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(tituloExcel ?? "Datos");

                int rowIndex = 0;

                // ===== TÍTULO =====
                if (!string.IsNullOrEmpty(tituloExcel))
                {
                    IFont fontTitulo = workbook.CreateFont();
                    fontTitulo.Color = HSSFColor.Blue.Index;
                    fontTitulo.Boldweight = (short)FontBoldWeight.Bold;
                    fontTitulo.FontHeightInPoints = 16;

                    ICellStyle estiloTitulo = workbook.CreateCellStyle();
                    estiloTitulo.SetFont(fontTitulo);
                    estiloTitulo.Alignment = HorizontalAlignment.Center;

                    IRow rowTitulo = sheet.CreateRow(rowIndex++);
                    ICell celdaTitulo = rowTitulo.CreateCell(0);
                    celdaTitulo.SetCellValue(tituloExcel);
                    celdaTitulo.CellStyle = estiloTitulo;

                    if (dataTable.Columns.Count > 0)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dataTable.Columns.Count - 1));
                    }

                    rowIndex++; // Espacio
                }

                // ===== ENCABEZADOS =====
                IFont fontEncabezado = workbook.CreateFont();
                fontEncabezado.Color = HSSFColor.Blue.Index;
                fontEncabezado.Boldweight = (short)FontBoldWeight.Bold;

                ICellStyle estiloEncabezado = workbook.CreateCellStyle();
                estiloEncabezado.SetFont(fontEncabezado);
                estiloEncabezado.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                estiloEncabezado.FillPattern = FillPattern.SolidForeground;
                estiloEncabezado.BorderBottom = BorderStyle.Thin;
                estiloEncabezado.BorderTop = BorderStyle.Thin;
                estiloEncabezado.BorderLeft = BorderStyle.Thin;
                estiloEncabezado.BorderRight = BorderStyle.Thin;

                IRow rowEncabezados = sheet.CreateRow(rowIndex++);
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    ICell celda = rowEncabezados.CreateCell(c);
                    celda.SetCellValue(dataTable.Columns[c].ColumnName);
                    celda.CellStyle = estiloEncabezado;
                }

                // ===== DATOS =====
                ICellStyle estiloNormal = workbook.CreateCellStyle();
                estiloNormal.BorderBottom = BorderStyle.Thin;
                estiloNormal.BorderTop = BorderStyle.Thin;
                estiloNormal.BorderLeft = BorderStyle.Thin;
                estiloNormal.BorderRight = BorderStyle.Thin;

                ICellStyle estiloAlterno = workbook.CreateCellStyle();
                estiloAlterno.BorderBottom = BorderStyle.Thin;
                estiloAlterno.BorderTop = BorderStyle.Thin;
                estiloAlterno.BorderLeft = BorderStyle.Thin;
                estiloAlterno.BorderRight = BorderStyle.Thin;
                estiloAlterno.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                estiloAlterno.FillPattern = FillPattern.SolidForeground;

                for (int r = 0; r < dataTable.Rows.Count; r++)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    bool esAlterno = r % 2 == 1;

                    for (int c = 0; c < dataTable.Columns.Count; c++)
                    {
                        ICell celda = row.CreateCell(c);
                        object valor = dataTable.Rows[r][c];

                        if (valor == null || valor == DBNull.Value)
                        {
                            celda.SetCellValue("");
                        }
                        else if (valor is DateTime)
                        {
                            celda.SetCellValue(((DateTime)valor).ToString("dd/MM/yyyy HH:mm:ss"));
                        }
                        else if (valor is bool)
                        {
                            celda.SetCellValue((bool)valor ? "Sí" : "No");
                        }
                        else if (valor is decimal || valor is double || valor is int || valor is float)
                        {
                            celda.SetCellValue(Convert.ToDouble(valor));
                        }
                        else
                        {
                            celda.SetCellValue(valor.ToString());
                        }

                        celda.CellStyle = esAlterno ? estiloAlterno : estiloNormal;
                    }
                }

                // ===== AUTO AJUSTAR COLUMNAS =====
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    sheet.AutoSizeColumn(c);
                }

                // ===== GUARDAR EL ARCHIVO =====
                using (FileStream fileStream = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fileStream);
                }

                // Verificar que el archivo se creó
                if (File.Exists(rutaCompleta))
                {
                    response.ExecutionOK = true;
                    response.Data = nombreArchivo;
                    response.Message = "Archivo generado correctamente";
                }
                else
                {
                    response.ExecutionOK = false;
                    response.Message = "No se pudo crear el archivo";
                }
            }
            catch (Exception ex)
            {
                response.ExecutionOK = false;
                response.Message = "Error al generar Excel: " + ex.Message;
            }

            return response;
        }

        // Método de compatibilidad
        public static DBResponse<string> GrabaArchivoExcelSimple(DataTable dataTable, string tituloExcel, string nombreArchivo)
        {
            return GenerarExcel(dataTable, tituloExcel, nombreArchivo);
        }
    }
}