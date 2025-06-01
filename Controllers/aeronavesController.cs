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
    public class AeronavesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public AeronavesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/Aeronaves
        // Procedimiento almacenado: spObtenerAeronaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<aeronave>>> GetAeronaves()
        {
            var aeronaves = await _context.aeronaves
                .FromSqlRaw("EXEC spObtenerAeronaves")
                .ToListAsync();
            return Ok(aeronaves);
        }

        // GET: api/Aeronaves/5
        // Procedimiento almacenado: spObtenerAeronavePorId
        [HttpGet("{id}")]
        public async Task<ActionResult<aeronave>> GetAeronave(int id)
        {
            var param = new SqlParameter("@aeronaveId", id);
            var aeronave = await _context.aeronaves
                .FromSqlRaw("EXEC spObtenerAeronavePorId @aeronaveId", param)
                .FirstOrDefaultAsync();

            if (aeronave == null)
                return NotFound();

            return Ok(aeronave);
        }

        // POST: api/Aeronaves
        // Procedimiento almacenado: spInsertarAeronave
        [HttpPost]
        public async Task<ActionResult<aeronave>> PostAeronave(aeronave aeronave)
        {
            var parameters = new[]
            {
                new SqlParameter("@modelo", aeronave.modelo),
                new SqlParameter("@capacidad", aeronave.capacidad),
                new SqlParameter("@matricula", aeronave.matricula),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spInsertarAeronave @modelo, @capacidad, @matricula", parameters);

            // Nota: Para obtener el id recién insertado se puede modificar el SP para devolverlo y capturarlo aquí.

            return CreatedAtAction(nameof(GetAeronave), new { id = aeronave.aeronaveid }, aeronave);
        }

        // PUT: api/Aeronaves/5
        // Procedimiento almacenado: spActualizarAeronave
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAeronave(int id, aeronave aeronave)
        {
            if (id != aeronave.aeronaveid)
                return BadRequest();

            var parameters = new[]
            {
                new SqlParameter("@aeronaveId", aeronave.aeronaveid),
                new SqlParameter("@modelo", aeronave.modelo),
                new SqlParameter("@capacidad", aeronave.capacidad),
                new SqlParameter("@matricula", aeronave.matricula),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC spActualizarAeronave @aeronaveId, @modelo, @capacidad, @matricula", parameters);

            return NoContent();
        }

        // DELETE: api/Aeronaves/5
        // Procedimiento almacenado: spEliminarAeronave
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAeronave(int id)
        {
            var param = new SqlParameter("@aeronaveId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spEliminarAeronave @aeronaveId", param);
            return NoContent();
        }

        // GET: api/Aeronaves/search/{query}
        // Procedimiento almacenado: spBuscarAeronaves
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<aeronave>>> SearchAeronaves(string query)
        {
            var param = new SqlParameter("@textoBusqueda", query);
            var result = await _context.aeronaves
                .FromSqlRaw("EXEC spBuscarAeronaves @textoBusqueda", param)
                .ToListAsync();
            return Ok(result);
        }
        // GET: api/Aeronaves/capacity/total
        // Procedimiento almacenado: spObtenerCapacidadTotal
        [HttpGet("capacity/total")]
        public async Task<ActionResult<int>> GetCapacidadTotal()
        {
            int capacidadTotal = 0;

            // Abrir la conexión
            await _context.Database.OpenConnectionAsync();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "spObtenerCapacidadTotal";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var result = await command.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int valor))
                    {
                        capacidadTotal = valor;
                    }
                }
            }
            finally
            {
                // Cerrar la conexión
                await _context.Database.CloseConnectionAsync();
            }

            return Ok(capacidadTotal);
        }

    }
}
