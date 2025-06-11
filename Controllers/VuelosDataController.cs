using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpectrumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VuelosController : ControllerBase
    {
        private readonly string _connectionString = "Server=xpectrum.mssql.somee.com;Database=xpectrum;User Id=KKevinyouman2004_SQLLogin_2;Password=Kevinyouman2004;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True;Connection Timeout=30"; // Tu cadena de conexión a SQL Server

        [HttpGet("GetVuelos")]
        public async Task<IActionResult> GetVuelos()
        {
            var vuelos = new List<VUELODATA>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.ListarVuelos", connection)) // Procedimiento almacenado para todos los vuelos
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vuelos.Add(new VUELODATA
                            {
                                VueloId = reader.IsDBNull(reader.GetOrdinal("vueloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("vueloId")),
                                CodigoVuelo = reader.IsDBNull(reader.GetOrdinal("codigoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoVuelo")),
                                OrigenId = reader.IsDBNull(reader.GetOrdinal("origenId")) ? 0 : reader.GetInt32(reader.GetOrdinal("origenId")),
                                DestinoId = reader.IsDBNull(reader.GetOrdinal("destinoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("destinoId")),
                                FechaSalida = reader.IsDBNull(reader.GetOrdinal("fechaSalida")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fechaSalida")),
                                HoraSalida = reader.IsDBNull(reader.GetOrdinal("horaSalida")) ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("horaSalida")),
                                FechaLlegada = reader.IsDBNull(reader.GetOrdinal("fechaLlegada")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fechaLlegada")),
                                HoraLlegada = reader.IsDBNull(reader.GetOrdinal("horaLlegada")) ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("horaLlegada")),
                                EstadoVuelo = reader.IsDBNull(reader.GetOrdinal("estadoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVuelo")),
                                AeronaveId = reader.IsDBNull(reader.GetOrdinal("aeronaveId")) ? 0 : reader.GetInt32(reader.GetOrdinal("aeronaveId")),
                                Aerolinea = reader.IsDBNull(reader.GetOrdinal("aerolinea")) ? string.Empty : reader.GetString(reader.GetOrdinal("aerolinea")),
                                CodigoAerolinea = reader.IsDBNull(reader.GetOrdinal("codigoAerolinea")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoAerolinea")),
                                TripulacionCapitan = reader.IsDBNull(reader.GetOrdinal("tripulacionCapitan")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionCapitan")),
                                TripulacionCopiloto = reader.IsDBNull(reader.GetOrdinal("tripulacionCopiloto")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionCopiloto")),
                                TripulacionAuxiliares = reader.IsDBNull(reader.GetOrdinal("tripulacionAuxiliares")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionAuxiliares")),
                                EscalaId = reader.IsDBNull(reader.GetOrdinal("escalaId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("escalaId")),
                                DuracionEscala = reader.IsDBNull(reader.GetOrdinal("duracionEscala")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("duracionEscala")),
                                ServiciosAdicionales = reader.IsDBNull(reader.GetOrdinal("serviciosAdicionales")) ? string.Empty : reader.GetString(reader.GetOrdinal("serviciosAdicionales")),
                                TarifaEspecial = reader.IsDBNull(reader.GetOrdinal("tarifaEspecial")) ? 0 : reader.GetDecimal(reader.GetOrdinal("tarifaEspecial")),
                                Moneda = reader.IsDBNull(reader.GetOrdinal("moneda")) ? string.Empty : reader.GetString(reader.GetOrdinal("moneda")),
                                CondicionesAdicionales = reader.IsDBNull(reader.GetOrdinal("condicionesAdicionales")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionesAdicionales")),
                                PrecioUSD = reader.IsDBNull(reader.GetOrdinal("precioUSD")) ? 0 : reader.GetDecimal(reader.GetOrdinal("precioUSD")),
                                PrecioPEN = reader.IsDBNull(reader.GetOrdinal("precioPEN")) ? 0 : reader.GetDecimal(reader.GetOrdinal("precioPEN")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("imagen")) ? string.Empty : reader.GetString(reader.GetOrdinal("imagen"))
                            });
                        }
                    }
                }
            }

            return Ok(vuelos);
        }
        [HttpGet("GetVuelosDisponibles")]
        public async Task<IActionResult> GetVuelosDisponibles()
        {
            var vuelos = new List<VUELODATA>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.ListarVuelosDisponibles", connection)) // Procedimiento almacenado para vuelos disponibles
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vuelos.Add(new VUELODATA
                            {
                                VueloId = reader.IsDBNull(reader.GetOrdinal("vueloId")) ? 0 : reader.GetInt32(reader.GetOrdinal("vueloId")),
                                CodigoVuelo = reader.IsDBNull(reader.GetOrdinal("codigoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoVuelo")),
                                OrigenId = reader.IsDBNull(reader.GetOrdinal("origenId")) ? 0 : reader.GetInt32(reader.GetOrdinal("origenId")),
                                DestinoId = reader.IsDBNull(reader.GetOrdinal("destinoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("destinoId")),
                                FechaSalida = reader.IsDBNull(reader.GetOrdinal("fechaSalida")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fechaSalida")),
                                HoraSalida = reader.IsDBNull(reader.GetOrdinal("horaSalida")) ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("horaSalida")),
                                FechaLlegada = reader.IsDBNull(reader.GetOrdinal("fechaLlegada")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fechaLlegada")),
                                HoraLlegada = reader.IsDBNull(reader.GetOrdinal("horaLlegada")) ? TimeSpan.Zero : reader.GetTimeSpan(reader.GetOrdinal("horaLlegada")),
                                EstadoVuelo = reader.IsDBNull(reader.GetOrdinal("estadoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVuelo")),
                                AeronaveId = reader.IsDBNull(reader.GetOrdinal("aeronaveId")) ? 0 : reader.GetInt32(reader.GetOrdinal("aeronaveId")),
                                Aerolinea = reader.IsDBNull(reader.GetOrdinal("aerolinea")) ? string.Empty : reader.GetString(reader.GetOrdinal("aerolinea")),
                                CodigoAerolinea = reader.IsDBNull(reader.GetOrdinal("codigoAerolinea")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoAerolinea")),
                                TripulacionCapitan = reader.IsDBNull(reader.GetOrdinal("tripulacionCapitan")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionCapitan")),
                                TripulacionCopiloto = reader.IsDBNull(reader.GetOrdinal("tripulacionCopiloto")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionCopiloto")),
                                TripulacionAuxiliares = reader.IsDBNull(reader.GetOrdinal("tripulacionAuxiliares")) ? string.Empty : reader.GetString(reader.GetOrdinal("tripulacionAuxiliares")),
                                EscalaId = reader.IsDBNull(reader.GetOrdinal("escalaId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("escalaId")),
                                DuracionEscala = reader.IsDBNull(reader.GetOrdinal("duracionEscala")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("duracionEscala")),
                                ServiciosAdicionales = reader.IsDBNull(reader.GetOrdinal("serviciosAdicionales")) ? string.Empty : reader.GetString(reader.GetOrdinal("serviciosAdicionales")),
                                TarifaEspecial = reader.IsDBNull(reader.GetOrdinal("tarifaEspecial")) ? 0 : reader.GetDecimal(reader.GetOrdinal("tarifaEspecial")),
                                Moneda = reader.IsDBNull(reader.GetOrdinal("moneda")) ? string.Empty : reader.GetString(reader.GetOrdinal("moneda")),
                                CondicionesAdicionales = reader.IsDBNull(reader.GetOrdinal("condicionesAdicionales")) ? string.Empty : reader.GetString(reader.GetOrdinal("condicionesAdicionales")),
                                PrecioUSD = reader.IsDBNull(reader.GetOrdinal("precioUSD")) ? 0 : reader.GetDecimal(reader.GetOrdinal("precioUSD")),
                                PrecioPEN = reader.IsDBNull(reader.GetOrdinal("precioPEN")) ? 0 : reader.GetDecimal(reader.GetOrdinal("precioPEN")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("imagen")) ? string.Empty : reader.GetString(reader.GetOrdinal("imagen"))
                            });
                        }
                    }
                }
            }

            return Ok(vuelos);
        }
        // Obtener los pasajeros por el código de vuelo
        [HttpGet("ObtenerPasajerosPorCodigoVuelo")]
        public async Task<IActionResult> ObtenerPasajerosPorCodigoVuelo(string codigoVuelo)
        {
            var pasajeros = new List<Pasajero>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Ejecutamos el procedimiento almacenado usando Dapper
                var result = await connection.QueryAsync<Pasajero>(
                    "ObtenerPasajerosPorCodigoVuelo",
                    new { codigoVuelo },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                pasajeros.AddRange(result);
            }

            if (pasajeros.Count == 0)
            {
                return NotFound("No se encontraron pasajeros para el vuelo especificado.");
            }

            return Ok(pasajeros);
        }

        // Modelo para los datos de los pasajeros
        public class Pasajero
        {
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
            public int UsuarioId { get; set; }
            public DateTime FechaReserva { get; set; }
            public string TipoPago { get; set; }
            public decimal TotalPago { get; set; }
            public DateTime FechaLlegada { get; set; }
            public TimeSpan HoraLlegada { get; set; }
            public DateTime FechaSalida { get; set; }
            public TimeSpan HoraSalida { get; set; }
            public string EstadoVuelo { get; set; }
            public string CodigoVuelo { get; set; }
            public decimal PrecioUSD { get; set; }
            public decimal PrecioPEN { get; set; }
            public string Imagen { get; set; }
        }
    

public class VUELODATA
        {
            public int VueloId { get; set; }
            public string CodigoVuelo { get; set; }
            public int OrigenId { get; set; }
            public int DestinoId { get; set; }
            public DateTime FechaSalida { get; set; }
            public TimeSpan HoraSalida { get; set; }
            public DateTime FechaLlegada { get; set; }
            public TimeSpan HoraLlegada { get; set; }
            public string EstadoVuelo { get; set; }
            public int AeronaveId { get; set; }
            public string Aerolinea { get; set; }
            public string CodigoAerolinea { get; set; }
            public string TripulacionCapitan { get; set; }
            public string TripulacionCopiloto { get; set; }
            public string TripulacionAuxiliares { get; set; }
            public int? EscalaId { get; set; }
            public int? DuracionEscala { get; set; }
            public string ServiciosAdicionales { get; set; }
            public decimal TarifaEspecial { get; set; }
            public string Moneda { get; set; }
            public string CondicionesAdicionales { get; set; }
            public decimal PrecioUSD { get; set; }
            public decimal PrecioPEN { get; set; }
            public string Imagen { get; set; }  // Nueva propiedad para almacenar la imagen
        }

    }
}

