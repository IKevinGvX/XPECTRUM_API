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
        internal object horaSalida;

        public int vueloid { get; set; }
        public string codigovuelo { get; set; } = null!;  // No null según la BD
        public int origenid { get; set; }
        public int destinoid { get; set; }
        public DateTime fechasalida { get; set; }
        public TimeSpan horasalida { get; set; }
        public DateTime fechallegada { get; set; }
        public TimeSpan horallegada { get; set; }
        public string estadovuelo { get; set; } = null!;  // No null según la BD
        public int aeronaveid { get; set; }

        // Nuevas propiedades, nullable porque son NULL en BD
        public string? tipoviaje { get; set; }
        public string? clase { get; set; }
        public string? beneficio { get; set; }
        public decimal? preciousd { get; set; }
        public decimal? preciopen { get; set; }

        // Relaciones (pueden ser null o no según tu diseño)
        public aeropuerto? origen { get; set; }
        public aeropuerto? destino { get; set; }
        public aeronave? aeronave { get; set; }

        public ICollection<reserva>? reservas { get; set; }
        public ICollection<asiento>? asientos { get; set; }
    }

    public class reserva
    {
        public int reservaId { get; set; }
        public int usuarioId { get; set; }            // Foreign key al usuario
        public int vueloId { get; set; }              // Foreign key al vuelo
        public DateTime fechaReserva { get; set; }
        public string estadoReserva { get; set; }
        public decimal totalPago { get; set; }

        // Relaciones de navegación
        public usuario Usuario { get; set; }          // Relación con 'usuario'
        public vuelo Vuelo { get; set; }              // Relación con 'vuelo'
        public ICollection<reservaservicio> ReservaServicios { get; set; }  // Relación con 'reservaservicio'
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
        public int reservaid { get; set; }
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
        public int reservaservicioid { get; set; } // Clave primaria
        public int reservaId { get; set; }         // Clave foránea a 'reserva'
        public int servicioadicionalid { get; set; } // Clave foránea a 'servicioadicional'
        public int cantidad { get; set; }
        public decimal precio { get; set; }

        // Relaciones de navegación
        public reserva Reserva { get; set; }              // Relación con 'reserva'
        public servicioadicional ServicioAdicional { get; set; } // Relación con 'servicioadicional'
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
