using Botris.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Botris.Controllers
{
    [Route("api/Reporte")]
    [ApiController]
    public class Reporte : ControllerBase
    {
        private readonly AplicationDbContext _context;
        public Reporte(AplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<List<InventarioDto>> GetDistinctSucursales()
        {
            var result = await _context.Inventarios
                .GroupBy(i =>  i.CodigoSucursal )
                .Select(group => new InventarioDto
                {
                    CodigoSucursal = group.Key,
                    Cabecera = group.Max(i => i.Cabecera), // Corregir la asignación de Cabecera
                    NombreSucursal = group.Max(i => i.NombreSucursal)
                })
        .ToListAsync();

            return result;
        }

        [HttpGet("BuscarUbicacion")]
        public async Task<IActionResult> Busqueda(string codigo)
        {
            try
            {
                var inventarios = await _context.Inventarios
                               .Where(i => i.CodigoSucursal == codigo)
                               .GroupBy(i => new { i.NombreSucursal, i.Cabecera })
                               .Select(group => new Inventario
                               {
                                   NombreSucursal = group.Key.NombreSucursal,
                                   Cabecera = group.Key.Cabecera,
                                   FechaCreacion = group.Max(i => i.FechaCreacion),

                               })
                               .ToListAsync();
                if (inventarios == null)
                {
                    return NotFound($"No se encontró un inventario en la ubicación ${codigo}");
                }

                return Ok(inventarios);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok(codigo);
        }


        [HttpGet("BuscarInventario")]
        public async Task<IActionResult> BusquedaInventario(int cabecera)
        {
            try
            {
                var inventario = await _context.Inventarios.FirstOrDefaultAsync(x => x.Cabecera == cabecera);
                if (inventario == null)
                {
                    return NotFound($"No se encontró un inventario con cabecera {cabecera}");
                }

                return Ok(inventario);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
