using Botris.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Transactions;

namespace Botris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Asignar : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public Asignar(AplicationDbContext context)
        {
            _context = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            if (_context.Productos == null)
            {
                return BadRequest();
            }
            return await _context.Productos.ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        {
            try
            {
                var tProductos = await _context.Productos.ToListAsync();
                if (tProductos != null && tProductos.Any())
                {
                    _context.Productos.RemoveRange(tProductos);
                    await _context.SaveChangesAsync();
                }

                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");
                var filePath = Path.Combine("C:\\Users\\Alejandro\\Desktop\\Botris Nuevo\\Botris Nuevo\\Botris", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Supongamos que el archivo tiene solo una hoja
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var nombre = worksheet.Cells[row, 1].Text; // Accede al valor de la celda
                        var barras = worksheet.Cells[row, 2].Text;
                        var fecha = worksheet.Cells[row, 3].Text;
                        DateTime fechaCreacion;
                        if (DateTime.TryParse(fecha, out fechaCreacion))
                        {
                            var sku = worksheet.Cells[row, 4].Text;
                            var marca = worksheet.Cells[row, 5].Text;
                            var rfid = worksheet.Cells[row, 6].Text;
                            var nombreSucursal = worksheet.Cells[row, 7].Text;
                            var codigo = worksheet.Cells[row, 8].Text;

                            var productos = new Producto
                            {
                                Nombre = nombre,
                                CodigoBarras = barras,
                                FechaCreacion = fechaCreacion,
                                SKU = sku,
                                Marca = marca,
                                RFID = rfid,
                                NombreSucursal = nombreSucursal,
                                Codigo = codigo

                            };
                            _context.Productos.Add(productos);
                        }
                        else
                        {
                            // Manejar el escenario en el que la fecha no se pueda convertir
                            // Puedes registrar un mensaje de error o realizar alguna otra acción
                            Console.WriteLine($"Error al convertir la fecha en la fila {row}");
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new { Message = "File uploaded and processed successfully. "});
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing the file: {ex.Message}");
            }
          
        }

        [HttpPost]
        [Route("InventarioAsignar")]
        public async Task<IActionResult> AsignarInventario()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (_context.Productos == null) return NotFound("No existen Productos");
                    var productos = await _context.Productos.ToListAsync();
                    var maxCabecera = await _context.Inventarios.MaxAsync(i => i.Cabecera);
                    if (maxCabecera == null) return NotFound("No existe Productos"); maxCabecera=maxCabecera + 1;
                    if (productos.Any())
                    {
                        foreach (var producto in productos)
                        {
                            await _context.Inventarios.AddAsync(new Inventario
                            { 
                                Cabecera= maxCabecera,
                                Nombre = producto.Nombre,
                                CodigoBarras = producto.CodigoBarras,
                                FechaCreacion = producto.FechaCreacion,
                                SKU = producto.SKU,
                                RFID = producto.RFID,
                                Marca = producto.Marca,
                                NombreSucursal = producto.NombreSucursal,
                                CodigoSucursal = producto.Codigo,
                            });

                        }
                        await _context.SaveChangesAsync(); 
                        scope.Complete();
                        return Ok("Productos Almacenados");
                    }
                    else
                    {
                        return BadRequest("No existen productos");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest($"Error: {ex.Message}");
                }
            }
            }

    }
}
