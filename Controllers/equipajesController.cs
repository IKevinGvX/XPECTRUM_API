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
    public class EquipajesController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public EquipajesController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/equipajes
        // Procedimiento almacenado: spObtenerEquipajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<equipaje>>> GetEquipajes()
        {
            var result = new List<equipaje>();
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "spObtenerEquipajes";
                command.CommandType = CommandType.StoredProcedure;

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new equipaje
                    {
                        equipajeid = reader.GetInt32(reader.GetOrdinal("equipajeId")),
                        resid = reader.GetInt32(reader.GetOrdinal("reservaId")),
                        peso = reader.GetDecimal(reader.GetOrdinal("peso")),
                        tipoequipaje = reader.GetString(reader.GetOrdinal("tipoEquipaje")),
                        estadoequipaje = reader.GetString(reader.GetOrdinal("estadoEquipaje"))
                    });
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return Ok(result);
        }

        // GET: api/equipajes/5
        // Procedimiento almacenado: spObtenerEquipajePorId
        [HttpGet("{id}")]
        public async Task<ActionResult<equipaje>> GetEquipaje(int id)
        {
            equipaje equipaje = null;
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "spObtenerEquipajePorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@equipajeId", id));

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    equipaje = new equipaje
                    {
                        equipajeid = reader.GetInt32(reader.GetOrdinal("equipajeId")),
                        resid = reader.GetInt32(reader.GetOrdinal("reserva")),
                        peso = reader.GetDecimal(reader.GetOrdinal("peso")),
                        tipoequipaje = reader.GetString(reader.GetOrdinal("tipoEquipaje")),
                        estadoequipaje  = reader.GetString(reader.GetOrdinal("estadoEquipaje"))
                    };
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

            if (equipaje == null)
                return NotFound();

            return Ok(equipaje);
        }

        // POST: api/equipajes
        // Procedimiento almacenado: spInsertarEquipaje
        [HttpPost]
        public async Task<ActionResult<equipaje>> PostEquipaje(equipaje equipaje)
        {
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "spInsertarEquipaje";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@reservaId", equipaje.resid));
                command.Parameters.Add(new SqlParameter("@peso", equipaje.peso));
                command.Parameters.Add(new SqlParameter("@tipoEquipaje", equipaje.tipoequipaje));
                command.Parameters.Add(new SqlParameter("@estadoEquipaje", equipaje.estadoequipaje));

                var newIdParam = new SqlParameter("@NuevoEquipajeId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(newIdParam);

                await command.ExecuteNonQueryAsync();

                equipaje.equipajeid = Convert.ToInt32(newIdParam.Value);
            }
            finally
            {
                await conn.CloseAsync();
            }

            return CreatedAtAction(nameof(GetEquipaje), new { id = equipaje.equipajeid }, equipaje);
        }

        // PUT: api/equipajes/5
        // Procedimiento almacenado: spActualizarEquipaje
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipaje(int id, equipaje equipaje)
        {
            if (id != equipaje.equipajeid)
            {
                return BadRequest();
            }

            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "spActualizarEquipaje";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@equipajeId", equipaje.equipajeid));
                command.Parameters.Add(new SqlParameter("@reservaId", equipaje.resid));
                command.Parameters.Add(new SqlParameter("@peso", equipaje.peso));
                command.Parameters.Add(new SqlParameter("@tipoEquipaje", equipaje.tipoequipaje));
                command.Parameters.Add(new SqlParameter("@estadoEquipaje", equipaje.estadoequipaje));

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                if (!await EquipajeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

            return NoContent();
        }

        // DELETE: api/equipajes/5
        // Procedimiento almacenado: spEliminarEquipaje
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipaje(int id)
        {
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "spEliminarEquipaje";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@equipajeId", id));

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }

            return NoContent();
        }

        private async Task<bool> EquipajeExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            bool exists = false;

            try
            {
                await conn.OpenAsync();
                using var command = conn.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Equipajes WHERE equipajeId = @id";
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@id", id));

                var result = await command.ExecuteScalarAsync();
                exists = Convert.ToInt32(result) > 0;
            }
            finally
            {
                await conn.CloseAsync();
            }

            return exists;
        }
    }
}
