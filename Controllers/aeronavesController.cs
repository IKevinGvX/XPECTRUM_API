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
    public class VuelosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public VuelosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/Vuelos/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<vuelo>>> GetVuelos()
        {
            var vuelos = await _context.vuelos
                .FromSqlRaw("EXEC spObtenerVuelos")
                .ToListAsync();
            return Ok(vuelos);
        }

        // GET: api/Vuelos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vuelo>> GetVuelo(int id)
        {
            var param = new SqlParameter("@vueloId", id);
            var vuelo = await _context.vuelos
                .FromSqlRaw("EXEC spObtenerVueloPorId @vueloId", param)
                .FirstOrDefaultAsync();

            if (vuelo == null)
                return NotFound();

            return Ok(vuelo);
        }

        // POST: api/Vuelos
        [HttpPost]
        public async Task<ActionResult<vuelo>> PostVuelo(vuelo vuelo)
        {
            var parameters = new[]
            {
                new SqlParameter("@codigoVuelo", vuelo.codigovuelo),
                new SqlParameter("@origenId", vuelo.origenid),
                new SqlParameter("@destinoId", vuelo.destinoid),
                new SqlParameter("@fechaSalida", vuelo.fechasalida),
                new SqlParameter("@horaSalida", vuelo.horasalida),
                new SqlParameter("@fechaLlegada", vuelo.fechallegada),
                new SqlParameter("@horaLlegada", vuelo.horallegada),
                new SqlParameter("@estadoVuelo", vuelo.estadovuelo),
                new SqlParameter("@aeronaveId", vuelo.aeronaveid),
                new SqlParameter("@tipoViaje", vuelo.tipoviaje ?? (object)DBNull.Value),
                new SqlParameter("@clase", vuelo.clase ?? (object)DBNull.Value),
                new SqlParameter("@beneficio", vuelo.beneficio ?? (object)DBNull.Value),
                new SqlParameter("@precioUSD", vuelo.preciousd ?? (object)DBNull.Value),
                new SqlParameter("@precioPEN", vuelo.preciopen ?? (object)DBNull.Value),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spInsertarVuelo @codigoVuelo, @origenId, @destinoId, @fechaSalida, @horaSalida, @fechaLlegada, @horaLlegada, @estadoVuelo, @aeronaveId, @tipoViaje, @clase, @beneficio, @precioUSD, @precioPEN", parameters);

            // Aquí idealmente modificar el SP para devolver el nuevo ID y capturarlo

            return CreatedAtAction(nameof(GetVuelo), new { id = vuelo.vueloid }, vuelo);
        }

        // PUT: api/Vuelos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVuelo(int id, vuelo vuelo)
        {
            if (id != vuelo.vueloid)
                return BadRequest();

            var parameters = new[]
            {
                new SqlParameter("@vueloId", vuelo.vueloid),
                new SqlParameter("@codigoVuelo", vuelo.codigovuelo),
                new SqlParameter("@origenId", vuelo.origenid),
                new SqlParameter("@destinoId", vuelo.destinoid),
                new SqlParameter("@fechaSalida", vuelo.fechasalida),
                new SqlParameter("@horaSalida", vuelo.horasalida),
                new SqlParameter("@fechaLlegada", vuelo.fechallegada),
                new SqlParameter("@horaLlegada", vuelo.horallegada),
                new SqlParameter("@estadoVuelo", vuelo.estadovuelo),
                new SqlParameter("@aeronaveId", vuelo.aeronaveid),
                new SqlParameter("@tipoViaje", vuelo.tipoviaje ?? (object)DBNull.Value),
                new SqlParameter("@clase", vuelo.clase ?? (object)DBNull.Value),
                new SqlParameter("@beneficio", vuelo.beneficio ?? (object)DBNull.Value),
                new SqlParameter("@precioUSD", vuelo.preciousd ?? (object)DBNull.Value),
                new SqlParameter("@precioPEN", vuelo.preciopen ?? (object)DBNull.Value),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spActualizarVuelo @vueloId, @codigoVuelo, @origenId, @destinoId, @fechaSalida, @horaSalida, @fechaLlegada, @horaLlegada, @estadoVuelo, @aeronaveId, @tipoViaje, @clase, @beneficio, @precioUSD, @precioPEN", parameters);

            return NoContent();
        }

        // DELETE: api/Vuelos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVuelo(int id)
        {
            var param = new SqlParameter("@vueloId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spEliminarVuelo @vueloId", param);
            return NoContent();
        }

        // GET: api/Vuelos/search/{query}
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<vuelo>>> SearchVuelos(string query)
        {
            var param = new SqlParameter("@textoBusqueda", query);
            var result = await _context.vuelos
                .FromSqlRaw("EXEC spBuscarVuelos @textoBusqueda", param)
                .ToListAsync();
            return Ok(result);
        }
    }
}
