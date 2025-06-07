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
    public class permisoesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public permisoesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/permisoes
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<permiso>>> Getpermisos()
        {
            return await _context.permisos.ToListAsync();
        }

        // GET: api/permisoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<permiso>> Getpermiso(string id)
        {
            var permiso = await _context.permisos.FindAsync(id);

            if (permiso == null)
            {
                return NotFound();
            }

            return permiso;
        }

        // PUT: api/permisoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpermiso(string id, permiso permiso)
        {
            if (id != permiso.permisocodigo)
            {
                return BadRequest();
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!permisoExists(id))
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

        // POST: api/permisoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<permiso>> Postpermiso(permiso permiso)
        {
            _context.permisos.Add(permiso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (permisoExists(permiso.permisocodigo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("Getpermiso", new { id = permiso.permisocodigo }, permiso);
        }

        // DELETE: api/permisoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepermiso(string id)
        {
            var permiso = await _context.permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.permisos.Remove(permiso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool permisoExists(string id)
        {
            return _context.permisos.Any(e => e.permisocodigo == id);
        }
    }
}
