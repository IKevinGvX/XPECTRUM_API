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
    public class logaccesoesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public logaccesoesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/logaccesoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<logacceso>>> Getlogsaccesos()
        {
            return await _context.logsaccesos.ToListAsync();
        }

        // GET: api/logaccesoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<logacceso>> Getlogacceso(int id)
        {
            var logacceso = await _context.logsaccesos.FindAsync(id);

            if (logacceso == null)
            {
                return NotFound();
            }

            return logacceso;
        }

        // PUT: api/logaccesoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlogacceso(int id, logacceso logacceso)
        {
            if (id != logacceso.logaccesoId)
            {
                return BadRequest();
            }

            _context.Entry(logacceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!logaccesoExists(id))
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

        // POST: api/logaccesoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<logacceso>> Postlogacceso(logacceso logacceso)
        {
            _context.logsaccesos.Add(logacceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getlogacceso", new { id = logacceso.logaccesoId }, logacceso);
        }

        // DELETE: api/logaccesoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletelogacceso(int id)
        {
            var logacceso = await _context.logsaccesos.FindAsync(id);
            if (logacceso == null)
            {
                return NotFound();
            }

            _context.logsaccesos.Remove(logacceso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool logaccesoExists(int id)
        {
            return _context.logsaccesos.Any(e => e.logaccesoId == id);
        }
    }
}
