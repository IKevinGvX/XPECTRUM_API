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
    public class RolesPermisosController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public RolesPermisosController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/rolespermisos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<rolpermiso>>> GetRolesPermisos()
        {
            var list = new List<rolpermiso>();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerRolesPermisos";
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new rolpermiso
                {
                    rolpermisoid = reader.GetInt32(reader.GetOrdinal("rolPermisoId")),
                    rolid = reader.GetInt32(reader.GetOrdinal("rolid")),
                    permisocodigo = reader.GetString(reader.GetOrdinal("permisoCodigo"))
                });
            }

            await conn.CloseAsync();
            return Ok(list);
        }

        // GET: api/rolespermisos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<rolpermiso>> GetRolPermiso(int id)
        {
            rolpermiso item = null;
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spObtenerRolPermisoPorId";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@rolPermisoId", id));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                item = new rolpermiso
                {
                    rolpermisoid = reader.GetInt32(reader.GetOrdinal("rolPermisoId")),
                    rolid = reader.GetInt32(reader.GetOrdinal("rolid")),
                    permisocodigo = reader.GetString(reader.GetOrdinal("permisoCodigo"))
                };
            }

            await conn.CloseAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/rolespermisos
        [HttpPost]
        public async Task<ActionResult<rolpermiso>> PostRolPermiso(rolpermiso item)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spInsertarRolPermiso";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@rolid", item.rolid));
            command.Parameters.Add(new SqlParameter("@permisoCodigo", item.permisocodigo));

            var newIdParam = new SqlParameter("@NuevoRolPermisoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(newIdParam);

            await command.ExecuteNonQueryAsync();

            item.rolpermisoid = (int)newIdParam.Value;

            await conn.CloseAsync();

            return CreatedAtAction(nameof(GetRolPermiso), new { id = item.rolpermisoid }, item);
        }

        // PUT: api/rolespermisos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolPermiso(int id, rolpermiso item)
        {
            if (id != item.rolpermisoid)
                return BadRequest();

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spActualizarRolPermiso";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@rolPermisoId", item.rolpermisoid));
            command.Parameters.Add(new SqlParameter("@rolid", item.rolid));
            command.Parameters.Add(new SqlParameter("@permisoCodigo", item.permisocodigo));

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                if (!await RolPermisoExists(id))
                    return NotFound();
                else
                    throw;
            }

            await conn.CloseAsync();

            return NoContent();
        }

        // DELETE: api/rolespermisos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolPermiso(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "spEliminarRolPermiso";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@rolPermisoId", id));

            await command.ExecuteNonQueryAsync();

            await conn.CloseAsync();

            return NoContent();
        }

        private async Task<bool> RolPermisoExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            bool exists;
            await using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM RolesPermisos WHERE rolPermisoId = @rolPermisoId";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@rolPermisoId", id));

                var result = await command.ExecuteScalarAsync();
                exists = (int)result > 0;
            }

            await conn.CloseAsync();

            return exists;
        }
    }
}
