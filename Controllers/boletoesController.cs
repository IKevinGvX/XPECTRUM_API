using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;
using xpectrum_api.models;


namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoletosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public BoletosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/boletos
        // Usa el procedimiento almacenado spObtenerBoletos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<boleto>>> GetBoletos()
        {
            var boletos = await _context.boletos
                .FromSqlRaw("EXEC spObtenerBoletos")
                .ToListAsync();
            return Ok(boletos);
        }

        // GET: api/boletos/5
        // Usa el procedimiento almacenado spObtenerBoletoPorId
        [HttpGet("{id}")]
        public async Task<ActionResult<boleto>> GetBoleto(int id)
        {
            var param = new SqlParameter("@boletoId", id);
            var boleto = await _context.boletos
                .FromSqlRaw("EXEC spObtenerBoletoPorId @boletoId", param)
                .FirstOrDefaultAsync();

            if (boleto == null)
                return NotFound();

            return Ok(boleto);
        }

        // POST: api/boletos
        // Usa el procedimiento almacenado spInsertarBoleto
        [HttpPost]
        public async Task<ActionResult<boleto>> PostBoleto(boleto boleto)
        {
            var parameters = new[]
            {
                new SqlParameter("@reservaId", boleto.reservaid),
                new SqlParameter("@codigoBoleto", boleto.codigoboleto),
                new SqlParameter("@fechaEmision", boleto.fechaemision),
                new SqlParameter("@estadoBoleto", boleto.estadoboleto)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC spInsertarBoleto @reservaId, @codigoBoleto, @fechaEmision, @estadoBoleto",
                parameters);

            // Opcional: Si el SP retorna el nuevo id, capturarlo y asignar aquí para devolver CreatedAtAction correcto.

            return CreatedAtAction(nameof(GetBoleto), new { id = boleto.boletoid }, boleto);
        }

        // PUT: api/boletos/5
        // Usa el procedimiento almacenado spActualizarBoleto
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoleto(int id, boleto boleto)
        {
            if (id != boleto.boletoid)
                return BadRequest();

            var parameters = new[]
            {
                new SqlParameter("@boletoId", boleto.boletoid),
                new SqlParameter("@reservaId", boleto.reservaid),
                new SqlParameter("@codigoBoleto", boleto.codigoboleto),
                new SqlParameter("@fechaEmision", boleto.fechaemision),
                new SqlParameter("@estadoBoleto", boleto.estadoboleto)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC spActualizarBoleto @boletoId, @reservaId, @codigoBoleto, @fechaEmision, @estadoBoleto",
                parameters);

            return NoContent();
        }

        // DELETE: api/boletos/5
        // Usa el procedimiento almacenado spEliminarBoleto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoleto(int id)
        {
            var param = new SqlParameter("@boletoId", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC spEliminarBoleto @boletoId", param);
            return NoContent();
        }
    }
}
