using Microsoft.EntityFrameworkCore;
using xpectrum_api.models;

namespace xpectrum_api.data
{
    public class xpectrumContext : DbContext
    {
        public xpectrumContext(DbContextOptions<xpectrumContext> options) : base(options) { }

        public DbSet<usuario> usuarios { get; set; }
        public DbSet<aeropuerto> aeropuertos { get; set; }
        public DbSet<aeronave> aeronaves { get; set; }
        public DbSet<vuelo> vuelos { get; set; }
        public DbSet<reserva> reservas { get; set; }
        public DbSet<asiento> asientos { get; set; }
        public DbSet<pasajeroreserva> pasajerosreservas { get; set; }
        public DbSet<servicioadicional> serviciosadicionales { get; set; }
        public DbSet<reservaservicio> reservaservicios { get; set; }
        public DbSet<pago> pagos { get; set; }
        public DbSet<boleto> boletos { get; set; }
        public DbSet<checkin> checkins { get; set; }
        public DbSet<equipaje> equipajes { get; set; }
        public DbSet<ticket> tickets { get; set; }
        public DbSet<configuracionsistema> configuracionessistema { get; set; }
        public DbSet<logacceso> logsaccesos { get; set; }
        public DbSet<notificacion> notificaciones { get; set; }
        public DbSet<permiso> permisos { get; set; }
        public DbSet<rol> roles { get; set; }
        public DbSet<rolpermiso> rolespermisos { get; set; }
        public DbSet<usuariorol> usuariosroles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Claves primarias
            modelBuilder.Entity<usuario>().HasKey(u => u.usuarioid);
            modelBuilder.Entity<aeropuerto>().HasKey(a => a.aeropuertoid);
            modelBuilder.Entity<aeronave>().HasKey(a => a.aeronaveid);
            modelBuilder.Entity<vuelo>().HasKey(v => v.vueloid);
            modelBuilder.Entity<reserva>().HasKey(r => r.reservaId);
            modelBuilder.Entity<asiento>().HasKey(a => a.asientoid);
            modelBuilder.Entity<pasajeroreserva>().HasKey(pr => pr.pasajeroreservaid);
            modelBuilder.Entity<servicioadicional>().HasKey(sa => sa.servicioadicionalid);
            modelBuilder.Entity<reservaservicio>().HasKey(rs => rs.reservaservicioid);
            modelBuilder.Entity<pago>().HasKey(p => p.pagoid);
            modelBuilder.Entity<boleto>().HasKey(b => b.boletoid);
            modelBuilder.Entity<checkin>().HasKey(c => c.checkinid);
            modelBuilder.Entity<equipaje>().HasKey(e => e.equipajeid);
            modelBuilder.Entity<ticket>().HasKey(t => t.ticketid);
            modelBuilder.Entity<configuracionsistema>().HasKey(c => c.configuracionsistemaid);
            modelBuilder.Entity<logacceso>().HasKey(l => l.logaccesoId);
            modelBuilder.Entity<notificacion>().HasKey(n => n.notificacionid);
            modelBuilder.Entity<permiso>().HasKey(p => p.permisocodigo);
            modelBuilder.Entity<rol>().HasKey(r => r.rolid);
            modelBuilder.Entity<rolpermiso>().HasKey(rp => rp.rolpermisoid);
            modelBuilder.Entity<usuariorol>().HasKey(ur => ur.usuariorolid);

            // Relaciones
            modelBuilder.Entity<reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.reservas)
                .HasForeignKey(r => r.usuarioId);

            modelBuilder.Entity<reserva>()
                .HasOne(r => r.Vuelo)
                .WithMany(v => v.reservas)
                .HasForeignKey(r => r.vueloId);

            modelBuilder.Entity<vuelo>()
                .HasOne(v => v.origen)
                .WithMany(a => a.vuelosorigen)
                .HasForeignKey(v => v.origenid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<vuelo>()
                .HasOne(v => v.destino)
                .WithMany(a => a.vuelosdestino)
                .HasForeignKey(v => v.destinoid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<vuelo>()
                .HasOne(v => v.aeronave)
                .WithMany(a => a.vuelos)
                .HasForeignKey(v => v.aeronaveid);

            base.OnModelCreating(modelBuilder);
        }
    }
}
