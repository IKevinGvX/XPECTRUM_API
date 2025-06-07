using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using xpectrum_api.data;
using xpectrum_api.models;
using System;
using Microsoft.Data.SqlClient;
using System.Drawing;

namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vueloesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public vueloesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/vueloes
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<vuelo>>> Getvuelos()
        {
            var vuelos = await _context.vuelos
                .FromSqlRaw("EXEC spBuscarVuelosAvanzado @origenId = NULL, @destinoId = NULL, @fechaSalidaInicio = NULL, @fechaSalidaFin = NULL, @estadoVuelo = NULL")
                .ToListAsync();

            return vuelos;
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<vuelo>> Getvuelo(int id)
        {
            var param = new SqlParameter("@vueloId", id);

            var vuelo = await _context.vuelos
                .FromSqlRaw("EXEC spBuscarVueloPorId @vueloId", param)
                .AsNoTracking()
                .FirstOrDefaultAsync();  // Trae un solo elemento (o null)

            if (vuelo == null)
            {
                return NotFound();
            }

            return vuelo;
        }



        // POST: api/vueloes
        [HttpPost("enviar")]
        public async Task<ActionResult<vuelo>> Postvuelo(vuelo vuelo)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC spInsertarVuelo @codigoVuelo = {0}, @origenId = {1}, @destinoId = {2}, @fechaSalida = {3}, @horaSalida = {4}, @fechaLlegada = {5}, @horaLlegada = {6}, @estadoVuelo = {7}, @aeronaveId = {8}",
                vuelo.codigovuelo,
                vuelo.origenid,
                vuelo.destinoid,
                vuelo.fechasalida,
                vuelo.horasalida,
                vuelo.fechallegada,
                vuelo.horallegada,
                vuelo.estadovuelo,
                vuelo.aeronaveid
            );

            return CreatedAtAction(nameof(Getvuelo), new { id = vuelo.vueloid }, vuelo);
        }

        // PUT: api/vueloes/5
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Putvuelo(int id, vuelo vuelo)
        {
            if (id != vuelo.vueloid)
            {
                return BadRequest();
            }

            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC spActualizarVuelo @vueloId = {0}, @codigoVuelo = {1}, @origenId = {2}, @destinoId = {3}, @fechaSalida = {4}, @horaSalida = {5}, @fechaLlegada = {6}, @horaLlegada = {7}, @estadoVuelo = {8}, @aeronaveId = {9}",
                id,
                vuelo.codigovuelo,
                vuelo.origenid,
                vuelo.destinoid,
                vuelo.fechasalida,
                vuelo.horasalida,
                vuelo.fechallegada,
                vuelo.horallegada,
                vuelo.estadovuelo,
                vuelo.aeronaveid
            );

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/vueloes/5
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Deletevuelo(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC spEliminarVuelo @vueloId = {0}", id);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // NUEVO: GET con filtros y paginación - api/vueloes/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<vuelo>>> BuscarVuelos(
            [FromQuery] int? origenId,
            [FromQuery] int? destinoId,
            [FromQuery] string fechaSalidaInicio,
            [FromQuery] string fechaSalidaFin,
            [FromQuery] string estadoVuelo,
            [FromQuery] string codigoVuelo,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamPagina = 10)
        {
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if (DateTime.TryParse(fechaSalidaInicio, out var fi))
                fechaInicio = fi;
            if (DateTime.TryParse(fechaSalidaFin, out var ff))
                fechaFin = ff;

            var vuelos = await _context.vuelos
                .FromSqlRaw("EXEC spBuscarVuelosAvanzado @origenId = {0}, @destinoId = {1}, @fechaSalidaInicio = {2}, @fechaSalidaFin = {3}, @estadoVuelo = {4}, @codigoVuelo = {5}, @pagina = {6}, @tamPagina = {7}",
                    origenId, destinoId, fechaInicio, fechaFin, estadoVuelo, codigoVuelo, pagina, tamPagina)
                .ToListAsync();

            return vuelos;
        }

        // NUEVO: GET total vuelos con filtros - api/vueloes/contar
        [HttpGet("contar")]
        public async Task<ActionResult<int>> ContarVuelos(
            [FromQuery] int? origenId,
            [FromQuery] int? destinoId,
            [FromQuery] string fechaSalidaInicio,
            [FromQuery] string fechaSalidaFin,
            [FromQuery] string estadoVuelo,
            [FromQuery] string codigoVuelo)
        {
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if (DateTime.TryParse(fechaSalidaInicio, out var fi))
                fechaInicio = fi;
            if (DateTime.TryParse(fechaSalidaFin, out var ff))
                fechaFin = ff;

            var total = await _context.Set<ContarVuelosResult>()
                .FromSqlRaw("EXEC spContarVuelosFiltros @origenId = {0}, @destinoId = {1}, @fechaSalidaInicio = {2}, @fechaSalidaFin = {3}, @estadoVuelo = {4}, @codigoVuelo = {5}",
                origenId, destinoId, fechaInicio, fechaFin, estadoVuelo, codigoVuelo)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (total == null)
                return 0;

            return total.TotalVuelos;
        }

        // Clase auxiliar para resultado del conteo
        private class ContarVuelosResult
        {
            public int TotalVuelos { get; set; }
        }
    
        [HttpGet("buscarPorCodigo/{codigoVuelo}")]
        public async Task<ActionResult<vuelo>> GetVueloPorCodigo(string codigoVuelo)
        {
            if (string.IsNullOrEmpty(codigoVuelo)) // Verifica si el parámetro es válido
            {
                return BadRequest("El código de vuelo es requerido.");
            }

            var param = new SqlParameter("@codigoVuelo", codigoVuelo);

            try
            {
                // Ejecuta el procedimiento almacenado y trae los resultados
                var vuelos = await _context.vuelos
                    .FromSqlRaw("EXEC spObtenerVueloPorCodigo @codigoVuelo", param)
                    .ToListAsync();  // Asegúrate de obtener la lista de resultados de manera asíncrona

                var vuelo = vuelos.FirstOrDefault();  // Luego filtra en memoria

                if (vuelo == null)
                {
                    return NotFound("No se encontró el vuelo con el código proporcionado.");
                }

                return vuelo;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones para capturar errores de ejecución
                return StatusCode(500, $"Error al ejecutar la consulta: {ex.Message}");
            }
        }
       


    }
}
