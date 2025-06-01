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
    public class AeropuertosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public AeropuertosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/aeropuertos
        // Procedimiento almacenado: spObtenerAeropuertos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<aeropuerto>>> GetAeropuertos()
        {
            var aeropuertos = await _context.aeropuertos
                .FromSqlRaw("EXEC spObtenerAeropuertos")
                .ToListAsync();
            return Ok(aeropuertos);
        }

        // GET: api/aeropuertos/5
        // Procedimiento almacenado: spObtenerAeropuertoPorId
        [HttpGet("{id}")]
        public async Task<ActionResult<aeropuerto>> GetAeropuerto(int id)
        {
            var param = new SqlParameter("@aeropuertoId", id);
            var aeropuerto = await _context.aeropuertos
                .FromSqlRaw("EXEC spObtenerAeropuertoPorId @aeropuertoId", param)
                .FirstOrDefaultAsync();

            if (aeropuerto == null)
                return NotFound();

            return Ok(aeropuerto);
        }

        // POST: api/aeropuertos
        // Procedimiento almacenado: spInsertarAeropuerto
        [HttpPost]
        public async Task<ActionResult<aeropuerto>> PostAeropuerto(aeropuerto aeropuerto)
        {
            var parameters = new[]
            {
                new SqlParameter("@codigoIATA", aeropuerto.codigoiata),
                new SqlParameter("@nombre", aeropuerto.nombre),
                new SqlParameter("@ciudad", aeropuerto.ciudad),
                new SqlParameter("@pais", aeropuerto.pais)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spInsertarAeropuerto @codigoIATA, @nombre, @ciudad, @pais", parameters);

            // Nota: Si quieres devolver el Id recién creado, el SP debe retornar el nuevo Id y capturarlo aquí.

            return CreatedAtAction(nameof(GetAeropuerto), new { id = aeropuerto.aeropuertoid }, aeropuerto);
        }

        // PUT: api/aeropuertos/5
        // Procedimiento almacenado: spActualizarAeropuerto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAeropuerto(int id, aeropuerto aeropuerto)
        {
            if (id != aeropuerto.aeropuertoid)
                return BadRequest();

            var parameters = new[]
            {
                new SqlParameter("@aeropuertoId", aeropuerto.aeropuertoid),
                new SqlParameter("@codigoIATA", aeropuerto.codigoiata),
                new SqlParameter("@nombre", aeropuerto.nombre),
                new SqlParameter("@ciudad", aeropuerto.ciudad),
                new SqlParameter("@pais", aeropuerto.pais)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spActualizarAeropuerto @aeropuertoId, @codigoIATA, @nombre, @ciudad, @pais", parameters);

            return NoContent();
        }

        // DELETE: api/aeropuertos/5
        // Procedimiento almacenado: spEliminarAeropuerto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAeropuerto(int id)
        {
            var param = new SqlParameter("@aeropuertoId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spEliminarAeropuerto @aeropuertoId", param);
            return NoContent();
        }

        // GET: api/aeropuertos/search/{query}
        // Procedimiento almacenado: spBuscarAeropuertos
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<aeropuerto>>> SearchAeropuertos(string query)
        {
            var param = new SqlParameter("@textoBusqueda", query);
            var result = await _context.aeropuertos
                .FromSqlRaw("EXEC spBuscarAeropuertos @textoBusqueda", param)
                .ToListAsync();
            return Ok(result);
        }
    }
}
