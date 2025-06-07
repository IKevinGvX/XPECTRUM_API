IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [aeronaves] (
        [aeronaveid] int NOT NULL IDENTITY,
        [modelo] nvarchar(max) NOT NULL,
        [capacidad] int NOT NULL,
        [matricula] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_aeronaves] PRIMARY KEY ([aeronaveid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [aeropuertos] (
        [aeropuertoid] int NOT NULL IDENTITY,
        [codigoiata] nvarchar(max) NOT NULL,
        [nombre] nvarchar(max) NOT NULL,
        [ciudad] nvarchar(max) NOT NULL,
        [pais] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_aeropuertos] PRIMARY KEY ([aeropuertoid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [configuracionessistema] (
        [configuracionsistemaid] int NOT NULL IDENTITY,
        [clave] nvarchar(max) NOT NULL,
        [valor] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_configuracionessistema] PRIMARY KEY ([configuracionsistemaid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [permisos] (
        [permisocodigo] nvarchar(450) NOT NULL,
        [nombrepermiso] nvarchar(max) NOT NULL,
        [descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_permisos] PRIMARY KEY ([permisocodigo])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [roles] (
        [rolid] int NOT NULL IDENTITY,
        [nombrerol] nvarchar(max) NOT NULL,
        [descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_roles] PRIMARY KEY ([rolid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [serviciosadicionales] (
        [servicioadicionalid] int NOT NULL IDENTITY,
        [nombreservicio] nvarchar(max) NOT NULL,
        [descripcion] nvarchar(max) NOT NULL,
        [precio] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_serviciosadicionales] PRIMARY KEY ([servicioadicionalid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [usuarios] (
        [usuarioid] int NOT NULL IDENTITY,
        [nombre] nvarchar(max) NOT NULL,
        [email] nvarchar(max) NOT NULL,
        [contraseñahash] nvarchar(max) NOT NULL,
        [tipousuario] nvarchar(max) NOT NULL,
        [telefono] nvarchar(max) NOT NULL,
        [estado] nvarchar(max) NOT NULL,
        [preferenciasnotificaciones] nvarchar(max) NULL,
        [fecharegistro] datetime2 NOT NULL,
        CONSTRAINT [PK_usuarios] PRIMARY KEY ([usuarioid])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [vuelos] (
        [vueloid] int NOT NULL IDENTITY,
        [codigovuelo] nvarchar(max) NOT NULL,
        [origenid] int NOT NULL,
        [destinoid] int NOT NULL,
        [fechasalida] datetime2 NOT NULL,
        [horasalida] time NOT NULL,
        [fechallegada] datetime2 NOT NULL,
        [horallegada] time NOT NULL,
        [estadovuelo] nvarchar(max) NOT NULL,
        [aeronaveid] int NOT NULL,
        CONSTRAINT [PK_vuelos] PRIMARY KEY ([vueloid]),
        CONSTRAINT [FK_vuelos_aeronaves_aeronaveid] FOREIGN KEY ([aeronaveid]) REFERENCES [aeronaves] ([aeronaveid]) ON DELETE CASCADE,
        CONSTRAINT [FK_vuelos_aeropuertos_destinoid] FOREIGN KEY ([destinoid]) REFERENCES [aeropuertos] ([aeropuertoid]) ON DELETE NO ACTION,
        CONSTRAINT [FK_vuelos_aeropuertos_origenid] FOREIGN KEY ([origenid]) REFERENCES [aeropuertos] ([aeropuertoid]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [rolespermisos] (
        [rolpermisoid] int NOT NULL IDENTITY,
        [rolid] int NOT NULL,
        [permisocodigo] nvarchar(max) NOT NULL,
        [permisocodigo1] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_rolespermisos] PRIMARY KEY ([rolpermisoid]),
        CONSTRAINT [FK_rolespermisos_permisos_permisocodigo1] FOREIGN KEY ([permisocodigo1]) REFERENCES [permisos] ([permisocodigo]) ON DELETE CASCADE,
        CONSTRAINT [FK_rolespermisos_roles_rolid] FOREIGN KEY ([rolid]) REFERENCES [roles] ([rolid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [logsaccesos] (
        [logaccesoId] int NOT NULL IDENTITY,
        [usuarioid] int NOT NULL,
        [fechahora] datetime2 NOT NULL,
        [accion] nvarchar(max) NOT NULL,
        [iporigen] nvarchar(max) NOT NULL,
        [resultado] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_logsaccesos] PRIMARY KEY ([logaccesoId]),
        CONSTRAINT [FK_logsaccesos_usuarios_usuarioid] FOREIGN KEY ([usuarioid]) REFERENCES [usuarios] ([usuarioid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [notificaciones] (
        [notificacionid] int NOT NULL IDENTITY,
        [usuarioid] int NOT NULL,
        [tiponotificacion] nvarchar(max) NOT NULL,
        [mensaje] nvarchar(max) NOT NULL,
        [fechaenvio] datetime2 NOT NULL,
        [estadolectura] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_notificaciones] PRIMARY KEY ([notificacionid]),
        CONSTRAINT [FK_notificaciones_usuarios_usuarioid] FOREIGN KEY ([usuarioid]) REFERENCES [usuarios] ([usuarioid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [tickets] (
        [ticketid] int NOT NULL IDENTITY,
        [usuarioid] int NOT NULL,
        [fechacreacion] datetime2 NOT NULL,
        [asunto] nvarchar(max) NOT NULL,
        [descripcion] nvarchar(max) NOT NULL,
        [estadoticket] nvarchar(max) NOT NULL,
        [fechacierre] datetime2 NULL,
        CONSTRAINT [PK_tickets] PRIMARY KEY ([ticketid]),
        CONSTRAINT [FK_tickets_usuarios_usuarioid] FOREIGN KEY ([usuarioid]) REFERENCES [usuarios] ([usuarioid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [usuariosroles] (
        [usuariorolid] int NOT NULL IDENTITY,
        [usuarioid] int NOT NULL,
        [rolid] int NOT NULL,
        CONSTRAINT [PK_usuariosroles] PRIMARY KEY ([usuariorolid]),
        CONSTRAINT [FK_usuariosroles_roles_rolid] FOREIGN KEY ([rolid]) REFERENCES [roles] ([rolid]) ON DELETE CASCADE,
        CONSTRAINT [FK_usuariosroles_usuarios_usuarioid] FOREIGN KEY ([usuarioid]) REFERENCES [usuarios] ([usuarioid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [asientos] (
        [asientoid] int NOT NULL IDENTITY,
        [vueloid] int NOT NULL,
        [numeroasiento] nvarchar(max) NOT NULL,
        [clase] nvarchar(max) NOT NULL,
        [estadoasiento] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_asientos] PRIMARY KEY ([asientoid]),
        CONSTRAINT [FK_asientos_vuelos_vueloid] FOREIGN KEY ([vueloid]) REFERENCES [vuelos] ([vueloid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [reservas] (
        [resid] int NOT NULL IDENTITY,
        [usuarioid] int NOT NULL,
        [vueloid] int NOT NULL,
        [fechareserva] datetime2 NOT NULL,
        [estadoreserva] nvarchar(max) NOT NULL,
        [totalpago] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_reservas] PRIMARY KEY ([resid]),
        CONSTRAINT [FK_reservas_usuarios_usuarioid] FOREIGN KEY ([usuarioid]) REFERENCES [usuarios] ([usuarioid]) ON DELETE CASCADE,
        CONSTRAINT [FK_reservas_vuelos_vueloid] FOREIGN KEY ([vueloid]) REFERENCES [vuelos] ([vueloid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [boletos] (
        [boletoid] int NOT NULL IDENTITY,
        [resid] int NOT NULL,
        [codigoboleto] nvarchar(max) NOT NULL,
        [fechaemision] datetime2 NOT NULL,
        [estadoboleto] nvarchar(max) NOT NULL,
        [reservaresid] int NOT NULL,
        CONSTRAINT [PK_boletos] PRIMARY KEY ([boletoid]),
        CONSTRAINT [FK_boletos_reservas_reservaresid] FOREIGN KEY ([reservaresid]) REFERENCES [reservas] ([resid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [equipajes] (
        [equipajeid] int NOT NULL IDENTITY,
        [resid] int NOT NULL,
        [peso] decimal(18,2) NOT NULL,
        [tipoequipaje] nvarchar(max) NOT NULL,
        [estadoequipaje] nvarchar(max) NOT NULL,
        [reservaresid] int NOT NULL,
        CONSTRAINT [PK_equipajes] PRIMARY KEY ([equipajeid]),
        CONSTRAINT [FK_equipajes_reservas_reservaresid] FOREIGN KEY ([reservaresid]) REFERENCES [reservas] ([resid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [pagos] (
        [pagoid] int NOT NULL IDENTITY,
        [resid] int NOT NULL,
        [fechapago] datetime2 NOT NULL,
        [monto] decimal(18,2) NOT NULL,
        [metodopago] nvarchar(max) NOT NULL,
        [estadopago] nvarchar(max) NOT NULL,
        [referenciapago] nvarchar(max) NOT NULL,
        [reservaresid] int NOT NULL,
        CONSTRAINT [PK_pagos] PRIMARY KEY ([pagoid]),
        CONSTRAINT [FK_pagos_reservas_reservaresid] FOREIGN KEY ([reservaresid]) REFERENCES [reservas] ([resid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [pasajerosreservas] (
        [pasajeroreservaid] int NOT NULL IDENTITY,
        [resid] int NOT NULL,
        [nombre] nvarchar(max) NOT NULL,
        [documentotipo] nvarchar(max) NOT NULL,
        [documentonumero] nvarchar(max) NOT NULL,
        [edad] int NOT NULL,
        [asientoid] int NOT NULL,
        [reservaresid] int NOT NULL,
        CONSTRAINT [PK_pasajerosreservas] PRIMARY KEY ([pasajeroreservaid]),
        CONSTRAINT [FK_pasajerosreservas_asientos_asientoid] FOREIGN KEY ([asientoid]) REFERENCES [asientos] ([asientoid]) ON DELETE CASCADE,
        CONSTRAINT [FK_pasajerosreservas_reservas_reservaresid] FOREIGN KEY ([reservaresid]) REFERENCES [reservas] ([resid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [reservaservicios] (
        [reservaservicioid] int NOT NULL IDENTITY,
        [resid] int NOT NULL,
        [servicioadicionalid] int NOT NULL,
        [cantidad] int NOT NULL,
        [reservaresid] int NOT NULL,
        CONSTRAINT [PK_reservaservicios] PRIMARY KEY ([reservaservicioid]),
        CONSTRAINT [FK_reservaservicios_reservas_reservaresid] FOREIGN KEY ([reservaresid]) REFERENCES [reservas] ([resid]) ON DELETE CASCADE,
        CONSTRAINT [FK_reservaservicios_serviciosadicionales_servicioadicionalid] FOREIGN KEY ([servicioadicionalid]) REFERENCES [serviciosadicionales] ([servicioadicionalid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE TABLE [checkins] (
        [checkinid] int NOT NULL IDENTITY,
        [boletoid] int NOT NULL,
        [fechacheckin] datetime2 NOT NULL,
        [metodocheckin] nvarchar(max) NOT NULL,
        [tarjetaembarque] nvarchar(max) NOT NULL,
        [estadocheckin] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_checkins] PRIMARY KEY ([checkinid]),
        CONSTRAINT [FK_checkins_boletos_boletoid] FOREIGN KEY ([boletoid]) REFERENCES [boletos] ([boletoid]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_asientos_vueloid] ON [asientos] ([vueloid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_boletos_reservaresid] ON [boletos] ([reservaresid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_checkins_boletoid] ON [checkins] ([boletoid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_equipajes_reservaresid] ON [equipajes] ([reservaresid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_logsaccesos_usuarioid] ON [logsaccesos] ([usuarioid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_notificaciones_usuarioid] ON [notificaciones] ([usuarioid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_pagos_reservaresid] ON [pagos] ([reservaresid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_pasajerosreservas_asientoid] ON [pasajerosreservas] ([asientoid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_pasajerosreservas_reservaresid] ON [pasajerosreservas] ([reservaresid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_reservas_usuarioid] ON [reservas] ([usuarioid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_reservas_vueloid] ON [reservas] ([vueloid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_reservaservicios_reservaresid] ON [reservaservicios] ([reservaresid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_reservaservicios_servicioadicionalid] ON [reservaservicios] ([servicioadicionalid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_rolespermisos_permisocodigo1] ON [rolespermisos] ([permisocodigo1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_rolespermisos_rolid] ON [rolespermisos] ([rolid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_tickets_usuarioid] ON [tickets] ([usuarioid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_usuariosroles_rolid] ON [usuariosroles] ([rolid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_usuariosroles_usuarioid] ON [usuariosroles] ([usuarioid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_vuelos_aeronaveid] ON [vuelos] ([aeronaveid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_vuelos_destinoid] ON [vuelos] ([destinoid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_vuelos_origenid] ON [vuelos] ([origenid]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250601033338_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250601033338_InitialCreate', N'9.0.5');
END;

COMMIT;
GO

