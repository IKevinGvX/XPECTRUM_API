using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;
using xpectrum_api.models;

namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public reservasController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<reserva>>> Getreservas()
        {
            var reservas = new List<reserva>();
            var conn = _context.Database.GetDbConnection();

            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spObtenerReservas";
                command.CommandType = CommandType.StoredProcedure;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    reservas.Add(new reserva
                    {
                        reservaId = reader.GetInt32(reader.GetOrdinal("reservaId")),
                        usuarioId = reader.GetInt32(reader.GetOrdinal("usuarioId")),
                        vueloId = reader.GetInt32(reader.GetOrdinal("vueloId")),
                        fechaReserva = reader.GetDateTime(reader.GetOrdinal("fechaReserva")),
                        estadoReserva = reader.GetString(reader.GetOrdinal("estadoReserva")),
                        totalPago = reader.GetDecimal(reader.GetOrdinal("totalPago"))
                    });
                }
            }

            await conn.CloseAsync();
            return Ok(reservas);
        }

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<reserva>> Getreserva(int id)
        {
            reserva reserva = null;
            var conn = _context.Database.GetDbConnection();

            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spObtenerReservaPorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@reservaId", id));

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    reserva = new reserva
                    {
                        reservaId = reader.GetInt32(reader.GetOrdinal("reservaId")),
                        usuarioId = reader.GetInt32(reader.GetOrdinal("usuarioId")),
                        vueloId = reader.GetInt32(reader.GetOrdinal("vueloId")),
                        fechaReserva = reader.GetDateTime(reader.GetOrdinal("fechaReserva")),
                        estadoReserva = reader.GetString(reader.GetOrdinal("estadoReserva")),
                        totalPago = reader.GetDecimal(reader.GetOrdinal("totalPago"))
                    };
                }
            }

            await conn.CloseAsync();

            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }

        // POST: api/reservas
        [HttpPost]
        public async Task<ActionResult<reserva>> Postreserva(reserva reserva)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spInsertarReserva";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@usuarioId", reserva.usuarioId));
                command.Parameters.Add(new SqlParameter("@vueloId", reserva.vueloId));
                command.Parameters.Add(new SqlParameter("@fechaReserva", reserva.fechaReserva));
                command.Parameters.Add(new SqlParameter("@estadoReserva", reserva.estadoReserva));
                command.Parameters.Add(new SqlParameter("@totalPago", reserva.totalPago));

                var newIdParam = new SqlParameter("@NuevoReservaId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(newIdParam);

                await command.ExecuteNonQueryAsync();

                reserva.reservaId = (int)newIdParam.Value;
            }

            await conn.CloseAsync();

            return CreatedAtAction(nameof(Getreserva), new { id = reserva.reservaId }, reserva);
        }

        // PUT: api/reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putreserva(int id, reserva reserva)
        {
            if (id != reserva.reservaId)
            {
                return BadRequest();
            }

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spActualizarReserva";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@reservaId", reserva.reservaId));
                command.Parameters.Add(new SqlParameter("@usuarioId", reserva.usuarioId));
                command.Parameters.Add(new SqlParameter("@vueloId", reserva.vueloId));
                command.Parameters.Add(new SqlParameter("@fechaReserva", reserva.fechaReserva));
                command.Parameters.Add(new SqlParameter("@estadoReserva", reserva.estadoReserva));
                command.Parameters.Add(new SqlParameter("@totalPago", reserva.totalPago));

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    if (!await reservaExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }

            await conn.CloseAsync();

            return NoContent();
        }

        // DELETE: api/reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletereserva(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spEliminarReserva";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@reservaId", id));

                await command.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();

            return NoContent();
        }

        private async Task<bool> reservaExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            bool exists;
            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM Reservas WHERE reservaId = @reservaId";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@reservaId", id));

                var result = await command.ExecuteScalarAsync();
                exists = (int)result > 0;
            }

            await conn.CloseAsync();

            return exists;
        }
    }
}
