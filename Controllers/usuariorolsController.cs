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
    public class usuariorolsController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public usuariorolsController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/usuariorols
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<usuariorol>>> Getusuariosroles()
        {
            return await _context.usuariosroles.ToListAsync();
        }

        // GET: api/usuariorols/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuariorol>> Getusuariorol(int id)
        {
            var usuariorol = await _context.usuariosroles.FindAsync(id);

            if (usuariorol == null)
            {
                return NotFound();
            }

            return usuariorol;
        }

        // PUT: api/usuariorols/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusuariorol(int id, usuariorol usuariorol)
        {
            if (id != usuariorol.usuariorolid)
            {
                return BadRequest();
            }

            _context.Entry(usuariorol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuariorolExists(id))
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

        // POST: api/usuariorols
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<usuariorol>> Postusuariorol(usuariorol usuariorol)
        {
            _context.usuariosroles.Add(usuariorol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getusuariorol", new { id = usuariorol.usuariorolid }, usuariorol);
        }

        // DELETE: api/usuariorols/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusuariorol(int id)
        {
            var usuariorol = await _context.usuariosroles.FindAsync(id);
            if (usuariorol == null)
            {
                return NotFound();
            }

            _context.usuariosroles.Remove(usuariorol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool usuariorolExists(int id)
        {
            return _context.usuariosroles.Any(e => e.usuariorolid == id);
        }
    }
}
