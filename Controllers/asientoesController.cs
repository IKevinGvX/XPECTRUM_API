using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using xpectrum_api.data;
using xpectrum_api.models;

namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsientosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public AsientosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/asientos
        // Procedimiento almacenado: spObtenerAsientos
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<asiento>>> GetAsientos()
        {
            var asientos = await _context.asientos
                .FromSqlRaw("EXEC spObtenerAsientos")
                .ToListAsync();
            return Ok(asientos);
        }

        // GET: api/asientos/5
        // Procedimiento almacenado: spObtenerAsientoPorId
        [HttpGet("{id}")]
        public async Task<ActionResult<asiento>> GetAsiento(int id)
        {
            var param = new SqlParameter("@asientoId", id);
            var asiento = await _context.asientos
                .FromSqlRaw("EXEC spObtenerAsientoPorId @asientoId", param)
                .FirstOrDefaultAsync();

            if (asiento == null)
                return NotFound();

            return Ok(asiento);
        }

        // POST: api/asientos
        // Procedimiento almacenado: spInsertarAsiento
        [HttpPost]
        public async Task<ActionResult<asiento>> PostAsiento(asiento asiento)
        {
            var parameters = new[]
            {
                new SqlParameter("@vueloId", asiento.vueloid),
                new SqlParameter("@numeroAsiento", asiento.numeroasiento),
                new SqlParameter("@clase", asiento.clase),
                new SqlParameter("@estadoAsiento", asiento.estadoasiento)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spInsertarAsiento @vueloId, @numeroAsiento, @clase, @estadoAsiento", parameters);

            // Aquí puedes retornar CreatedAtAction con el asiento, si el SP devuelve el nuevo Id, lo capturas.

            return CreatedAtAction(nameof(GetAsiento), new { id = asiento.asientoid }, asiento);
        }

        // PUT: api/asientos/5
        // Procedimiento almacenado: spActualizarAsiento
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsiento(int id, asiento asiento)
        {
            if (id != asiento.asientoid)
                return BadRequest();

            var parameters = new[]
            {
                new SqlParameter("@asientoId", asiento.asientoid),
                new SqlParameter("@vueloId", asiento.vueloid),
                new SqlParameter("@numeroAsiento", asiento.numeroasiento),
                new SqlParameter("@clase", asiento.clase),
                new SqlParameter("@estadoAsiento", asiento.estadoasiento)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spActualizarAsiento @asientoId, @vueloId, @numeroAsiento, @clase, @estadoAsiento", parameters);

            return NoContent();
        }

        // DELETE: api/asientos/5
        // Procedimiento almacenado: spEliminarAsiento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsiento(int id)
        {
            var param = new SqlParameter("@asientoId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spEliminarAsiento @asientoId", param);
            return NoContent();
        }
    }
}
