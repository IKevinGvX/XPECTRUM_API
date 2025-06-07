using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;
using xpectrum_api.models;

namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificacionsController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public notificacionsController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/notificacions
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<notificacion>>> Getnotificaciones()
        {
            return await _context.notificaciones.ToListAsync();
        }

        // GET: api/notificacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<notificacion>> Getnotificacion(int id)
        {
            var notificacion = await _context.notificaciones.FindAsync(id);

            if (notificacion == null)
            {
                return NotFound();
            }

            return notificacion;
        }

        // PUT: api/notificacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putnotificacion(int id, notificacion notificacion)
        {
            if (id != notificacion.notificacionid)
            {
                return BadRequest();
            }

            _context.Entry(notificacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!notificacionExists(id))
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

        // POST: api/notificacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<notificacion>> Postnotificacion(notificacion notificacion)
        {
            _context.notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getnotificacion", new { id = notificacion.notificacionid }, notificacion);
        }

        // DELETE: api/notificacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletenotificacion(int id)
        {
            var notificacion = await _context.notificaciones.FindAsync(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            _context.notificaciones.Remove(notificacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool notificacionExists(int id)
        {
            return _context.notificaciones.Any(e => e.notificacionid == id);
        }
    }
}
