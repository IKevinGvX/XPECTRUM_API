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
    public class pasajeroreservasController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public pasajeroreservasController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/pasajeroreservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pasajeroreserva>>> Getpasajerosreservas()
        {
            return await _context.pasajerosreservas.ToListAsync();
        }

        // GET: api/pasajeroreservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pasajeroreserva>> Getpasajeroreserva(int id)
        {
            var pasajeroreserva = await _context.pasajerosreservas.FindAsync(id);

            if (pasajeroreserva == null)
            {
                return NotFound();
            }

            return pasajeroreserva;
        }

        // PUT: api/pasajeroreservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpasajeroreserva(int id, pasajeroreserva pasajeroreserva)
        {
            if (id != pasajeroreserva.pasajeroreservaid)
            {
                return BadRequest();
            }

            _context.Entry(pasajeroreserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pasajeroreservaExists(id))
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

        // POST: api/pasajeroreservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<pasajeroreserva>> Postpasajeroreserva(pasajeroreserva pasajeroreserva)
        {
            _context.pasajerosreservas.Add(pasajeroreserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpasajeroreserva", new { id = pasajeroreserva.pasajeroreservaid }, pasajeroreserva);
        }

        // DELETE: api/pasajeroreservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepasajeroreserva(int id)
        {
            var pasajeroreserva = await _context.pasajerosreservas.FindAsync(id);
            if (pasajeroreserva == null)
            {
                return NotFound();
            }

            _context.pasajerosreservas.Remove(pasajeroreserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool pasajeroreservaExists(int id)
        {
            return _context.pasajerosreservas.Any(e => e.pasajeroreservaid == id);
        }
    }
}
