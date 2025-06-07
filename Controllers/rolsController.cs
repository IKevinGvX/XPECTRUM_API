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
    public class RolesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public RolesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/roles
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<rol>>> GetRoles()
        {
            var roles = new List<rol>();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spObtenerRoles";
                command.CommandType = CommandType.StoredProcedure;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    roles.Add(new rol
                    {
                        rolid = reader.GetInt32(reader.GetOrdinal("rolId")),
                        nombrerol = reader.GetString(reader.GetOrdinal("nombreRol")),
                        descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
                    });
                }
            }

            await conn.CloseAsync();
            return Ok(roles);
        }

        // GET: api/roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<rol>> GetRol(int id)
        {
            rol rol = null;
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spObtenerRolPorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@rolId", id));

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    rol = new rol
                    {
                        rolid = reader.GetInt32(reader.GetOrdinal("rolId")),
                        nombrerol = reader.GetString(reader.GetOrdinal("nombreRol")),
                        descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
                    };
                }
            }

            await conn.CloseAsync();

            if (rol == null)
                return NotFound();

            return Ok(rol);
        }

        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult<rol>> PostRol(rol rol)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spInsertarRol";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nombreRol", rol.nombrerol));
                command.Parameters.Add(new SqlParameter("@descripcion", (object)rol.descripcion ?? DBNull.Value));

                var newIdParam = new SqlParameter("@NuevoRolId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(newIdParam);

                await command.ExecuteNonQueryAsync();

                rol.rolid = (int)newIdParam.Value;
            }

            await conn.CloseAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.rolid }, rol);
        }

        // PUT: api/roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, rol rol)
        {
            if (id != rol.rolid)
                return BadRequest();

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spActualizarRol";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@rollId", rol.rolid));
                command.Parameters.Add(new SqlParameter("@nombreRol", rol.nombrerol));
                command.Parameters.Add(new SqlParameter("@descripcion", (object)rol.descripcion ?? DBNull.Value));

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    if (!await RolExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }

            await conn.CloseAsync();

            return NoContent();
        }

        // DELETE: api/roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "spEliminarRol";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@rollId", id));

                await command.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();

            return NoContent();
        }

        private async Task<bool> RolExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            bool exists;
            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM Roles WHERE rollId = @rollId";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@rollId", id));

                var result = await command.ExecuteScalarAsync();
                exists = (int)result > 0;
            }

            await conn.CloseAsync();

            return exists;
        }
    }
}
