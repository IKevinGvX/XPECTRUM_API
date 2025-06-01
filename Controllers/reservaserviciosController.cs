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
    public class reservaserviciosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public reservaserviciosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/reservaservicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<reservaservicio>>> Getreservaservicios()
        {
            var list = new List<reservaservicio>();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerReservasServicios";
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new reservaservicio
                {
                    reservaservicioid = reader.GetInt32(reader.GetOrdinal("reservaservicioid")),
                    reservaid = reader.GetInt32(reader.GetOrdinal("reservaid")),
                    servicioadicionalid = reader.GetInt32(reader.GetOrdinal("servicioid")),
                    cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                    precio = reader.GetDecimal(reader.GetOrdinal("precio"))
                });
            }

            await conn.CloseAsync();
            return Ok(list);
        }

        // GET: api/reservaservicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<reservaservicio>> Getreservaservicio(int id)
        {
            reservaservicio item = null;
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerReservaServicioPorId";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@reservaservicioid", id));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                item = new reservaservicio
                {
                    reservaservicioid = reader.GetInt32(reader.GetOrdinal("reservaservicioid")),
                    reservaid = reader.GetInt32(reader.GetOrdinal("reservaid")),
                    servicioadicionalid = reader.GetInt32(reader.GetOrdinal("servicioid")),
                    cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                    precio = reader.GetDecimal(reader.GetOrdinal("precio"))
                };
            }

            await conn.CloseAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/reservaservicios
        [HttpPost]
        public async Task<ActionResult<reservaservicio>> Postreservaservicio(reservaservicio item)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spInsertarReservaServicio";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@reservaid", item.reservaid));
            command.Parameters.Add(new SqlParameter("@servicioid", item.servicioadicionalid));
            command.Parameters.Add(new SqlParameter("@cantidad", item.cantidad));
            command.Parameters.Add(new SqlParameter("@precio", item.precio));

            var newIdParam = new SqlParameter("@NuevoReservaServicioId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(newIdParam);

            await command.ExecuteNonQueryAsync();

            item.reservaservicioid = (int)newIdParam.Value;

            await conn.CloseAsync();

            return CreatedAtAction(nameof(Getreservaservicio), new { id = item.reservaservicioid }, item);
        }

        // PUT: api/reservaservicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putreservaservicio(int id, reservaservicio item)
        {
            if (id != item.reservaservicioid)
                return BadRequest();

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spActualizarReservaServicio";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@reservaservicioid", item.reservaservicioid));
            command.Parameters.Add(new SqlParameter("@reservaid", item.reservaid));
            command.Parameters.Add(new SqlParameter("@servicioid", item.servicioadicionalid));
            command.Parameters.Add(new SqlParameter("@cantidad", item.cantidad));
            command.Parameters.Add(new SqlParameter("@precio", item.precio));

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                if (!await reservaservicioExists(id))
                    return NotFound();
                else
                    throw;
            }

            await conn.CloseAsync();

            return NoContent();
        }

        // DELETE: api/reservaservicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletereservaservicio(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spEliminarReservaServicio";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@reservaservicioid", id));

            await command.ExecuteNonQueryAsync();

            await conn.CloseAsync();

            return NoContent();
        }

        private async Task<bool> reservaservicioExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM ReservasServicios WHERE reservaservicioid = @reservaservicioid";
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@reservaservicioid", id));

            var result = await command.ExecuteScalarAsync();
            await conn.CloseAsync();

            return (int)result > 0;
        }
    }
}
