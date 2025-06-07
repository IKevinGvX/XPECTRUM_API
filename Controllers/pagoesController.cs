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
    public class pagoesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public pagoesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/pagoes
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<pago>>> Getpagos()
        {
            return await _context.pagos.ToListAsync();
        }

        // GET: api/pagoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pago>> Getpago(int id)
        {
            var pago = await _context.pagos.FindAsync(id);

            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

        // PUT: api/pagoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpago(int id, pago pago)
        {
            if (id != pago.pagoid)
            {
                return BadRequest();
            }

            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pagoExists(id))
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

        // POST: api/pagoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<pago>> Postpago(pago pago)
        {
            _context.pagos.Add(pago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpago", new { id = pago.pagoid }, pago);
        }

        // DELETE: api/pagoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepago(int id)
        {
            var pago = await _context.pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            _context.pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool pagoExists(int id)
        {
            return _context.pagos.Any(e => e.pagoid == id);
        }
    }
}
