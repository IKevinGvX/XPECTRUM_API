using System;
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
    public class ConfiguracionSistemaController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public ConfiguracionSistemaController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/configuracionsistema
        [HttpGet]
        public async Task<ActionResult<IEnumerable<configuracionsistema>>> GetConfiguraciones()
        {
            var result = new List<configuracionsistema>();

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "spObtenerConfiguracionesSistema";
            command.CommandType = CommandType.StoredProcedure;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new configuracionsistema
                {
                    configuracionsistemaid = reader.GetInt32(reader.GetOrdinal("configuracionSistemaId")),
                    clave = reader.GetString(reader.GetOrdinal("clave")),
                    valor = reader.GetString(reader.GetOrdinal("valor"))
                });
            }

            return Ok(result);
        }

        // GET: api/configuracionsistema/5
        [HttpGet("{id}")]
        public async Task<ActionResult<configuracionsistema>> GetConfiguracion(int id)
        {
            configuracionsistema config = null;

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "spObtenerConfiguracionSistemaPorId";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@configuracionSistemaId", id));

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                config = new configuracionsistema
                {
                    configuracionsistemaid = reader.GetInt32(reader.GetOrdinal("configuracionSistemaId")),
                    clave = reader.GetString(reader.GetOrdinal("clave")),
                    valor = reader.GetString(reader.GetOrdinal("valor"))
                };
            }

            if (config == null)
                return NotFound();

            return Ok(config);
        }

        // POST: api/configuracionsistema
        [HttpPost]
        public async Task<ActionResult<configuracionsistema>> PostConfiguracion(configuracionsistema config)
        {
            if (string.IsNullOrWhiteSpace(config.clave) || string.IsNullOrWhiteSpace(config.valor))
                return BadRequest("Clave y Valor son obligatorios.");

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "spInsertarConfiguracionSistema";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@clave", config.clave));
            command.Parameters.Add(new SqlParameter("@valor", config.valor));

            var newIdParam = new SqlParameter("@NuevoConfiguracionSistemaId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(newIdParam);

            await command.ExecuteNonQueryAsync();

            config.configuracionsistemaid = Convert.ToInt32(newIdParam.Value);

            return CreatedAtAction(nameof(GetConfiguracion), new { id = config.configuracionsistemaid }, config);
        }

        // PUT: api/configuracionsistema/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfiguracion(int id, configuracionsistema config)
        {
            if (id != config.configuracionsistemaid)
                return BadRequest("El ID del recurso no coincide con el de la URL.");

            if (string.IsNullOrWhiteSpace(config.clave) || string.IsNullOrWhiteSpace(config.valor))
                return BadRequest("Clave y Valor son obligatorios.");

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "spActualizarConfiguracionSistema";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@configuracionSistemaId", config.configuracionsistemaid));
            command.Parameters.Add(new SqlParameter("@clave", config.clave));
            command.Parameters.Add(new SqlParameter("@valor", config.valor));

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                if (!await ConfiguracionExists(id))
                    return NotFound();

                // Puedes agregar más manejo de errores según código ex.Number
                throw new Exception("Error al actualizar la configuración del sistema.", ex);
            }

            return NoContent();
        }

        // DELETE: api/configuracionsistema/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguracion(int id)
        {
            if (!await ConfiguracionExists(id))
                return NotFound();

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "spEliminarConfiguracionSistema";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@configuracionSistemaId", id));

            await command.ExecuteNonQueryAsync();

            return NoContent();
        }

        private async Task<bool> ConfiguracionExists(int id)
        {
            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM ConfiguracionSistema WHERE configuracionSistemaId = @id";
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@id", id));

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
    }
}
