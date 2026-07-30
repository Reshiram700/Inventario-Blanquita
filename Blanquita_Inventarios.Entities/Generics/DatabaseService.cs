using Blanquita_Inventarios.Entities.Generics;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.Entities
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        public DatabaseService(string appDataDirectory)
        {
            _db = new SQLiteAsyncConnection(
                Path.Combine(appDataDirectory, "Inventario03.db3"));
        }


        // CRUD Operations de Articulos
        public async Task<int> Guardar_SAP_Articulo_Async(SAP_Articulos_Sqlite item)
        {
            return await _db.InsertAsync(item); // Insertar nuevo
        }

        public async Task<List<SAP_Articulos_Sqlite>> Obtener_SAP_Articulos_Async()
        {
            return await _db.Table<SAP_Articulos_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SAP_Articulo_Async(SAP_Articulos_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SAP_Articulos_Async()
        {
            return await _db.DeleteAllAsync<SAP_Articulos_Sqlite>();
        }

        // CRUD Operations de Barcode
        public async Task<int> Guardar_SAP_Barcode_Async(SAP_Barcodes_Sqlite item)
        {
            return await _db.InsertAsync(item); // Insertar nuevo
        }

        public async Task<List<SAP_Barcodes_Sqlite>> Obtener_SAP_Barcodes_Async()
        {
            return await _db.Table<SAP_Barcodes_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SAP_Barcodes_Async(SAP_Barcodes_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SAP_Barcodes_Async()
        {
            return await _db.DeleteAllAsync<SAP_Barcodes_Sqlite>();
        }

        //Zonas
        public async Task<int> Guardar_SI_Zonas_Async(SI_Zonas_Sqlite item)
        {
            return await _db.InsertAsync(item);
        }

        public async Task<List<SI_Zonas_Sqlite>> Obtener_SI_Zonas_Async()
        {
            return await _db.Table<SI_Zonas_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SI_Zonas_Async(SI_Zonas_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SI_Zonas_Async()
        {
            return await _db.DeleteAllAsync<SI_Zonas_Sqlite>();
        }

        //Marbetes
        public async Task<int> Guardar_SI_Marbetes_Async(SI_Marbetes_Sqlite item)
        {
            return await _db.InsertAsync(item);
        }

        public async Task<List<SI_Marbetes_Sqlite>> Obtener_SI_Marbetes_Async()
        {
            return await _db.Table<SI_Marbetes_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SI_Marbetes_Async(SI_Marbetes_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SI_Marbetes_Async()
        {
            return await _db.DeleteAllAsync<SI_Marbetes_Sqlite>();
        }

        //Marbetes Articulos
        public async Task<int> Guardar_SI_MarbetesArticulos_Async(SI_MarbetesArticulos_Sqlite item)
        {
            return await _db.InsertAsync(item);
        }

        public async Task<List<SI_MarbetesArticulos_Sqlite>> Obtener_SI_MarbetesArticulos_Async()
        {
            return await _db.Table<SI_MarbetesArticulos_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SI_MarbetesArticulos_Async(SI_MarbetesArticulos_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SI_MarbetesArticulos_Async()
        {
            return await _db.DeleteAllAsync<SI_MarbetesArticulos_Sqlite>();
        }

        //Parametrizacion
        public async Task<int> Guardar_SI_Parametrizacion_Async(SI_Parametrizacion_Sqlite item)
        {
            return await _db.InsertAsync(item);
        }

        public async Task<List<SI_Parametrizacion_Sqlite>> Obtener_SI_Parametrizacion_Async()
        {
            return await _db.Table<SI_Parametrizacion_Sqlite>().ToListAsync();
        }

        public async Task<int> Eliminar_SI_Parametrizacion_Async(SI_Parametrizacion_Sqlite item)
        {
            return await _db.DeleteAsync(item);
        }

        public async Task<int> Eliminar_SI_Parametrizacion_Async()
        {
            return await _db.DeleteAllAsync<SI_Parametrizacion_Sqlite>();
        }

        public async Task InsertarArticulosBulkAsync(List<SAP_Articulos_Sqlite> items)
        {
            await _db.RunInTransactionAsync(tran =>
            {
                tran.InsertAll(items);
            });
        }

        public async Task InsertarBarcodesBulkAsync(List<SAP_Barcodes_Sqlite> items)
        {
            await _db.RunInTransactionAsync(tran =>
            {
                tran.InsertAll(items);
            });
        }

        public async Task<int> Count_ArticulosByMarbete(int marbete)
        {
            var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>()
                                     .Where(w => w.Marbete == marbete)
                                     .FirstOrDefaultAsync();

            if (dbMarbete == null)
                return 0;

            return await _db.Table<SI_MarbetesArticulos_Sqlite>()
                            .Where(w => w.IdMarbete == dbMarbete.IdMarbete)
                            .CountAsync();
        }


        public async Task InsertarZonasBulkAsync(List<SI_Zonas_Sqlite> items)
        {
            await _db.RunInTransactionAsync(tran =>
            {
                tran.InsertAll(items);
            });
        }

        public async Task InsertarMarbetesBulkAsync(List<SI_Marbetes_Sqlite> items)
        {
            await _db.RunInTransactionAsync(tran =>
            {
                tran.InsertAll(items);
            });
        }

        private bool _initialized = false;

        public async Task InitAsync()
        {
            if (_initialized) return;

            await _db.CreateTableAsync<SAP_Articulos_Sqlite>();
            await _db.CreateTableAsync<SAP_Barcodes_Sqlite>();
            await _db.CreateTableAsync<SI_Parametrizacion_Sqlite>();
            await _db.CreateTableAsync<SI_Zonas_Sqlite>();
            await _db.CreateTableAsync<SI_Marbetes_Sqlite>();
            await _db.CreateTableAsync<SI_MarbetesArticulos_Sqlite>();

            _initialized = true;
        }

        #region 

        public async Task<int> Update_Parametrizacion(SI_Parametrizacion_Sqlite parametrizacion)
        {
            return await _db.UpdateAsync(parametrizacion);
        }

        public async Task<DBResponse<SI_Marbetes_Sqlite>> Get_Marbete(int marbete)
        {
            var response = new DBResponse<SI_Marbetes_Sqlite>();

            var result = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == marbete).FirstOrDefaultAsync();
            response.ExecutionOK = true;
            response.Data = result;

            return response;
        }

        public async Task<DatosBarcode> Buscar_DatosScanBarcodeProducto_Async(string barcode)
        {
            DatosBarcode datos = new DatosBarcode();

            var result = await _db.Table<SAP_Barcodes_Sqlite>().Where(w => w.BcdCode == barcode).FirstOrDefaultAsync();
            if (result != null)
            {
                datos.Codigo = barcode;
                datos.ItemCode = result.ItemCode;                
                var resultName = await _db.Table<SAP_Articulos_Sqlite>().Where(w => w.ItemCode == result.ItemCode).FirstOrDefaultAsync();
                if (resultName != null)
                {
                    datos.Descripcion = resultName.ItemName;
                    datos.Precio = resultName.Price;
                }
                datos.Uom = result.UomCode;
                datos.BaseQty = result.BaseQty;
            }

            return datos;
        }

        public async Task<DatosBarcode> Buscar_DatosItemCodeProducto_Async(string itemCode)
        {
            DatosBarcode datos = new DatosBarcode();

            var result = await _db.Table<SAP_Barcodes_Sqlite>().Where(w => w.ItemCode == itemCode).FirstOrDefaultAsync();
            if (result != null)
            {
                datos.Codigo = "";
                datos.ItemCode = result.ItemCode;
                var resultName = await _db.Table<SAP_Articulos_Sqlite>().Where(w => w.ItemCode == result.ItemCode).FirstOrDefaultAsync();
                if (resultName != null)
                    datos.Descripcion = resultName.ItemName;
                datos.Uom = result.UomCode;
            }

            return datos;
        }

        public async Task<DBResponse<int>> Insert_ArticuloMarbete_Async(DatosBarcode datos)
        {
            var response = new DBResponse<int>();

            try
            {
                var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == datos.Marbete).FirstOrDefaultAsync();
                if (dbMarbete != null)
                {
                    if (dbMarbete.Estatus == 1)
                    {
                        var existenArticulos = await _db.Table<SI_MarbetesArticulos_Sqlite>().Where(w => w.IdMarbete == dbMarbete.IdMarbete).FirstOrDefaultAsync();
                        if (existenArticulos == null)
                        {
                            //Actualizamos la fecha de inicio de captura de articulos en el marbete
                            dbMarbete.Inicio = DateTime.Now;
                            await _db.UpdateAsync(dbMarbete);
                        }

                        SI_MarbetesArticulos_Sqlite registrar = new SI_MarbetesArticulos_Sqlite
                        {
                            IdMarbete = dbMarbete.IdMarbete,
                            Barcode = datos.Codigo,
                            Codigo = datos.ItemCode,
                            Descripcion = datos.Descripcion,
                            Uom = datos.Uom,
                            BaseQty = datos.BaseQty,
                            Precio = datos.Precio,
                            Costo = datos.Costo,
                            Cantidad = datos.Cantidad,
                            Capturo = datos.NombrePDA,
                            FechaRegistro = DateTime.Now
                        };
                        await _db.InsertAsync(registrar);

                        response.ExecutionOK = true;
                        response.Data = registrar.IdDetalle;
                    }
                    else
                    {
                        response.Message = "No es posible grabar producto debido a que el Marbete esta cerrado";
                    }
                }
                else
                {
                    response.Message = "El Marbete no existe en la Base de Datos";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<SI_Marbetes_Sqlite>> Cerrar_Marbete(int marbete, string nombrePda)
        {
            var response = new DBResponse<SI_Marbetes_Sqlite>();

            try
            {
                var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == marbete).FirstOrDefaultAsync();
                if (dbMarbete != null)
                {
                    dbMarbete.Fin = DateTime.Now;
                    dbMarbete.Capturo = nombrePda;
                    dbMarbete.Estatus = 2;
                    await _db.UpdateAsync(dbMarbete);

                    response.ExecutionOK = true;
                    response.Data = dbMarbete;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<List<SI_Marbetes_Sqlite>>> Get_MarbetesCapturados()
        {
            var response = new DBResponse<List<SI_Marbetes_Sqlite>>();

            try
            {
                var query = await _db.Table<SI_MarbetesArticulos_Sqlite>().ToListAsync();
                if (query != null && query.Count > 0)
                {
                    var listId = query.Select(s => s.IdMarbete).Distinct().ToList();

                    var queryMarbetes = await _db.Table<SI_Marbetes_Sqlite>().Where(w => listId.Contains(w.IdMarbete)).ToListAsync();

                    response.ExecutionOK = true;
                    response.Data = queryMarbetes;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<List<MarbetesCapturados>>> Get_MarbetesCapturadosProductos()
        {
            var response = new DBResponse<List<MarbetesCapturados>>();
            List<MarbetesCapturados> listado = new List<MarbetesCapturados>();

            try
            {
                var query = await _db.Table<SI_MarbetesArticulos_Sqlite>().ToListAsync();
                if (query != null && query.Count > 0)
                {
                    var listId = query.Select(s => s.IdMarbete).Distinct().ToList();

                    var queryMarbetes = await _db.Table<SI_Marbetes_Sqlite>().Where(w => listId.Contains(w.IdMarbete)).ToListAsync();
                                        
                    foreach (var m in queryMarbetes)
                    {
                        int cantidadProductos = await _db.Table<SI_MarbetesArticulos_Sqlite>().Where(w => w.IdMarbete == m.IdMarbete).CountAsync();

                        listado.Add(new MarbetesCapturados { 
                            IdZona = m.IdZona,
                            Marbete = m.Marbete,
                            Estatus = m.Estatus,
                            CantidadProductos = cantidadProductos
                        });
                    }

                    response.ExecutionOK = true;
                    response.Data = listado;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<List<SI_Marbetes_Sqlite>>> Get_MarbetesDescargados()
        {
            var response = new DBResponse<List<SI_Marbetes_Sqlite>>();

            try
            {
                var query = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Estatus == 3).ToListAsync();

                response.ExecutionOK = true;
                response.Data = query;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<List<MarbetesCapturados>>> Get_MarbetesPendientesDescargar()
        {
            var response = new DBResponse<List<MarbetesCapturados>>();
            List<MarbetesCapturados> listado = new List<MarbetesCapturados>();

            try
            {
                var query = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Estatus == 2).ToListAsync();

                var listId = query.Select(s => s.IdMarbete).Distinct().ToList();

                foreach (var m in query)
                {
                    int cantidadProductos = await _db.Table<SI_MarbetesArticulos_Sqlite>().Where(w => w.IdMarbete == m.IdMarbete).CountAsync();

                    listado.Add(new MarbetesCapturados
                    {
                        IdZona = m.IdZona,
                        IdMarbete = m.IdMarbete,
                        Marbete = m.Marbete,
                        Estatus = m.Estatus,
                        CantidadProductos = cantidadProductos
                    });
                }

                response.ExecutionOK = true;
                response.Data = listado;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<SI_Marbetes_Sqlite>> Abrir_Marbete(int marbete)
        {
            var response = new DBResponse<SI_Marbetes_Sqlite>();

            try
            {
                var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == marbete).FirstOrDefaultAsync();
                if (dbMarbete != null)
                {
                    if (dbMarbete.Estatus == 2)
                    {
                        dbMarbete.Estatus = 1;
                        await _db.UpdateAsync(dbMarbete);

                        response.ExecutionOK = true;
                        response.Data = dbMarbete;
                    }
                    else
                    {
                        response.Message = "No es posible abrir el Marbete";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<List<SI_MarbetesArticulos_Sqlite>>> Get_MarbeteProductos(int marbete)
        {
            var response = new DBResponse<List<SI_MarbetesArticulos_Sqlite>>();

            try
            {
                var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == marbete).FirstOrDefaultAsync();
                if (dbMarbete != null)
                {
                    if (dbMarbete.Estatus == 2)
                    {
                        var query = await _db.Table<SI_MarbetesArticulos_Sqlite>().Where(w => w.IdMarbete == dbMarbete.IdMarbete).ToListAsync();

                        response.ExecutionOK = true;
                        response.Data = query;
                    }
                    else
                    {
                        response.Message = "El Marbete no se encuentra cerrado, revise la información";
                    }
                }
                else
                {
                    response.Message = "No se encontro información del Marbete";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DBResponse<SI_Marbetes_Sqlite>> Update_MarbeteDescargado(int marbete)
        {
            var response = new DBResponse<SI_Marbetes_Sqlite>();

            try
            {
                var dbMarbete = await _db.Table<SI_Marbetes_Sqlite>().Where(w => w.Marbete == marbete).FirstOrDefaultAsync();
                if (dbMarbete != null)
                {
                    dbMarbete.Estatus = 3;
                    await _db.UpdateAsync(dbMarbete);

                    response.ExecutionOK = true;
                    response.Data = dbMarbete;
                }
                else
                {
                    response.Message = "No se encontro información del Marbete";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion
    }
}
