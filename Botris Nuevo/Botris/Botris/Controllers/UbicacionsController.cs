using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Botris.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Botris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionsController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public UbicacionsController(AplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ubicacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones()
        {
          if (_context.Ubicaciones == null)
          {
              return NotFound();
          }
            return await _context.Ubicaciones.ToListAsync();
        }

        // GET: api/Ubicacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
          if (_context.Ubicaciones == null)
          {
              return NotFound();
          }
            var ubicacion = await _context.Ubicaciones.FindAsync(id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            return ubicacion;
        }

        // PUT: api/Ubicacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(ubicacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ubicacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
          if (_context.Ubicaciones == null)
          {
              return Problem("Entity set 'AplicationDbContext.Ubicaciones'  is null.");
          }
            _context.Ubicaciones.Add(ubicacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUbicacion", new { id = ubicacion.Id }, ubicacion);
        }

        // DELETE: api/Ubicacions/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUbicacion(int id)
        {
            if (_context.Ubicaciones == null)
            {
                return NotFound();
            }
            var identity = HttpContext.User.Identity as ClaimsIdentity;
                var rToken = Jwt.ValidarToken(identity,_context);
            if (!rToken.success)
            {
                return StatusCode(401, rToken); // Devuelve un código 401 no autorizado
            }
            var asd = rToken.result;
            if (asd.Rol != "administrador")
            {
                IActionResult result = Ok(new
                {
                    success = false,
                    messagge = "eliminado no tienes permiso",
                    result = ""
                });
            }
            var ubicacion =  _context.Ubicaciones.Find(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            _context.Ubicaciones.Remove(ubicacion);
             _context.SaveChanges();

            return NoContent();
        }

        private bool UbicacionExists(int id)
        {
            return (_context.Ubicaciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
