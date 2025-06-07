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
            var vuelos = new List<VueloDTO>();

            // Conexión a la base de datos SQL Server
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta que llama al procedimiento almacenado
                using (var command = new SqlCommand("dbo.Splistadodevuelosinvocados", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Ejecutar el procedimiento y leer los resultados
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Aquí tratamos los valores de horaSalida y horaLlegada, si son TimeSpan
                            var horaSalida = reader.IsDBNull(reader.GetOrdinal("horaSalida")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horaSalida"));
                            var horaLlegada = reader.IsDBNull(reader.GetOrdinal("horaLlegada")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horaLlegada"));

                            string horaSalidaString = horaSalida.HasValue ? horaSalida.Value.ToString(@"hh\:mm\:ss") : "00:00:00";
                            string horaLlegadaString = horaLlegada.HasValue ? horaLlegada.Value.ToString(@"hh\:mm\:ss") : "00:00:00";

                            vuelos.Add(new VueloDTO
                            {
                                CodigoVuelo = reader.IsDBNull(reader.GetOrdinal("codigoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoVuelo")),
                                FechaSalida = reader.IsDBNull(reader.GetOrdinal("fechaSalida")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("fechaSalida")),
                                HoraSalida = horaSalidaString,  // Usamos la variable con formato string
                                FechaLlegada = reader.IsDBNull(reader.GetOrdinal("fechaLlegada")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("fechaLlegada")),
                                HoraLlegada = horaLlegadaString, // Usamos la variable con formato string
                                DuracionHoras = reader.IsDBNull(reader.GetOrdinal("duracionHoras")) ? 0 : reader.GetInt32(reader.GetOrdinal("duracionHoras")),
                                DuracionMinutos = reader.IsDBNull(reader.GetOrdinal("duracionMinutos")) ? 0 : reader.GetInt32(reader.GetOrdinal("duracionMinutos")),
                                EstadoVueloFinal = reader.IsDBNull(reader.GetOrdinal("estadoVueloFinal")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVueloFinal")),
                                AeropuertoOrigen = reader.IsDBNull(reader.GetOrdinal("aeropuertoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeropuertoOrigen")),
                                AeropuertoDestino = reader.IsDBNull(reader.GetOrdinal("aeropuertoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeropuertoDestino")),
                                AeronaveModelo = reader.IsDBNull(reader.GetOrdinal("aeronaveModelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeronaveModelo")),
                                AeronaveCapacidad = reader.IsDBNull(reader.GetOrdinal("aeronaveCapacidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("aeronaveCapacidad")),
                                EstadoVuelo = reader.IsDBNull(reader.GetOrdinal("estadoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVuelo")),
                                TipoViaje = reader.IsDBNull(reader.GetOrdinal("tipoviaje")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipoviaje")),
                                Clase = reader.IsDBNull(reader.GetOrdinal("clase")) ? string.Empty : reader.GetString(reader.GetOrdinal("clase")),
                                Beneficio = reader.IsDBNull(reader.GetOrdinal("beneficio")) ? string.Empty : reader.GetString(reader.GetOrdinal("beneficio")),
                                PrecioUSD = reader.IsDBNull(reader.GetOrdinal("preciousd")) ? 0 : reader.GetDecimal(reader.GetOrdinal("preciousd")),
                                PrecioPEN = reader.IsDBNull(reader.GetOrdinal("preciopen")) ? 0 : reader.GetDecimal(reader.GetOrdinal("preciopen")),
                                CiudadOrigen = reader.IsDBNull(reader.GetOrdinal("ciudadOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ciudadOrigen")),
                                CiudadDestino = reader.IsDBNull(reader.GetOrdinal("ciudadDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("ciudadDestino"))
                            });
                        }
                    }
                }
            }

            return Ok(vuelos);
        }

        [HttpGet("Vuelosxpasajeros")]
        public async Task<IActionResult> GetVuelosPasajeros()
        {
            var vuelos = new List<VueloDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Llamar al procedimiento almacenado VueloXpsj
                using (var command = new SqlCommand("VueloXpsj", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Ejecutar el procedimiento almacenado y leer los resultados
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Convertir TimeSpan (horaSalida y horaLlegada) de la base de datos
                            var horaSalida = reader.IsDBNull(reader.GetOrdinal("horaSalida")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horaSalida"));
                            var horaLlegada = reader.IsDBNull(reader.GetOrdinal("horaLlegada")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horaLlegada"));

                            // Convertir TimeSpan a string en formato "hh:mm:ss"
                            string horaSalidaString = horaSalida.HasValue ? horaSalida.Value.ToString(@"hh\:mm\:ss") : "00:00:00";
                            string horaLlegadaString = horaLlegada.HasValue ? horaLlegada.Value.ToString(@"hh\:mm\:ss") : "00:00:00";

                            // Añadir los vuelos a la lista
                            vuelos.Add(new VueloDTO
                            {
                                CodigoVuelo = reader.IsDBNull(reader.GetOrdinal("codigoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigoVuelo")),
                                FechaSalida = reader.IsDBNull(reader.GetOrdinal("fechaSalida")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("fechaSalida")),
                                HoraSalida = horaSalidaString,  // Usamos la variable con formato string
                                FechaLlegada = reader.IsDBNull(reader.GetOrdinal("fechaLlegada")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("fechaLlegada")),
                                HoraLlegada = horaLlegadaString, // Usamos la variable con formato string
                                DuracionHoras = reader.IsDBNull(reader.GetOrdinal("duracionHoras")) ? 0 : reader.GetInt32(reader.GetOrdinal("duracionHoras")),
                                DuracionMinutos = reader.IsDBNull(reader.GetOrdinal("duracionMinutos")) ? 0 : reader.GetInt32(reader.GetOrdinal("duracionMinutos")),
                                EstadoVueloFinal = reader.IsDBNull(reader.GetOrdinal("estadoVueloFinal")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVueloFinal")),
                                AeropuertoOrigen = reader.IsDBNull(reader.GetOrdinal("aeropuertoOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeropuertoOrigen")),
                                AeropuertoDestino = reader.IsDBNull(reader.GetOrdinal("aeropuertoDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeropuertoDestino")),
                                AeronaveModelo = reader.IsDBNull(reader.GetOrdinal("aeronaveModelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("aeronaveModelo")),
                                AeronaveCapacidad = reader.IsDBNull(reader.GetOrdinal("aeronaveCapacidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("aeronaveCapacidad")),
                                EstadoVuelo = reader.IsDBNull(reader.GetOrdinal("estadoVuelo")) ? string.Empty : reader.GetString(reader.GetOrdinal("estadoVuelo")),
                                TipoViaje = reader.IsDBNull(reader.GetOrdinal("tipoviaje")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipoviaje")),
                                Clase = reader.IsDBNull(reader.GetOrdinal("clase")) ? string.Empty : reader.GetString(reader.GetOrdinal("clase")),
                                Beneficio = reader.IsDBNull(reader.GetOrdinal("beneficio")) ? string.Empty : reader.GetString(reader.GetOrdinal("beneficio")),
                                PrecioUSD = reader.IsDBNull(reader.GetOrdinal("preciousd")) ? 0 : reader.GetDecimal(reader.GetOrdinal("preciousd")),
                                PrecioPEN = reader.IsDBNull(reader.GetOrdinal("preciopen")) ? 0 : reader.GetDecimal(reader.GetOrdinal("preciopen")),
                                CiudadOrigen = reader.IsDBNull(reader.GetOrdinal("ciudadOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("ciudadOrigen")),
                                CiudadDestino = reader.IsDBNull(reader.GetOrdinal("ciudadDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("ciudadDestino")),
                                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                                DiaSemana = reader.IsDBNull(reader.GetOrdinal("DiaSemana")) ? string.Empty : reader.GetString(reader.GetOrdinal("DiaSemana"))
                            });
                        }
                    }
                }
            }

            return Ok(vuelos);  // Retornar la lista de vuelos como respuesta
        }
    }

    public class VueloDTO
    {
        public string CodigoVuelo { get; set; }
        public DateTime FechaSalida { get; set; }
        public string HoraSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public string HoraLlegada { get; set; }
        public int DuracionHoras { get; set; }
        public int DuracionMinutos { get; set; }
        public string EstadoVueloFinal { get; set; }
        public string AeropuertoOrigen { get; set; }
        public string AeropuertoDestino { get; set; }
        public string AeronaveModelo { get; set; }
        public int AeronaveCapacidad { get; set; }
        public string EstadoVuelo { get; set; }
        public string TipoViaje { get; set; }
        public string Clase { get; set; }
        public string Beneficio { get; set; }
        public decimal PrecioUSD { get; set; }
        public decimal PrecioPEN { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        public string NombreUsuario { get; set; }
        public string DiaSemana { get; set; }
    }
}
