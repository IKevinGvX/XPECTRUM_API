namespace xpectrum_api.models
{
    using System;
    using System.Collections.Generic;

    public class usuario
    {
        public int usuarioid { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string contraseñahash { get; set; }
        public string tipousuario { get; set; }
        public string telefono { get; set; }
        public string estado { get; set; }
        public string? preferenciasnotificaciones { get; set; }
        public DateTime fecharegistro { get; set; }

        public ICollection<reserva> reservas { get; set; }
        public ICollection<logacceso> logsaccesos { get; set; }
        public ICollection<notificacion> notificaciones { get; set; }
        public ICollection<ticket> tickets { get; set; }
        public ICollection<usuariorol> usuarioroles { get; set; }
    }

    public class aeropuerto
    {
        public int aeropuertoid { get; set; }
        public string codigoiata { get; set; }
        public string nombre { get; set; }
        public string ciudad { get; set; }
        public string pais { get; set; }

        public ICollection<vuelo> vuelosorigen { get; set; }
        public ICollection<vuelo> vuelosdestino { get; set; }
    }

    public class aeronave
    {
        public int aeronaveid { get; set; }
        public string modelo { get; set; }
        public int capacidad { get; set; }
        public string matricula { get; set; }

        public ICollection<vuelo> vuelos { get; set; }
    }

    public class vuelo
    {
        public int vueloid { get; set; }
        public string codigovuelo { get; set; }
        public int origenid { get; set; }
        public int destinoid { get; set; }
        public DateTime fechasalida { get; set; }
        public TimeSpan horasalida { get; set; }
        public DateTime fechallegada { get; set; }
        public TimeSpan horallegada { get; set; }
        public string estadovuelo { get; set; }
        public int aeronaveid { get; set; }

        public aeropuerto origen { get; set; }
        public aeropuerto destino { get; set; }
        public aeronave aeronave { get; set; }

        public ICollection<reserva> reservas { get; set; }
        public ICollection<asiento> asientos { get; set; }
    }

    public class reserva
    {
        public int reservaId { get; set; }
        public int usuarioId { get; set; }            // Foreign key al usuario que hizo la reserva
        public int vueloId { get; set; }              // Foreign key al vuelo reservado
        public DateTime fechaReserva { get; set; }   // Fecha en que se hizo la reserva
        public string estadoReserva { get; set; }    // Estado actual de la reserva (ejemplo: Confirmada, Cancelada, Pendiente)
        public decimal totalPago { get; set; }       // Monto total pagado en la reserva

        // Relaciones de navegación
        public usuario Usuario { get; set; }                             // Usuario que realizó la reserva
        public vuelo Vuelo { get; set; }                                 // Vuelo reservado
        public ICollection<pasajeroreserva> PasajerosReservas { get; set; }  // Lista de pasajeros asociados a esta reserva
        public ICollection<reservaservicio> ReservaServicios { get; set; }   // Servicios adicionales asociados a esta reserva
        public ICollection<pago> Pagos { get; set; }                       // Pagos realizados para esta reserva
        public ICollection<boleto> Boletos { get; set; }                   // Boletos generados para esta reserva
        public ICollection<equipaje> Equipajes { get; set; }               // Equipajes relacionados con esta reserva

        public reserva()
        {
            PasajerosReservas = new HashSet<pasajeroreserva>();
            ReservaServicios = new HashSet<reservaservicio>();
            Pagos = new HashSet<pago>();
            Boletos = new HashSet<boleto>();
            Equipajes = new HashSet<equipaje>();
        }
    }

    public class asiento
    {
        public int asientoid { get; set; }
        public int vueloid { get; set; }
        public string numeroasiento { get; set; }
        public string clase { get; set; }
        public string estadoasiento { get; set; }

        public vuelo vuelo { get; set; }
        public ICollection<pasajeroreserva> pasajerosreservas { get; set; }
    }

    public class pasajeroreserva
    {
        public int pasajeroreservaid { get; set; }
        public int resid { get; set; }
        public string nombre { get; set; }
        public string documentotipo { get; set; }
        public string documentonumero { get; set; }
        public int edad { get; set; }
        public int asientoid { get; set; }

        public reserva reserva { get; set; }
        public asiento asiento { get; set; }
    }

    public class servicioadicional
    {
        public int servicioadicionalid { get; set; }
        public string nombreservicio { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }

        public ICollection<reservaservicio> reservaservicios { get; set; }
    }
    public class reservaservicio
    {
        public int reservaservicioid { get; set; }
        public int reservaid { get; set; }                     // FK a reserva
        public int servicioadicionalid { get; set; }      // FK a servicio adicional
        public int cantidad { get; set; }
        public decimal precio { get; set; }                // Nuevo campo precio

        // Relaciones de navegación
        public reserva Reserva { get; set; }
        public servicioadicional ServicioAdicional { get; set; }
    }


    public class pago
    {
        public int pagoid { get; set; }
        public int resid { get; set; }
        public DateTime fechapago { get; set; }
        public decimal monto { get; set; }
        public string metodopago { get; set; }
        public string estadopago { get; set; }
        public string referenciapago { get; set; }

        public reserva reserva { get; set; }
    }

    public class boleto
    {
        public int boletoid { get; set; }
        public int reservaid { get; set; }
        public string codigoboleto { get; set; }
        public DateTime fechaemision { get; set; }
        public string estadoboleto { get; set; }

        public reserva reserva { get; set; }
        public ICollection<checkin> checkins { get; set; }
    }

    public class checkin
    {
        public int checkinid { get; set; }
        public int boletoid { get; set; }
        public DateTime fechacheckin { get; set; }
        public string metodocheckin { get; set; }
        public string tarjetaembarque { get; set; }
        public string estadocheckin { get; set; }

        public boleto boleto { get; set; }
    }

    public class equipaje
    {
        public int equipajeid { get; set; }
        public int resid { get; set; }
        public decimal peso { get; set; }
        public string tipoequipaje { get; set; }
        public string estadoequipaje { get; set; }

        public reserva reserva { get; set; }
    }

    public class ticket
    {
        public int ticketid { get; set; }
        public int usuarioid { get; set; }
        public DateTime fechacreacion { get; set; }
        public string asunto { get; set; }
        public string descripcion { get; set; }
        public string estadoticket { get; set; }
        public DateTime? fechacierre { get; set; }

        public usuario usuario { get; set; }
    }

    public class configuracionsistema
    {
        public int configuracionsistemaid { get; set; }
        public string clave { get; set; }
        public string valor { get; set; }
    }

    public class logacceso
    {
        public int logaccesoId { get; set; }
        public int usuarioid { get; set; }
        public DateTime fechahora { get; set; }
        public string accion { get; set; }
        public string iporigen { get; set; }
        public string resultado { get; set; }

        public usuario usuario { get; set; }
    }

    public class notificacion
    {
        public int notificacionid { get; set; }
        public int usuarioid { get; set; }
        public string tiponotificacion { get; set; }
        public string mensaje { get; set; }
        public DateTime fechaenvio { get; set; }
        public string estadolectura { get; set; }

        public usuario usuario { get; set; }
    }

    public class permiso
    {
        public string permisocodigo { get; set; }
        public string nombrepermiso { get; set; }
        public string descripcion { get; set; }

        public ICollection<rolpermiso> rolespermisos { get; set; }
    }

    public class rol
    {
        public int rolid { get; set; }
        public string nombrerol { get; set; }
        public string descripcion { get; set; }

        public ICollection<usuariorol> usuariosroles { get; set; }
        public ICollection<rolpermiso> rolespermisos { get; set; }
    }

    public class rolpermiso
    {
        public int rolpermisoid { get; set; }
        public int rolid { get; set; }
        public string permisocodigo { get; set; }

        public rol rol { get; set; }
        public permiso permiso { get; set; }
    }

    public class usuariorol
    {
        public int usuariorolid { get; set; }
        public int usuarioid { get; set; }
        public int rolid { get; set; }

        public usuario usuario { get; set; }
        public rol rol { get; set; }
    }
}
