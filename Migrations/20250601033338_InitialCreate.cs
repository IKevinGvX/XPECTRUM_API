using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xpectrum_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aeronaves",
                columns: table => new
                {
                    aeronaveid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacidad = table.Column<int>(type: "int", nullable: false),
                    matricula = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aeronaves", x => x.aeronaveid);
                });

            migrationBuilder.CreateTable(
                name: "aeropuertos",
                columns: table => new
                {
                    aeropuertoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigoiata = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aeropuertos", x => x.aeropuertoid);
                });

            migrationBuilder.CreateTable(
                name: "configuracionessistema",
                columns: table => new
                {
                    configuracionsistemaid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    valor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracionessistema", x => x.configuracionsistemaid);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    permisocodigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombrepermiso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.permisocodigo);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    rolid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombrerol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.rolid);
                });

            migrationBuilder.CreateTable(
                name: "serviciosadicionales",
                columns: table => new
                {
                    servicioadicionalid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreservicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviciosadicionales", x => x.servicioadicionalid);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contraseñahash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipousuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    preferenciasnotificaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecharegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.usuarioid);
                });

            migrationBuilder.CreateTable(
                name: "vuelos",
                columns: table => new
                {
                    vueloid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigovuelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    origenid = table.Column<int>(type: "int", nullable: false),
                    destinoid = table.Column<int>(type: "int", nullable: false),
                    fechasalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    horasalida = table.Column<TimeSpan>(type: "time", nullable: false),
                    fechallegada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    horallegada = table.Column<TimeSpan>(type: "time", nullable: false),
                    estadovuelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aeronaveid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vuelos", x => x.vueloid);
                    table.ForeignKey(
                        name: "FK_vuelos_aeronaves_aeronaveid",
                        column: x => x.aeronaveid,
                        principalTable: "aeronaves",
                        principalColumn: "aeronaveid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vuelos_aeropuertos_destinoid",
                        column: x => x.destinoid,
                        principalTable: "aeropuertos",
                        principalColumn: "aeropuertoid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vuelos_aeropuertos_origenid",
                        column: x => x.origenid,
                        principalTable: "aeropuertos",
                        principalColumn: "aeropuertoid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rolespermisos",
                columns: table => new
                {
                    rolpermisoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolid = table.Column<int>(type: "int", nullable: false),
                    permisocodigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    permisocodigo1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolespermisos", x => x.rolpermisoid);
                    table.ForeignKey(
                        name: "FK_rolespermisos_permisos_permisocodigo1",
                        column: x => x.permisocodigo1,
                        principalTable: "permisos",
                        principalColumn: "permisocodigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rolespermisos_roles_rolid",
                        column: x => x.rolid,
                        principalTable: "roles",
                        principalColumn: "rolid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logsaccesos",
                columns: table => new
                {
                    logaccesoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioid = table.Column<int>(type: "int", nullable: false),
                    fechahora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iporigen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    resultado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logsaccesos", x => x.logaccesoId);
                    table.ForeignKey(
                        name: "FK_logsaccesos_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificaciones",
                columns: table => new
                {
                    notificacionid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioid = table.Column<int>(type: "int", nullable: false),
                    tiponotificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaenvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estadolectura = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificaciones", x => x.notificacionid);
                    table.ForeignKey(
                        name: "FK_notificaciones_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    ticketid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioid = table.Column<int>(type: "int", nullable: false),
                    fechacreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    asunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estadoticket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechacierre = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.ticketid);
                    table.ForeignKey(
                        name: "FK_tickets_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuariosroles",
                columns: table => new
                {
                    usuariorolid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioid = table.Column<int>(type: "int", nullable: false),
                    rolid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuariosroles", x => x.usuariorolid);
                    table.ForeignKey(
                        name: "FK_usuariosroles_roles_rolid",
                        column: x => x.rolid,
                        principalTable: "roles",
                        principalColumn: "rolid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuariosroles_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asientos",
                columns: table => new
                {
                    asientoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vueloid = table.Column<int>(type: "int", nullable: false),
                    numeroasiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    clase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estadoasiento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asientos", x => x.asientoid);
                    table.ForeignKey(
                        name: "FK_asientos_vuelos_vueloid",
                        column: x => x.vueloid,
                        principalTable: "vuelos",
                        principalColumn: "vueloid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    resid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioid = table.Column<int>(type: "int", nullable: false),
                    vueloid = table.Column<int>(type: "int", nullable: false),
                    fechareserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estadoreserva = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalpago = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.resid);
                    table.ForeignKey(
                        name: "FK_reservas_usuarios_usuarioid",
                        column: x => x.usuarioid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservas_vuelos_vueloid",
                        column: x => x.vueloid,
                        principalTable: "vuelos",
                        principalColumn: "vueloid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "boletos",
                columns: table => new
                {
                    boletoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resid = table.Column<int>(type: "int", nullable: false),
                    codigoboleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaemision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estadoboleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reservaresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boletos", x => x.boletoid);
                    table.ForeignKey(
                        name: "FK_boletos_reservas_reservaresid",
                        column: x => x.reservaresid,
                        principalTable: "reservas",
                        principalColumn: "resid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipajes",
                columns: table => new
                {
                    equipajeid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resid = table.Column<int>(type: "int", nullable: false),
                    peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tipoequipaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estadoequipaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reservaresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipajes", x => x.equipajeid);
                    table.ForeignKey(
                        name: "FK_equipajes_reservas_reservaresid",
                        column: x => x.reservaresid,
                        principalTable: "reservas",
                        principalColumn: "resid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    pagoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resid = table.Column<int>(type: "int", nullable: false),
                    fechapago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    metodopago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estadopago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    referenciapago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reservaresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.pagoid);
                    table.ForeignKey(
                        name: "FK_pagos_reservas_reservaresid",
                        column: x => x.reservaresid,
                        principalTable: "reservas",
                        principalColumn: "resid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pasajerosreservas",
                columns: table => new
                {
                    pasajeroreservaid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resid = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentotipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentonumero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edad = table.Column<int>(type: "int", nullable: false),
                    asientoid = table.Column<int>(type: "int", nullable: false),
                    reservaresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pasajerosreservas", x => x.pasajeroreservaid);
                    table.ForeignKey(
                        name: "FK_pasajerosreservas_asientos_asientoid",
                        column: x => x.asientoid,
                        principalTable: "asientos",
                        principalColumn: "asientoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pasajerosreservas_reservas_reservaresid",
                        column: x => x.reservaresid,
                        principalTable: "reservas",
                        principalColumn: "resid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservaservicios",
                columns: table => new
                {
                    reservaservicioid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resid = table.Column<int>(type: "int", nullable: false),
                    servicioadicionalid = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    reservaresid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservaservicios", x => x.reservaservicioid);
                    table.ForeignKey(
                        name: "FK_reservaservicios_reservas_reservaresid",
                        column: x => x.reservaresid,
                        principalTable: "reservas",
                        principalColumn: "resid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservaservicios_serviciosadicionales_servicioadicionalid",
                        column: x => x.servicioadicionalid,
                        principalTable: "serviciosadicionales",
                        principalColumn: "servicioadicionalid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkins",
                columns: table => new
                {
                    checkinid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    boletoid = table.Column<int>(type: "int", nullable: false),
                    fechacheckin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    metodocheckin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarjetaembarque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estadocheckin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkins", x => x.checkinid);
                    table.ForeignKey(
                        name: "FK_checkins_boletos_boletoid",
                        column: x => x.boletoid,
                        principalTable: "boletos",
                        principalColumn: "boletoid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asientos_vueloid",
                table: "asientos",
                column: "vueloid");

            migrationBuilder.CreateIndex(
                name: "IX_boletos_reservaresid",
                table: "boletos",
                column: "reservaresid");

            migrationBuilder.CreateIndex(
                name: "IX_checkins_boletoid",
                table: "checkins",
                column: "boletoid");

            migrationBuilder.CreateIndex(
                name: "IX_equipajes_reservaresid",
                table: "equipajes",
                column: "reservaresid");

            migrationBuilder.CreateIndex(
                name: "IX_logsaccesos_usuarioid",
                table: "logsaccesos",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_notificaciones_usuarioid",
                table: "notificaciones",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_pagos_reservaresid",
                table: "pagos",
                column: "reservaresid");

            migrationBuilder.CreateIndex(
                name: "IX_pasajerosreservas_asientoid",
                table: "pasajerosreservas",
                column: "asientoid");

            migrationBuilder.CreateIndex(
                name: "IX_pasajerosreservas_reservaresid",
                table: "pasajerosreservas",
                column: "reservaresid");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_usuarioid",
                table: "reservas",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_vueloid",
                table: "reservas",
                column: "vueloid");

            migrationBuilder.CreateIndex(
                name: "IX_reservaservicios_reservaresid",
                table: "reservaservicios",
                column: "reservaresid");

            migrationBuilder.CreateIndex(
                name: "IX_reservaservicios_servicioadicionalid",
                table: "reservaservicios",
                column: "servicioadicionalid");

            migrationBuilder.CreateIndex(
                name: "IX_rolespermisos_permisocodigo1",
                table: "rolespermisos",
                column: "permisocodigo1");

            migrationBuilder.CreateIndex(
                name: "IX_rolespermisos_rolid",
                table: "rolespermisos",
                column: "rolid");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_usuarioid",
                table: "tickets",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_usuariosroles_rolid",
                table: "usuariosroles",
                column: "rolid");

            migrationBuilder.CreateIndex(
                name: "IX_usuariosroles_usuarioid",
                table: "usuariosroles",
                column: "usuarioid");

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_aeronaveid",
                table: "vuelos",
                column: "aeronaveid");

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_destinoid",
                table: "vuelos",
                column: "destinoid");

            migrationBuilder.CreateIndex(
                name: "IX_vuelos_origenid",
                table: "vuelos",
                column: "origenid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkins");

            migrationBuilder.DropTable(
                name: "configuracionessistema");

            migrationBuilder.DropTable(
                name: "equipajes");

            migrationBuilder.DropTable(
                name: "logsaccesos");

            migrationBuilder.DropTable(
                name: "notificaciones");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "pasajerosreservas");

            migrationBuilder.DropTable(
                name: "reservaservicios");

            migrationBuilder.DropTable(
                name: "rolespermisos");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "usuariosroles");

            migrationBuilder.DropTable(
                name: "boletos");

            migrationBuilder.DropTable(
                name: "asientos");

            migrationBuilder.DropTable(
                name: "serviciosadicionales");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "vuelos");

            migrationBuilder.DropTable(
                name: "aeronaves");

            migrationBuilder.DropTable(
                name: "aeropuertos");
        }
    }
}
