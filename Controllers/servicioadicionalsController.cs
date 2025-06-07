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
    public class ServiciosAdicionalesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public ServiciosAdicionalesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/ServiciosAdicionales
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<servicioadicional>>> GetServiciosAdicionales()
        {
            var list = new List<servicioadicional>();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerServiciosAdicionales";
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new servicioadicional
                {
                    servicioadicionalid = reader.GetInt32(reader.GetOrdinal("servicioAdicionalId")),
                    nombreservicio = reader.GetString(reader.GetOrdinal("nombreServicio")),
                    descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
                    precio = reader.GetDecimal(reader.GetOrdinal("precio"))
                });
            }

            await conn.CloseAsync();
            return Ok(list);
        }

        // GET: api/ServiciosAdicionales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<servicioadicional>> GetServicioAdicional(int id)
        {
            servicioadicional item = null;
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerServicioAdicionalPorId";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@servicioAdicionalId", id));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                item = new servicioadicional
                {
                    servicioadicionalid = reader.GetInt32(reader.GetOrdinal("servicioAdicionalId")),
                    nombreservicio = reader.GetString(reader.GetOrdinal("nombreServicio")),
                    descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
                    precio = reader.GetDecimal(reader.GetOrdinal("precio"))
                };
            }

            await conn.CloseAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/ServiciosAdicionales
        [HttpPost]
        public async Task<ActionResult<servicioadicional>> PostServicioAdicional(servicioadicional item)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spInsertarServicioAdicional";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@nombreServicio", item.nombreservicio));
            command.Parameters.Add(new SqlParameter("@descripcion", (object)item.descripcion ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@precio", item.precio));

            var newIdParam = new SqlParameter("@NuevoServicioAdicionalId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(newIdParam);

            await command.ExecuteNonQueryAsync();

            item.servicioadicionalid = (int)newIdParam.Value;

            await conn.CloseAsync();

            return CreatedAtAction(nameof(GetServicioAdicional), new { id = item.servicioadicionalid }, item);
        }

        // PUT: api/ServiciosAdicionales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicioAdicional(int id, servicioadicional item)
        {
            if (id != item.servicioadicionalid)
                return BadRequest();

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spActualizarServicioAdicional";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@servicioAdicionalId", item.servicioadicionalid));
            command.Parameters.Add(new SqlParameter("@nombreServicio", item.nombreservicio));
            command.Parameters.Add(new SqlParameter("@descripcion", (object)item.descripcion ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@precio", item.precio));

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                if (!await ServicioAdicionalExists(id))
                    return NotFound();
                else
                    throw;
            }

            await conn.CloseAsync();

            return NoContent();
        }

        // DELETE: api/ServiciosAdicionales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicioAdicional(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spEliminarServicioAdicional";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@servicioAdicionalId", id));

            await command.ExecuteNonQueryAsync();

            await conn.CloseAsync();

            return NoContent();
        }

        private async Task<bool> ServicioAdicionalExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            bool exists;
            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM ServiciosAdicionales WHERE servicioAdicionalId = @servicioAdicionalId";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@servicioAdicionalId", id));

                var result = await command.ExecuteScalarAsync();
                exists = (int)result > 0;
            }

            await conn.CloseAsync();

            return exists;
        }
    }
}
