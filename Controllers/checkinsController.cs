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
    public class CheckInsController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public CheckInsController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/checkins
        // Procedimiento almacenado: spObtenerCheckIns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<checkin>>> GetCheckIns()
        {
            var result = new List<checkin>();
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "spObtenerCheckIns";
                command.CommandType = CommandType.StoredProcedure;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Add(new checkin
                    {
                        checkinid = reader.GetInt32(reader.GetOrdinal("checkinId")),
                        boletoid = reader.GetInt32(reader.GetOrdinal("boletoId")),
                        fechacheckin = reader.GetDateTime(reader.GetOrdinal("fechaCheckIn")),
                        metodocheckin = reader.GetString(reader.GetOrdinal("metodoCheckIn")),
                        tarjetaembarque = reader.GetString(reader.GetOrdinal("tarjetaEmbarque")),
                        estadocheckin = reader.GetString(reader.GetOrdinal("estadoCheckIn"))
                    });
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

            return Ok(result);
        }

        // GET: api/checkins/5
        // Procedimiento almacenado: spObtenerCheckInPorId
        [HttpGet("{id}")]
        public async Task<ActionResult<checkin>> GetCheckIn(int id)
        {
            checkin checkin = null;
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "spObtenerCheckInPorId";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@checkinId", id));

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    checkin = new checkin
                    {
                        checkinid = reader.GetInt32(reader.GetOrdinal("checkinId")),
                        boletoid = reader.GetInt32(reader.GetOrdinal("boletoId")),
                        fechacheckin = reader.GetDateTime(reader.GetOrdinal("fechaCheckIn")),
                        metodocheckin = reader.GetString(reader.GetOrdinal("metodoCheckIn")),
                        tarjetaembarque = reader.GetString(reader.GetOrdinal("tarjetaEmbarque")),
                        estadocheckin = reader.GetString(reader.GetOrdinal("estadoCheckIn"))
                    };
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

            if (checkin == null)
                return NotFound();

            return Ok(checkin);
        }

        // POST: api/checkins
        // Procedimiento almacenado: spInsertarCheckIn
        [HttpPost]
        public async Task<ActionResult<checkin>> PostCheckIn(checkin checkin)
        {
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "spInsertarCheckIn";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@boletoId", checkin.boletoid));
                command.Parameters.Add(new SqlParameter("@fechaCheckIn", checkin.fechacheckin));
                command.Parameters.Add(new SqlParameter("@metodoCheckIn", checkin.metodocheckin));
                command.Parameters.Add(new SqlParameter("@tarjetaEmbarque", checkin.tarjetaembarque));
                command.Parameters.Add(new SqlParameter("@estadoCheckIn", checkin.estadocheckin));

                var newIdParam = new SqlParameter("@NuevoCheckInId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(newIdParam);

                await command.ExecuteNonQueryAsync();

                checkin.checkinid = Convert.ToInt32(newIdParam.Value);
            }
            finally
            {
                await conn.CloseAsync();
            }

            return CreatedAtAction(nameof(GetCheckIn), new { id = checkin.checkinid }, checkin);
        }

        // PUT: api/checkins/5
        // Procedimiento almacenado: spActualizarCheckIn
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckIn(int id, checkin checkin)
        {
            if (id != checkin.checkinid)
            {
                return BadRequest();
            }

            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "spActualizarCheckIn";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@checkinId", checkin.checkinid));
                command.Parameters.Add(new SqlParameter("@boletoId", checkin.boletoid));
                command.Parameters.Add(new SqlParameter("@fechaCheckIn", checkin.fechacheckin));
                command.Parameters.Add(new SqlParameter("@metodoCheckIn", checkin.metodocheckin));
                command.Parameters.Add(new SqlParameter("@tarjetaEmbarque", checkin.tarjetaembarque));
                command.Parameters.Add(new SqlParameter("@estadoCheckIn", checkin.estadocheckin));

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                if (!await CheckInExists(id))
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

        // DELETE: api/checkins/5
        // Procedimiento almacenado: spEliminarCheckIn
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckIn(int id)
        {
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "spEliminarCheckIn";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@checkinId", id));

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }

            return NoContent();
        }

        private async Task<bool> CheckInExists(int id)
        {
            var conn = _context.Database.GetDbConnection();
            bool exists = false;

            try
            {
                await conn.OpenAsync();

                using var command = conn.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM CheckIns WHERE checkinId = @id";
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
