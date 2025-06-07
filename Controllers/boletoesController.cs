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
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<boleto>>> GetBoletos()
        {
            var boletos = await _context.boletos
                .FromSqlRaw("EXEC spObtenerBoletos")
                .ToListAsync();
            return Ok(boletos);
        }

        // GET: api/boletos/5
        // GET: api/boletos/searchbyid/5
        [HttpGet("searchbyid/{id}")]
        public async Task<ActionResult<boleto>> SearchById(int id)
        {
            var param = new SqlParameter("@boletoId", id);

            // Ejecuta el procedimiento y trae los resultados a memoria con ToListAsync()
            var boletos = await _context.boletos
                .FromSqlRaw("EXEC spObtenerBoletoPorId @boletoId", param)
                .ToListAsync();

            // Ahora seleccionamos el primero (o null)
            var boleto = boletos.FirstOrDefault();

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

            return CreatedAtAction(nameof(GetBoletos), new { id = boleto.boletoid }, boleto);
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
        // GET: api/boletos/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<boleto>>> BuscarBoletosAvanzado(
            [FromQuery] string? estadoBoleto = null,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var parameters = new[]
            {
        new SqlParameter("@estadoBoleto", estadoBoleto ?? (object)DBNull.Value),
        new SqlParameter("@fechaInicio", fechaInicio ?? (object)DBNull.Value),
        new SqlParameter("@fechaFin", fechaFin ?? (object)DBNull.Value),
        new SqlParameter("@pagina", pagina),
        new SqlParameter("@tamanioPagina", tamanioPagina)
    };

            var boletos = await _context.boletos
                .FromSqlRaw("EXEC spBuscarBoletosAvanzado @estadoBoleto, @fechaInicio, @fechaFin, @pagina, @tamanioPagina", parameters)
                .ToListAsync();

            return Ok(boletos);
        }
        // GET: api/boletos/searchbycodigo/{codigo}
        [HttpGet("searchbycodigo/{codigo}")]
        public async Task<ActionResult<IEnumerable<boleto>>> SearchByCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("El código no puede estar vacío.");

            try
            {
                var param = new SqlParameter("@codigoBoleto", codigo);

                var boletos = await _context.boletos
                    .FromSqlRaw("EXEC dbo.spObtenerBoletoPorCodigo @codigoBoleto", param)
                    .ToListAsync();

                if (boletos == null || boletos.Count == 0)
                    return NotFound();

                return Ok(boletos);
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si tienes un logger
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


    }
}
