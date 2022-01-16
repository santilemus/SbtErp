select * from sys.database_scoped_configurations
 where name = 'IDENTITY_CACHE';
if (@@rowcount = 1)
   -- para evitar que en casos donde se detiene abruptamente el servidor sql, reinice los
   -- identity con una brecha de 1000.
   alter database scoped configuration set IDENTITY_CACHE = off
go

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [UsrMedico]    Script Date: 19/11/2020 14:08:55 ******/
If not Exists (select loginname from master.dbo.syslogins 
                where name = 'UsrMedico' and dbname = 'Medico')
  create login UsrMedico with password='Admin#20XXI', default_database=[Medico], default_language=[us_english]
GO

IF not exists (
    SELECT  [name]
    FROM    sys.database_principals
    WHERE   [name] = 'usrMedico')
begin
  create user UsrMedico for login [UsrMedico]
end
go

if not exists(
    SELECT * 
    FROM sys.database_principals 
    WHERE name = 'admins' and type = 'R')
begin
/* creamos el role admins */
  create role admins
end
go
EXEC sp_addrolemember 'admins', 'UsrMedico'
go
/****** Object:  UserDefinedFunction [dbo].[InitialCap]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[InitialCap](@String VARCHAR(8000))
                  RETURNS VARCHAR(8000)
                 AS
 BEGIN 

                   DECLARE @Position INT;

SELECT @String   = STUFF(LOWER(@String),1,1,UPPER(LEFT(@String,1))) COLLATE Latin1_General_Bin,
                    @Position = PATINDEX('%[^A-Za-z''][a-z]%',@String COLLATE Latin1_General_Bin);

                    WHILE @Position > 0
                    SELECT @String   = STUFF(@String,@Position,2,UPPER(SUBSTRING(@String,@Position,2))) COLLATE Latin1_General_Bin,
                    @Position = PATINDEX('%[^A-Za-z''][a-z]%',@String COLLATE Latin1_General_Bin);

                     RETURN @String;
  END ;
GO
/****** Object:  Table [dbo].[ActividadEconomica]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActividadEconomica](
	[Codigo] [varchar](12) NOT NULL,
	[Concepto] [varchar](200) NULL,
	[ActividadPadre] [varchar](12) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ActividadEconomica] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Analysis]    Script Date: 24/1/2021 01:30:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Analysis](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DimensionPropertiesString] [nvarchar](max) NULL,
	[Name] [nvarchar](100) NULL,
	[Criteria] [nvarchar](max) NULL,
	[ObjectTypeName] [nvarchar](max) NULL,
	[ChartSettingsContent] [varbinary](max) NULL,
	[PivotGridSettingsContent] [varbinary](max) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Analysis] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AsociacionProfesional]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AsociacionProfesional](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Direccion] [varchar](150) NULL,
	[Telefono] [varchar](25) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_AsociacionProfesional] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditDataItemPersistent](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
	[OperationType] [nvarchar](100) NULL,
	[Description] [nvarchar](2048) NULL,
	[AuditedObject] [uniqueidentifier] NULL,
	[OldObject] [uniqueidentifier] NULL,
	[NewObject] [uniqueidentifier] NULL,
	[OldValue] [nvarchar](1024) NULL,
	[NewValue] [nvarchar](1024) NULL,
	[PropertyName] [nvarchar](100) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_AuditDataItemPersistent] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditedObjectWeakReference]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditedObjectWeakReference](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[GuidId] [uniqueidentifier] NULL,
	[IntId] [int] NULL,
	[DisplayName] [nvarchar](250) NULL,
 CONSTRAINT [PK_AuditedObjectWeakReference] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BugReport]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugReport](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Titulo] [varchar](100) NULL,
	[Url] [varchar](80) NULL,
	[FechaReporte] [datetime2](7) NULL,
	[CapturaPantalla] [varbinary](max) NULL,
	[Descripcion] [varbinary](max) NULL,
	[ReportadoPor] [varchar](50) NULL,
	[Plataforma] [smallint] NULL,
	[SistemaOperativo] [smallint] NULL,
	[Navegador] [smallint] NULL,
	[PasosReproducir] [varchar](max) NULL,
	[ResultadoEsperado] [varchar](100) NULL,
	[ResultadoActual] [varchar](100) NULL,
	[DefectoSeveridad] [smallint] NULL,
	[DefectoPrioridad] [smallint] NULL,
	[AsignadoA] [varchar](50) NULL,
	[Estado] [smallint] NULL,
	[Correccion] [varchar](100) NULL,
	[FechaCorrecion] [datetime2](7) NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[Notas] [varchar](400) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_BugReport] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cargo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cargo](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](100) NULL,
	[TipoContrato] [int] NULL,
	[TipoSalario] [int] NULL,
	[Salario] [numeric](12, 2) NULL,
	[Obligaciones] [varchar](500) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Cargo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cita]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Subject] [varchar](250) NULL,
	[StartOn] [datetime] NULL,
	[EndOn] [datetime] NULL,
	[AllDay] [bit] NULL,
	[Location] [varchar](100) NULL,
	[Label] [int] NULL,
	[Description] [varchar](max) NULL,
	[Status] [int] NULL,
	[Type] [int] NULL,
	[RemindIn] [float] NULL,
	[AlarmTime] [datetime] NULL,
	[ResourceIds] [nvarchar](max) NULL,
	[RecurrencePattern] [int] NULL,
	[RecurrenceInfoXml] [nvarchar](max) NULL,
	[ReminderInfoXml] [nvarchar](200) NULL,
	[IsPostponed] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
	[Paciente] [int] NULL,
	[SePresento] [bit] NULL,
 CONSTRAINT [PK_Cita] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConCatalogo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConCatalogo](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[CodigoCuenta] [varchar](20) NULL,
	[Nombre] [varchar](150) NULL,
	[Nivel] [smallint] NULL,
	[Activa] [bit] NULL,
	[CtaPadre] [int] NULL,
	[TipoCuenta] [smallint] NULL,
	[TipoSaldoCta] [smallint] NULL,
	[CtaResumen] [bit] NULL,
	[CtaMayor] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConCatalogo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConPeriodo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConPeriodo](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[Comentario] [varchar](250) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConPeriodo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Constante]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Constante](
	[Codigo] [varchar](25) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[TipoConstante] [int] NULL,
	[Valor] [varchar](150) NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Constante] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consulta]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consulta](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[Medico] [int] NULL,
	[Paciente] [int] NULL,
	[Consultorio] [int] NULL,
	[Fecha] [datetime] NULL,
	[UnidadDeRemision] [varchar](100) NULL,
	[RealizarExamenes] [bit] NULL,
	[ProximaCita] [datetime] NULL,
	[Diagnostico] [varchar](1000) NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Consulta] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaDiagnostico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaDiagnostico](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Problema] [int] NULL,
	[Descripcion] [nvarchar](250) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaDiagnostico] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaExamen]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaExamen](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Examen] [int] NULL,
	[Fecha] [datetime] NULL,
	[Laboratorio] [int] NULL,
	[FechaPresentacion] [datetime] NULL,
	[Documento] [uniqueidentifier] NULL,
	[Resultado] [varchar](400) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaExamen] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaExamenFisico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaExamenFisico](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Fecha] [datetime] NULL,
	[Descripcion] [varchar](250) NULL,
	[Resultado] [varchar](400) NULL,
	[Documento] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaExamenFisico] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaIncapacidad]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaIncapacidad](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[Motivo] [varchar](300) NULL,
	[OptimisticLockField] [int] NOT NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaIncapacidad] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaParametro]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaParametro](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Consulta] [int] NULL,
	[Nombre] [varchar](30) NULL,
	[Descripcion] [varchar](60) NULL,
	[Tipo] [smallint] NULL,
	[Prioridad] [smallint] NULL,
	[Visible] [bit] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaParametro] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaReceta]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaReceta](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Medicamento] [int] NULL,
	[Farmaceutica] [int] NULL,
	[Dosis] [varchar](100) NULL,
	[Cantidad] [numeric](12, 2) NULL,
	[ViaAdministracion] [int] NULL,
	[Frecuencia] [varchar](100) NULL,
	[Precaucion] [varchar](250) NULL,
	[MuestraMedica] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaReceta] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaSigno]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaSigno](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Signo] [int] NULL,
	[Valor] [numeric](12, 2) NULL,
	[Imc] [int] NULL,
	[PercentilTablaDetalle] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaSigno] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConsultaSintoma]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsultaSintoma](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Nombre] [varchar](250) NULL,
	[Descripcion] [varchar](100) NULL,
	[Intensidad] [nvarchar](10) NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ConsultaSintoma] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[OID] [int] NOT NULL,
	[Empresa] [int] NULL,
	[Unidad] [int] NULL,
	[TipoSalario] [int] NULL,
	[Salario] [money] NULL,
	[Estado] [varchar](12) NULL,
	[Nacionalidad] [varchar](8) NULL,
	[TipoCuenta] [int] NULL,
	[NumeroCuenta] [varchar](14) NULL,
	[Pensionado] [bit] NULL,
	[NumeroCarne] [varchar](10) NULL,
	[Titulo] [varchar](12) NULL,
	[NumeroJVPM] [varchar](10) NULL,
	[Cargo] [int] NULL,
	[TipoContrato] [int] NULL,
	[Banco] [int] NULL,
	[DiaDescanso] [smallint] NULL,
	[AFP] [int] NULL,
	[Color] [int] NULL,
	[Caption] [varchar](100) NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpleadoCapacitacion]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpleadoCapacitacion](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empleado] [int] NULL,
	[Codigo] [varchar](12) NULL,
	[Descripcion] [varchar](150) NULL,
	[FechaInicio] [datetime] NULL,
	[DiasDuracion] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpleadoCapacitacion] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpleadoMembresia]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpleadoMembresia](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empleado] [int] NULL,
	[AsociacionProfesional] [int] NULL,
	[Vigente] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpleadoMembresia] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpleadoPariente]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpleadoPariente](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empleado] [int] NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Tipo] [varchar](12) NULL,
	[FechaNacimiento] [datetime] NULL,
	[Beneficiario] [bit] NULL,
	[Direccion] [varchar](150) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpleadoPariente] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpleadoProfesion]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpleadoProfesion](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empleado] [int] NULL,
	[Profesion] [int] NULL,
	[NumeroProfesional] [varchar](12) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpleadoProfesion] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RazonSocial] [varchar](200) NULL,
	[Pais] [varchar](8) NULL,
	[Provincia] [varchar](8) NULL,
	[Ciudad] [varchar](8) NULL,
	[Direccion] [varchar](200) NULL,
	[Nit] [varchar](14) NULL,
	[NRC] [varchar](14) NULL,
	[EMail] [varchar](60) NULL,
	[SitioWeb] [varchar](60) NULL,
	[Activa] [bit] NULL,
	[Logo] [varbinary](max) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpresaGiro]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpresaGiro](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[ActEconomica] [varchar](12) NULL,
	[Vigente] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpresaGiro] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpresaTelefono]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpresaTelefono](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[Telefono] [varchar](14) NOT NULL,
	[Empresa] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpresaTelefono] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpresaUnidad]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpresaUnidad](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[Nombre] [varchar](100) NULL,
	[Direccion] [varchar](200) NULL,
	[UnidadPadre] [int] NULL,
	[Telefono] [varchar](20) NULL,
	[IdRole] [smallint] NULL,
	[Codigo] [varchar](6) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EmpresaUnidad] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enfermedad]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enfermedad](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CodigoCie] [varchar](6) NULL,
	[Nombre] [varchar](300) NULL,
	[Categoria] [int] NULL,
	[EsGrupo] [bit] NULL,
	[Comentario] [varchar](250) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Enfermedad] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstiloVida]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstiloVida](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[Descripcion] [varchar](200) NULL,
	[Factor] [nvarchar](10) NULL,
	[Estado] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_EstiloVida] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ResourceIds] [nvarchar](max) NULL,
	[RecurrencePattern] [uniqueidentifier] NULL,
	[Subject] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[StartOn] [datetime] NULL,
	[EndOn] [datetime] NULL,
	[AllDay] [bit] NULL,
	[Location] [nvarchar](100) NULL,
	[Label] [int] NULL,
	[Status] [int] NULL,
	[Type] [int] NULL,
	[RemindIn] [float] NULL,
	[ReminderInfoXml] [nvarchar](200) NULL,
	[AlarmTime] [datetime] NULL,
	[IsPostponed] [bit] NULL,
	[RecurrenceInfoXml] [nvarchar](max) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examen]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examen](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Categoria] [nvarchar](10) NULL,
	[Comentario] [varchar](250) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Examen] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FactorRiesgo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FactorRiesgo](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Diagnostico] [int] NULL,
	[Descripcion] [varchar](150) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_FactorRiesgo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileData]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileData](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[size] [int] NULL,
	[FileName] [nvarchar](260) NULL,
	[Content] [varbinary](max) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_FileData] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistoriaFamiliar]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistoriaFamiliar](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[QuePadecen] [varchar](200) NULL,
	[Quienes] [varchar](200) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_HistoriaFamiliar] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[InventarioTipoMovimiento]    Script Date: 26/10/2021 11:25:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InventarioTipoMovimiento](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Padre] [int] NULL,
	[Nombre] [varchar](60) NULL,
	[Codigo] [varchar](8) NULL,
	[Operacion] [smallint] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_InventarioTipoMovimiento] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Inventario]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventario](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Bodega] [int] NULL,
	[Producto] [int] NULL,
	[TipoMovimiento] [int] NULL,
	[Cantidad] [numeric](12, 2) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kardex]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kardex](
	[Bodega] [int] NULL,
	[Oid] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Fecha] [datetime] NULL,
	[TipoMovimiento] [int] NULL,
	[Cantidad] [numeric](12, 2) NULL,
	[CostoUnidad] [numeric](16, 8) NULL,
	[PrecioUnidad] [numeric](14, 4) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Kardex] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listas]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listas](
	[Codigo] [varchar](12) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Categoria] [int] NULL,
	[Comentario] [varchar](250) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_gloListas] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicamento]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicamento](
	[OID] [int] NOT NULL,
	[ContraIndicacion] [varchar](200) NULL,
	[Concentracion] [varchar](100) NULL,
	[Via] [varchar](25) NULL,
	[Prioridad] [smallint] NULL,
	[NivelUso] [smallint] NULL,
 CONSTRAINT [PK_Medicamento] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicamentoDosis]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicamentoDosis](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Medicamento] [int] NULL,
	[Dosis] [smallint] NULL,
	[Edad] [varchar](50) NULL,
	[Comentario] [varchar](200) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_MedicamentoDosis] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicamentoVia]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicamentoVia](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Medicamento] [int] NULL,
	[Via] [nvarchar](10) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_MedicamentoVia] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medico-Citas]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medico-Citas](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Citas] [int] NULL,
	[Medicos] [int] NULL,
	[OptimisticLockField] [int] NULL,
 CONSTRAINT [PK_Medico-Citas] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicoConsultorio]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicoConsultorio](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Medico] [int] NULL,
	[Consultorio] [int] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_MedicoConsultorio] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicoEspecialidad]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicoEspecialidad](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Medico] [int] NULL,
	[Especialidad] [nvarchar](10) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_MedicoEspecialidad] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedLista]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedLista](
	[Codigo] [nvarchar](10) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Categoria] [int] NULL,
	[Activo] [bit] NULL,
	[Comentario] [varchar](250) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_MedicoListas] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelDifference]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelDifference](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UserId] [nvarchar](100) NULL,
	[ContextId] [nvarchar](100) NULL,
	[Version] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ModelDifference] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelDifferenceAspect]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelDifferenceAspect](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Xml] [nvarchar](max) NULL,
	[Owner] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ModelDifferenceAspect] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleInfo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleInfo](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Version] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[AssemblyFileName] [nvarchar](100) NULL,
	[IsMain] [bit] NULL,
	[OptimisticLockField] [int] NULL,
 CONSTRAINT [PK_ModuleInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Moneda]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Moneda](
	[Codigo] [varchar](3) NOT NULL,
	[Nombre] [varchar](60) NULL,
	[Plural] [varchar](25) NULL,
	[FactorCambio] [numeric](12, 2) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Moneda] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paciente](
	[OID] [int] NOT NULL,
	[Empresa] [int] NULL,
	[Nacionalidad] [varchar](8) NULL,
	[Ocupacion] [varchar](100) NULL,
	[Patrono] [int] NULL,
	[PolizaSeguro] [varchar](50) NULL,
	[TipoSeguro] [varchar](12) NULL,
	[LugarNacimiento] [varchar](100) NULL,
	[Referencia] [varchar](200) NULL,
	[Activo] [bit] NULL,
	[Aseguradora] [int] NULL,
 CONSTRAINT [PK_Paciente] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PacienteFileData]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PacienteFileData](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[File] [uniqueidentifier] NULL,
	[Paciente] [int] NULL,
	[Categoria] [nvarchar](10) NULL,
	[Descripcion] [varchar](100) NULL,
	[Fecha] [datetime2](7) NULL,
	[Vigente] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PacienteFileData] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PacienteMedico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PacienteMedico](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[Medico] [int] NULL,
	[InicioDeRelacion] [datetime] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PacienteMedico] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PacienteVacuna]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PacienteVacuna](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Vacuna] [int] NULL,
	[Fecha] [datetime] NULL,
	[ProximaDosis] [datetime] NULL,
	[Aplicada] [bit] NULL,
	[ParteDeCuerpo] [int] NULL,
	[NoDosis] [smallint] NULL,
	[NoRefuerzo] [smallint] NULL,
	[Farmaceutica] [int] NULL,
	[Marca] [nvarchar](50) NULL,
	[Lote] [nvarchar](25) NULL,
	[Comentario] [nvarchar](200) NULL,
	[Paciente] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PacienteVacunas] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pariente]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pariente](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[IdPariente] [int] NULL,
	[Nombres] [varchar](50) NULL,
	[Apellidos] [varchar](50) NULL,
	[Parentesco] [varchar](12) NULL,
	[FechaNacimiento] [datetime] NULL,
	[Estatura] [numeric](5, 2) NULL,
	[Responsable] [bit] NULL,
	[Diagnostico] [int] NULL,
	[Comentario] [varchar](200) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Pariente] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PercentilTabla]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PercentilTabla](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Genero] [smallint] NULL,
	[Nombre] [varchar](60) NULL,
	[Signo] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PercentilTabla] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PercentilTablaDetalle]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PercentilTablaDetalle](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[PercentilTabla] [int] NOT NULL,
	[EdadMes] [smallint] NULL,
	[Lambda] [numeric](8, 5) NULL,
	[Mu] [numeric](8, 5) NULL,
	[Sigma] [numeric](8, 5) NULL,
	[DesviacionStandard] [numeric](8, 5) NULL,
	[P1st] [numeric](8, 5) NULL,
	[P3rd] [numeric](8, 5) NULL,
	[P5th] [numeric](8, 5) NULL,
	[P10th] [numeric](8, 5) NULL,
	[P15th] [numeric](8, 5) NULL,
	[P25th] [numeric](8, 5) NULL,
	[P50th] [numeric](8, 5) NULL,
	[P75th] [numeric](8, 5) NULL,
	[P85th] [numeric](8, 5) NULL,
	[P90th] [numeric](8, 5) NULL,
	[P95th] [numeric](8, 5) NULL,
	[P97th] [numeric](8, 5) NULL,
	[P99th] [numeric](8, 5) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PercentilTablaDetalle] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyActionPermissionObject]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyActionPermissionObject](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ActionId] [nvarchar](100) NULL,
	[Role] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyActionPermissionObject] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyMemberPermissionsObject]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyMemberPermissionsObject](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Members] [nvarchar](max) NULL,
	[ReadState] [int] NULL,
	[WriteState] [int] NULL,
	[Criteria] [nvarchar](max) NULL,
	[TypePermissionObject] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyMemberPermissionsObject] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyNavigationPermissionsObject]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyNavigationPermissionsObject](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ItemPath] [nvarchar](max) NULL,
	[NavigateState] [int] NULL,
	[Role] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyNavigationPermissionsObject] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyObjectPermissionsObject]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyObjectPermissionsObject](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Criteria] [nvarchar](max) NULL,
	[ReadState] [int] NULL,
	[WriteState] [int] NULL,
	[DeleteState] [int] NULL,
	[NavigateState] [int] NULL,
	[TypePermissionObject] [uniqueidentifier] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyObjectPermissionsObject] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyRole]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyRole](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](100) NULL,
	[IsAdministrative] [bit] NULL,
	[CanEditModel] [bit] NULL,
	[PermissionPolicy] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyRole] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyTypePermissionsObject]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyTypePermissionsObject](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Role] [uniqueidentifier] NULL,
	[TargetType] [nvarchar](max) NULL,
	[ReadState] [int] NULL,
	[WriteState] [int] NULL,
	[CreateState] [int] NULL,
	[DeleteState] [int] NULL,
	[NavigateState] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyTypePermissionsObject] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyUser]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyUser](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[StoredPassword] [nvarchar](max) NULL,
	[ChangePasswordOnFirstLogon] [bit] NULL,
	[UserName] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyUser] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles](
	[Roles] [uniqueidentifier] NULL,
	[Users] [uniqueidentifier] NULL,
	[OID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[OptimisticLockField] [int] NULL,
 CONSTRAINT [PK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persona]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persona](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Pais] [varchar](8) NULL,
	[Provincia] [varchar](8) NULL,
	[Ciudad] [varchar](8) NULL,
	[Direccion] [varchar](100) NULL,
	[FechaNacimiento] [datetime] NULL,
	[EMail] [varchar](60) NULL,
	[Genero] [int] NULL,
	[EstadoCivil] [int] NULL,
	[FechaIngreso] [datetime] NULL,
	[FechaRetiro] [datetime] NULL,
	[TipoSangre] [varchar](12) NULL,
	[Idioma] [varchar](25) NULL,
	[FechaMuerte] [datetime] NULL,
	[CausaMuerte] [varchar](50) NULL,
	[Fotografia] [varbinary](max) NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonaContacto]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaContacto](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Persona] [int] NULL,
	[Contacto] [int] NULL,
	[Nombre] [varchar](80) NULL,
	[Direccion] [varchar](150) NULL,
	[Telefono] [varchar](25) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PersonaContacto] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonaDocumento]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaDocumento](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Persona] [int] NULL,
	[Tipo] [varchar](12) NULL,
	[Numero] [varchar](14) NULL,
	[Nombre] [varchar](80) NULL,
	[FechaEmision] [datetime] NULL,
	[LugarEmision] [varchar](100) NULL,
	[Vigente] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PersonaDocumento] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonaTelefono]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaTelefono](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[Telefono] [varchar](14) NOT NULL,
	[Persona] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PersonaTelefono] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlanMedico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanMedico](
	[Oid] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](80) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PlanMedico] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlanMedicoDetalle]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanMedicoDetalle](
	[Oid] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Regla] [int] NULL,
	[PlanMedico] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_PlanMedicoDetalle] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProblemaMedico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProblemaMedico](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[Tipo] [nvarchar](10) NULL,
	[Titulo] [varchar](100) NULL,
	[FechaInicio] [datetime] NULL,
	[FechaFin] [datetime] NULL,
	[Frecuencia] [nvarchar](10) NULL,
	[Gravedad] [nvarchar](10) NULL,
	[Diagnostico] [int] NULL,
	[Reaccion] [nvarchar](10) NULL,
	[Comentario] [varchar](200) NULL,
	[Resultado] [nvarchar](10) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProblemaMedico] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProCategoria]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProCategoria](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](150) NULL,
	[Padre] [int] NULL,
	[Codigo] [varchar](20) NULL,
	[Clasificacion] [int] NULL,
	[EsGrupo] [bit] NULL,
	PorcentajeIVA numeric(14,4) null,
	ClasificacionIva smallint null,
	MetodoCosteo smallint,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_GloCategoriaProducto] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Codigo] [varchar](20) NULL,
	[Nombre] [varchar](100) NULL,
	[NombreCorto] [varchar](25) NULL,
	[CodigoBarra] [int] NULL,
	[Categoria] [int] NULL,
	[Presentacion] [int] NULL,
	[CantMinima] [money] NULL,
	[CantMaxima] [money] NULL,
	[UnidadMedida] [varchar](8) NULL,
	[CostoPromedio] [numeric](14,6) null,
	[Activo] [bit] NULL,
	[Empresa] [int] NULL,
	[Comentario] [varchar](200) NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoAtributo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoAtributo](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Atributo] [varchar](12) NULL,
	[Descripcion] [varchar](100) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoAtributo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoCodigoBarra]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoCodigoBarra](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[CodigoBarra] [varchar](20) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoCodigoBarra] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoEnsamble]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoEnsamble](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Item] [int] NULL,
	[Cantidad] [numeric](12, 2) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoEnsamble] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoEquivalente]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoEquivalente](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Equivalente] [int] NULL,
	[Comentario] [nvarchar](100) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoEquivalente] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventarioLote]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventarioLote](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[CodigoLote] [varchar](48) null,
	[FechaFabricacion] [datetime] NULL,
	[FechaVence][datetime] null,
	[Costo] [numeric](16, 8) NULL,
	[Entrada] [numeric](12, 2) NULL,
	[Salida] [numeric](12, 2) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_InventarioLote] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoPrecio]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoPrecio](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Descripcion] [varchar](100) NULL,
	[Tipo] [varchar](12) NULL,
	[PrecioUnitario] [numeric](14, 4) NULL,
	[CantidadDesde] [numeric](12, 2) NULL,
	[CantidadHasta] [numeric](12, 2) NULL,
	[HoraDesde] [datetime] NULL,
	[HoraHasta] [datetime] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoPrecio] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductoProveedor]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoProveedor](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Producto] [int] NULL,
	[Proveedor] [int] NULL,
	[Codigo] [varchar](20) NULL,
	[Fabricante] [int] NULL,
	[PaisOrigen] [varchar](8) NULL,
	[DiasOrden] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProductoProveedor] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profesion]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesion](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CodigoCiuo] [varchar](10) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[TituloCorto] [varchar](25) NULL,
	[Clasificacion] [int] NULL,
	[Activo] [bit] NOT NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Profesion] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProPresentacion]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProPresentacion](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Codigo] [varchar](12) NULL,
	[Nombre] [varchar](50) NULL,
	[Unidades] [numeric](12, 2) NULL,
	[Defecto] [bit] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ProdPresentacion] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecordatorioClinico]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordatorioClinico](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Paciente] [int] NULL,
	[Regla] [int] NULL,
	[Ajuste] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_RecordatorioClinico] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regla]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regla](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Tipo] [smallint] NULL,
	[PracticaDefecto] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Regla] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportDataV2]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportDataV2](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ObjectTypeName] [nvarchar](512) NULL,
	[Content] [varbinary](max) NULL,
	[Name] [nvarchar](100) NULL,
	[ParametersObjectTypeName] [nvarchar](512) NULL,
	[IsInplaceReport] [bit] NULL,
	[PredefinedReportType] [nvarchar](512) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ReportDataV2] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Caption] [nvarchar](100) NULL,
	[Color] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceResources_EventEvents]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceResources_EventEvents](
	[Events] [uniqueidentifier] NULL,
	[Resources] [uniqueidentifier] NULL,
	[OID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[OptimisticLockField] [int] NULL,
 CONSTRAINT [PK_ResourceResources_EventEvents] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Signo]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Signo](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Unidad] [varchar](25) NULL,
	[Activo] [bit] NULL,
	[Tipo] [smallint] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Signo] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysAuditoriaProceso]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysAuditoriaProceso](
	[Oid] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[NombreProceso] [varchar](60) NULL,
	[Bitacora] [varchar](2000) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioCrea] [varchar](25) NULL,
 CONSTRAINT [PK_SysAuditoriaProceso] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysConsulta]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysConsulta](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](30) NULL,
	[Descripcion] [varchar](100) NULL,
	[Ssql] [varchar](2000) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_SysConsulta] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TablaIMC]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TablaIMC](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Descripcion] [varchar](60) NULL,
	[Desde] [numeric](5, 2) NULL,
	[Hasta] [numeric](5, 2) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TablaIMC] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Telefono]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Telefono](
	[Numero] [varchar](14) NOT NULL,
	[Tipo] [smallint] NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Telefono] PRIMARY KEY CLUSTERED 
(
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tercero]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tercero](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](200) NULL,
	[TipoPersona] [int] NULL,
	[TipoContribuyente] [int] NULL,
	[EMail] [varchar](60) NULL,
	[SitioWeb] [varchar](60) NULL,
	[Comision] [numeric](10, 4) NULL,
	[DireccionPrincipal] [int] NULL,
	[Activo] [bit] NULL,
	[ObjectType] [int] NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Tercero] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroContacto]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroContacto](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Telefono] [varchar](25) NULL,
	[EMail] [varchar](60) NULL,
	[Activo] [bit] NULL,
	[Tercero] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroContacto] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroCredito]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroCredito](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[Cliente] [int] NULL,
	[Clasificacion] [varchar](4) NULL,
	[FechaOtorgamiento] [datetime2](7) NULL,
	[DiasCredito] [smallint] NULL,
	[Limite] [numeric](14, 2) NULL,
	[ToleranciaPago] [numeric](14, 2) NULL,
	[DireccionCobro] [int] NULL,
	[FechaCancelacion] [datetime2](7) NULL,
	[Comentario] [varchar](200) NULL,
	[Vigente] [bit] NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroCredito] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroDireccion]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroDireccion](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Tercero] [int] NULL,
	[Pais] [varchar](8) NULL,
	[Provincia] [varchar](8) NULL,
	[Ciudad] [varchar](8) NULL,
	[Direccion] [varchar](200) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroDireccion] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroDocumento]    Script Date: 21/1/2021 00:34:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroDocumento](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Tipo] [varchar](12) NULL,
	[Numero] [varchar](14) NULL,
	[Nombre] [varchar](80) NULL,
	[LugarEmision] [varchar](100) NULL,
	[FechaEmision] [datetime] NULL,
	[Vigente] [bit] NULL,
	[Tercero] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroDocumento] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroGarantia]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroGarantia](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Empresa] [int] NULL,
	[Cliente] [int] NULL,
	[Tipo] [varchar](12) NULL,
	[Descripcion] [varchar](100) NULL,
	[FechaInicio] [datetime2](7) NULL,
	[Valor] [numeric](14, 2) NULL,
	[FechaVence] [datetime2](7) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroGarantia] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroGiro]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroGiro](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ActEconomica] [varchar](12) NULL,
	[Vigente] [bit] NULL,
	[Tercero] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroGiro] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroNota]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroNota](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Tercero] [int] NULL,
	[Fecha] [datetime] NULL,
	[Comentario] [varchar](200) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroNota] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroRole]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroRole](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Codigo] [varchar](10) NULL,
	[IdRole] [int] NULL,
	[Activo] [bit] NULL,
	[Tercero] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroRole] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroSucursal]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroSucursal](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Tercero] [int] NULL,
	[Nombre] [varchar](100) NULL,
	[Direccion] [varchar](100) NULL,
	[Telefono] [varchar](25) NULL,
	[EMail] [varchar](60) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroSucursal] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerceroTelefono]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerceroTelefono](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[Telefono] [varchar](14) NOT NULL,
	[Tercero] [int] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerceroTelefono] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerminologiaAnatomica]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerminologiaAnatomica](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CodigoTA] [varchar](12) NULL,
	[CodigoFMA] [varchar](10) NULL,
	[TerminoAnatomico] [varchar](150) NULL,
	[Categoria] [nvarchar](10) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_TerminologiaAnatomica] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltraSonografiaObstetrica]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltraSonografiaObstetrica](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Tecnico] [int] NULL,
	[Fecha] [datetime2](7) NULL,
	[TipoUltrasonografia] [smallint] NULL,
	[TipoEmbarazo] [smallint] NULL,
	[Diagnostico] [varchar](200) NULL,
	[DatosPlan] [varchar](200) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_UltraSonografiaObstetrica] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltrasonografiaObstetricaDetalle]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltrasonografiaObstetricaDetalle](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Ultrasonografia] [int] NULL,
	[Situacion] [smallint] NULL,
	[Presentacion] [smallint] NULL,
	[Posicion] [smallint] NULL,
	[Dorso] [smallint] NULL,
	[Placenta] [smallint] NULL,
	[FormaPlacentaPrevia] [smallint] NULL,
	[GradoPlacenta] [smallint] NULL,
	[EspesorPlacenta] [numeric](10, 2) NULL,
	[Crl] [numeric](10, 2) NULL,
	[Dbp] [numeric](10, 2) NULL,
	[Ac] [numeric](10, 2) NULL,
	[Dms] [numeric](10, 2) NULL,
	[Fcf] [smallint] NULL,
	[Cc] [numeric](10, 2) NULL,
	[Lf] [numeric](10, 2) NULL,
	[Vv] [numeric](10, 2) NULL,
	[ReaccionDesidual] [numeric](10, 2) NULL,
	[Ila] [numeric](10, 2) NULL,
	[Eg] [smallint] NULL,
	[Genero] [int] NULL,
	[Fpp] [datetime2](7) NULL,
	[PF] [numeric](10, 2) NULL,
	[Pbf] [varchar](50) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_UltrasonografiaObstetricaDetalle] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltrasonografiaPelvica]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltrasonografiaPelvica](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Consulta] [int] NULL,
	[Tecnico] [int] NULL,
	[Fecha] [datetime2](7) NULL,
	[TipoUltrasonografia] [smallint] NULL,
	[Ovario] [varchar](150) NULL,
	[OvarioIzquierdo] [varchar](150) NULL,
	[OvarioDerecho] [varchar](150) NULL,
	[TrompaFalopioIzquierda] [varchar](150) NULL,
	[TrompaFalopioDerecha] [varchar](150) NULL,
	[FondoSaco] [varchar](150) NULL,
	[Observaciones] [varchar](250) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_UltrasonografiaPelvica] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnidadMedida]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnidadMedida](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](60) NULL,
	[TipoSistema] [smallint] NULL,
	[TipoUnidad] [smallint] NULL,
	[Magnitud] [money] NULL,
	[Simbolo] [varchar](3) NULL,
	[Activo] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_UnidadMedida] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Empresa] [int] NULL,
	[Agencia] [int] NULL,
	[Empleado] [int] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vacuna]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vacuna](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nombre] [varchar](100) NULL,
	[CodigoPT] [varchar](8) NULL,
	[DescripcionCPT] [varchar](250) NULL,
	[CodigoCVX] [varchar](6) NULL,
	[Medicamento] [int] NULL,
	[Comentario] [varchar](150) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_Vacuna] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XPObjectType]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XPObjectType](
	[OID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](254) NULL,
	[AssemblyName] [nvarchar](254) NULL,
 CONSTRAINT [PK_XPObjectType] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XPWeakReference]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XPWeakReference](
	[Oid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[TargetType] [int] NULL,
	[TargetKey] [nvarchar](100) NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_XPWeakReference] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ZonaGeografica]    Script Date: 21/1/2021 00:34:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ZonaGeografica](
	[Codigo] [varchar](8) NOT NULL,
	[ZonaPadre] [varchar](8) NULL,
	[Nombre] [varchar](60) NULL,
	[Gentilicio] [varchar](25) NULL,
	[CodigoTelefonico] [varchar](4) NULL,
	[Moneda] [varchar](3) NULL,
	[Activa] [bit] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_ZonaGeografica] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iActividadPadre_ActividadEconomica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iActividadPadre_ActividadEconomica] ON [dbo].[ActividadEconomica]
(
	[ActividadPadre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ActividadEconomica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ActividadEconomica] ON [dbo].[ActividadEconomica]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_AsociacionProfesional]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_AsociacionProfesional] ON [dbo].[AsociacionProfesional]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iAuditedObject_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAuditedObject_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[AuditedObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iModifiedOn_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iModifiedOn_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[ModifiedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iNewObject_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iNewObject_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[NewObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iOldObject_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iOldObject_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[OldObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iOperationType_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iOperationType_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[OperationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iUserName_AuditDataItemPersistent]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iUserName_AuditDataItemPersistent] ON [dbo].[AuditDataItemPersistent]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_BugReport]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_BugReport] ON [dbo].[BugReport]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Cargo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Cargo] ON [dbo].[Cargo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEndOn_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEndOn_Cita] ON [dbo].[Cita]
(
	[EndOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Cita] ON [dbo].[Cita]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_Cita] ON [dbo].[Cita]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_Cita] ON [dbo].[Cita]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRecurrencePattern_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRecurrencePattern_Cita] ON [dbo].[Cita]
(
	[RecurrencePattern] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iStartOn_Cita]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iStartOn_Cita] ON [dbo].[Cita]
(
	[StartOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCtaPadre_ConCatalogo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCtaPadre_ConCatalogo] ON [dbo].[ConCatalogo]
(
	[CtaPadre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_ConCatalogo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_ConCatalogo] ON [dbo].[ConCatalogo]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConCatalogo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConCatalogo] ON [dbo].[ConCatalogo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConPeriodo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConPeriodo] ON [dbo].[ConPeriodo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Constante]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Constante] ON [dbo].[Constante]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsultorio_Consulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsultorio_Consulta] ON [dbo].[Consulta]
(
	[Consultorio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_Consulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_Consulta] ON [dbo].[Consulta]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Consulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Consulta] ON [dbo].[Consulta]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedico_Consulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedico_Consulta] ON [dbo].[Consulta]
(
	[Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_Consulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_Consulta] ON [dbo].[Consulta]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaDiagnostico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaDiagnostico] ON [dbo].[ConsultaDiagnostico]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaDiagnostico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaDiagnostico] ON [dbo].[ConsultaDiagnostico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProblema_ConsultaDiagnostico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProblema_ConsultaDiagnostico] ON [dbo].[ConsultaDiagnostico]
(
	[Problema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaExamen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaExamen] ON [dbo].[ConsultaExamen]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDocumento_ConsultaExamen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDocumento_ConsultaExamen] ON [dbo].[ConsultaExamen]
(
	[Documento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iExamen_ConsultaExamen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iExamen_ConsultaExamen] ON [dbo].[ConsultaExamen]
(
	[Examen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaExamen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaExamen] ON [dbo].[ConsultaExamen]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iLaboratorio_ConsultaExamen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iLaboratorio_ConsultaExamen] ON [dbo].[ConsultaExamen]
(
	[Laboratorio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaExamenFisico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaExamenFisico] ON [dbo].[ConsultaExamenFisico]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDocumento_ConsultaExamenFisico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDocumento_ConsultaExamenFisico] ON [dbo].[ConsultaExamenFisico]
(
	[Documento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaExamenFisico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaExamenFisico] ON [dbo].[ConsultaExamenFisico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaIncapacidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaIncapacidad] ON [dbo].[ConsultaIncapacidad]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaIncapacidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaIncapacidad] ON [dbo].[ConsultaIncapacidad]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaParametro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaParametro] ON [dbo].[ConsultaParametro]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaParametro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaParametro] ON [dbo].[ConsultaParametro]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaReceta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaReceta] ON [dbo].[ConsultaReceta]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iFarmaceutica_ConsultaReceta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFarmaceutica_ConsultaReceta] ON [dbo].[ConsultaReceta]
(
	[Farmaceutica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaReceta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaReceta] ON [dbo].[ConsultaReceta]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedicamento_ConsultaReceta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedicamento_ConsultaReceta] ON [dbo].[ConsultaReceta]
(
	[Medicamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iViaAdministracion_ConsultaReceta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iViaAdministracion_ConsultaReceta] ON [dbo].[ConsultaReceta]
(
	[ViaAdministracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaSigno]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaSigno] ON [dbo].[ConsultaSigno]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaSigno]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaSigno] ON [dbo].[ConsultaSigno]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iImc_ConsultaSigno]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iImc_ConsultaSigno] ON [dbo].[ConsultaSigno]
(
	[Imc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iSigno_ConsultaSigno]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iSigno_ConsultaSigno] ON [dbo].[ConsultaSigno]
(
	[Signo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_ConsultaSintoma]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_ConsultaSintoma] ON [dbo].[ConsultaSintoma]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ConsultaSintoma]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ConsultaSintoma] ON [dbo].[ConsultaSintoma]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iIntensidad_ConsultaSintoma]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iIntensidad_ConsultaSintoma] ON [dbo].[ConsultaSintoma]
(
	[Intensidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iAFP_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAFP_Empleado] ON [dbo].[Empleado]
(
	[AFP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iBanco_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iBanco_Empleado] ON [dbo].[Empleado]
(
	[Banco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCargo_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCargo_Empleado] ON [dbo].[Empleado]
(
	[Cargo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_Empleado] ON [dbo].[Empleado]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iEstado_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEstado_Empleado] ON [dbo].[Empleado]
(
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iNacionalidad_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iNacionalidad_Empleado] ON [dbo].[Empleado]
(
	[Nacionalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iUnidad_Empleado]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iUnidad_Empleado] ON [dbo].[Empleado]
(
	[Unidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCodigo_EmpleadoCapacitacion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCodigo_EmpleadoCapacitacion] ON [dbo].[EmpleadoCapacitacion]
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpleado_EmpleadoCapacitacion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpleado_EmpleadoCapacitacion] ON [dbo].[EmpleadoCapacitacion]
(
	[Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpleadoCapacitacion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpleadoCapacitacion] ON [dbo].[EmpleadoCapacitacion]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iAsociacionProfesional_EmpleadoMembresia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAsociacionProfesional_EmpleadoMembresia] ON [dbo].[EmpleadoMembresia]
(
	[AsociacionProfesional] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpleado_EmpleadoMembresia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpleado_EmpleadoMembresia] ON [dbo].[EmpleadoMembresia]
(
	[Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpleadoMembresia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpleadoMembresia] ON [dbo].[EmpleadoMembresia]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpleado_EmpleadoPariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpleado_EmpleadoPariente] ON [dbo].[EmpleadoPariente]
(
	[Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iFechaNacimiento_EmpleadoPariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFechaNacimiento_EmpleadoPariente] ON [dbo].[EmpleadoPariente]
(
	[FechaNacimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpleadoPariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpleadoPariente] ON [dbo].[EmpleadoPariente]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_EmpleadoPariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_EmpleadoPariente] ON [dbo].[EmpleadoPariente]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpleado_EmpleadoProfesion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpleado_EmpleadoProfesion] ON [dbo].[EmpleadoProfesion]
(
	[Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpleadoProfesion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpleadoProfesion] ON [dbo].[EmpleadoProfesion]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProfesion_EmpleadoProfesion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProfesion_EmpleadoProfesion] ON [dbo].[EmpleadoProfesion]
(
	[Profesion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCiudad_Empresa]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCiudad_Empresa] ON [dbo].[Empresa]
(
	[Ciudad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Empresa]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Empresa] ON [dbo].[Empresa]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iPais_Empresa]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPais_Empresa] ON [dbo].[Empresa]
(
	[Pais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iProvincia_Empresa]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProvincia_Empresa] ON [dbo].[Empresa]
(
	[Provincia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iActEconomica_EmpresaGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iActEconomica_EmpresaGiro] ON [dbo].[EmpresaGiro]
(
	[ActEconomica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_EmpresaGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_EmpresaGiro] ON [dbo].[EmpresaGiro]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpresaGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpresaGiro] ON [dbo].[EmpresaGiro]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_EmpresaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_EmpresaTelefono] ON [dbo].[EmpresaTelefono]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpresaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpresaTelefono] ON [dbo].[EmpresaTelefono]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTelefono_EmpresaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTelefono_EmpresaTelefono] ON [dbo].[EmpresaTelefono]
(
	[Telefono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_EmpresaUnidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_EmpresaUnidad] ON [dbo].[EmpresaUnidad]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EmpresaUnidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EmpresaUnidad] ON [dbo].[EmpresaUnidad]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iUnidadPadre_EmpresaUnidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iUnidadPadre_EmpresaUnidad] ON [dbo].[EmpresaUnidad]
(
	[UnidadPadre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCategoria_Enfermedad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCategoria_Enfermedad] ON [dbo].[Enfermedad]
(
	[Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCodigoCie_Enfermedad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iCodigoCie_Enfermedad] ON [dbo].[Enfermedad]
(
	[CodigoCie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Enfermedad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Enfermedad] ON [dbo].[Enfermedad]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iFactor_EstiloVida]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFactor_EstiloVida] ON [dbo].[EstiloVida]
(
	[Factor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_EstiloVida]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_EstiloVida] ON [dbo].[EstiloVida]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_EstiloVida]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_EstiloVida] ON [dbo].[EstiloVida]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEndOn_Event]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEndOn_Event] ON [dbo].[Event]
(
	[EndOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Event]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Event] ON [dbo].[Event]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRecurrencePattern_Event]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRecurrencePattern_Event] ON [dbo].[Event]
(
	[RecurrencePattern] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iStartOn_Event]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iStartOn_Event] ON [dbo].[Event]
(
	[StartOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCategoria_Examen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCategoria_Examen] ON [dbo].[Examen]
(
	[Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Examen]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Examen] ON [dbo].[Examen]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDiagnostico_FactorRiesgo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDiagnostico_FactorRiesgo] ON [dbo].[FactorRiesgo]
(
	[Diagnostico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_FactorRiesgo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_FactorRiesgo] ON [dbo].[FactorRiesgo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_FileData]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_FileData] ON [dbo].[FileData]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_HistoriaFamiliar]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_HistoriaFamiliar] ON [dbo].[HistoriaFamiliar]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_HistoriaFamiliar]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_HistoriaFamiliar] ON [dbo].[HistoriaFamiliar]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Index [iBodega_Inventario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iBodega_Inventario] ON [dbo].[Inventario]
(
	[Bodega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Inventario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Inventario] ON [dbo].[Inventario]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_Inventario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_Inventario] ON [dbo].[Inventario]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iBodega_Kardex]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iBodega_Kardex] ON [dbo].[Kardex]
(
	[Bodega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Kardex]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Kardex] ON [dbo].[Kardex]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_Kardex]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_Kardex] ON [dbo].[Kardex]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_gloListas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_gloListas] ON [dbo].[Listas]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_MedicamentoDosis]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_MedicamentoDosis] ON [dbo].[MedicamentoDosis]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedicamento_MedicamentoDosis]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedicamento_MedicamentoDosis] ON [dbo].[MedicamentoDosis]
(
	[Medicamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_MedicamentoVia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_MedicamentoVia] ON [dbo].[MedicamentoVia]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedicamento_MedicamentoVia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedicamento_MedicamentoVia] ON [dbo].[MedicamentoVia]
(
	[Medicamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iVia_MedicamentoVia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iVia_MedicamentoVia] ON [dbo].[MedicamentoVia]
(
	[Via] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCitas_Medico-Citas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCitas_Medico-Citas] ON [dbo].[Medico-Citas]
(
	[Citas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCitasMedicos_Medico-Citas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iCitasMedicos_Medico-Citas] ON [dbo].[Medico-Citas]
(
	[Citas] ASC,
	[Medicos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedicos_Medico-Citas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedicos_Medico-Citas] ON [dbo].[Medico-Citas]
(
	[Medicos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsultorio_MedicoConsultorio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsultorio_MedicoConsultorio] ON [dbo].[MedicoConsultorio]
(
	[Consultorio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_MedicoConsultorio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_MedicoConsultorio] ON [dbo].[MedicoConsultorio]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedico_MedicoConsultorio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedico_MedicoConsultorio] ON [dbo].[MedicoConsultorio]
(
	[Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iEspecialidad_MedicoEspecialidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEspecialidad_MedicoEspecialidad] ON [dbo].[MedicoEspecialidad]
(
	[Especialidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_MedicoEspecialidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_MedicoEspecialidad] ON [dbo].[MedicoEspecialidad]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedico_MedicoEspecialidad]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedico_MedicoEspecialidad] ON [dbo].[MedicoEspecialidad]
(
	[Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_MedicoListas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_MedicoListas] ON [dbo].[MedLista]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ModelDifference]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ModelDifference] ON [dbo].[ModelDifference]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ModelDifferenceAspect]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ModelDifferenceAspect] ON [dbo].[ModelDifferenceAspect]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iOwner_ModelDifferenceAspect]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iOwner_ModelDifferenceAspect] ON [dbo].[ModelDifferenceAspect]
(
	[Owner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Moneda]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Moneda] ON [dbo].[Moneda]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iAseguradora_Paciente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAseguradora_Paciente] ON [dbo].[Paciente]
(
	[Aseguradora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_Paciente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_Paciente] ON [dbo].[Paciente]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iNacionalidad_Paciente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iNacionalidad_Paciente] ON [dbo].[Paciente]
(
	[Nacionalidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPatrono_Paciente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPatrono_Paciente] ON [dbo].[Paciente]
(
	[Patrono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipoSeguro_Paciente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipoSeguro_Paciente] ON [dbo].[Paciente]
(
	[TipoSeguro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCategoria_PacienteFileData]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCategoria_PacienteFileData] ON [dbo].[PacienteFileData]
(
	[Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iFile_PacienteFileData]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFile_PacienteFileData] ON [dbo].[PacienteFileData]
(
	[File] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PacienteFileData]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PacienteFileData] ON [dbo].[PacienteFileData]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_PacienteFileData]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_PacienteFileData] ON [dbo].[PacienteFileData]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PacienteMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PacienteMedico] ON [dbo].[PacienteMedico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedico_PacienteMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedico_PacienteMedico] ON [dbo].[PacienteMedico]
(
	[Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_PacienteMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_PacienteMedico] ON [dbo].[PacienteMedico]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iFarmaceutica_PacienteVacunas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFarmaceutica_PacienteVacunas] ON [dbo].[PacienteVacuna]
(
	[Farmaceutica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PacienteVacunas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PacienteVacunas] ON [dbo].[PacienteVacuna]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_PacienteVacunas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_PacienteVacunas] ON [dbo].[PacienteVacuna]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iParteDeCuerpo_PacienteVacunas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iParteDeCuerpo_PacienteVacunas] ON [dbo].[PacienteVacuna]
(
	[ParteDeCuerpo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iVacuna_PacienteVacunas]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iVacuna_PacienteVacunas] ON [dbo].[PacienteVacuna]
(
	[Vacuna] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDiagnostico_Pariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDiagnostico_Pariente] ON [dbo].[Pariente]
(
	[Diagnostico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Pariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Pariente] ON [dbo].[Pariente]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iIdPariente_Pariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iIdPariente_Pariente] ON [dbo].[Pariente]
(
	[IdPariente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_Pariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_Pariente] ON [dbo].[Pariente]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iParentesco_Pariente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iParentesco_Pariente] ON [dbo].[Pariente]
(
	[Parentesco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PercentilTabla]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PercentilTabla] ON [dbo].[PercentilTabla]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iSigno_PercentilTabla]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iSigno_PercentilTabla] ON [dbo].[PercentilTabla]
(
	[Signo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PercentilTablaDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PercentilTablaDetalle] ON [dbo].[PercentilTablaDetalle]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPercentilTabla_PercentilTablaDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPercentilTabla_PercentilTablaDetalle] ON [dbo].[PercentilTablaDetalle]
(
	[PercentilTabla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyActionPermissionObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyActionPermissionObject] ON [dbo].[PermissionPolicyActionPermissionObject]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRole_PermissionPolicyActionPermissionObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRole_PermissionPolicyActionPermissionObject] ON [dbo].[PermissionPolicyActionPermissionObject]
(
	[Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyMemberPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyMemberPermissionsObject] ON [dbo].[PermissionPolicyMemberPermissionsObject]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTypePermissionObject_PermissionPolicyMemberPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTypePermissionObject_PermissionPolicyMemberPermissionsObject] ON [dbo].[PermissionPolicyMemberPermissionsObject]
(
	[TypePermissionObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyNavigationPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyNavigationPermissionsObject] ON [dbo].[PermissionPolicyNavigationPermissionsObject]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRole_PermissionPolicyNavigationPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRole_PermissionPolicyNavigationPermissionsObject] ON [dbo].[PermissionPolicyNavigationPermissionsObject]
(
	[Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyObjectPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyObjectPermissionsObject] ON [dbo].[PermissionPolicyObjectPermissionsObject]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTypePermissionObject_PermissionPolicyObjectPermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTypePermissionObject_PermissionPolicyObjectPermissionsObject] ON [dbo].[PermissionPolicyObjectPermissionsObject]
(
	[TypePermissionObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyRole]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyRole] ON [dbo].[PermissionPolicyRole]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_PermissionPolicyRole]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_PermissionPolicyRole] ON [dbo].[PermissionPolicyRole]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyTypePermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyTypePermissionsObject] ON [dbo].[PermissionPolicyTypePermissionsObject]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRole_PermissionPolicyTypePermissionsObject]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRole_PermissionPolicyTypePermissionsObject] ON [dbo].[PermissionPolicyTypePermissionsObject]
(
	[Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PermissionPolicyUser]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PermissionPolicyUser] ON [dbo].[PermissionPolicyUser]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_PermissionPolicyUser]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_PermissionPolicyUser] ON [dbo].[PermissionPolicyUser]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRoles_PermissionPolicyUserUsers_PermissionPolicyRoleRoles]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRoles_PermissionPolicyUserUsers_PermissionPolicyRoleRoles] ON [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]
(
	[Roles] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRolesUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iRolesUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles] ON [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]
(
	[Roles] ASC,
	[Users] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles] ON [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]
(
	[Users] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCiudad_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCiudad_Persona] ON [dbo].[Persona]
(
	[Ciudad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Persona] ON [dbo].[Persona]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_Persona] ON [dbo].[Persona]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iPais_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPais_Persona] ON [dbo].[Persona]
(
	[Pais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iProvincia_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProvincia_Persona] ON [dbo].[Persona]
(
	[Provincia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipoSangre_Persona]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipoSangre_Persona] ON [dbo].[Persona]
(
	[TipoSangre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iContacto_PersonaContacto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iContacto_PersonaContacto] ON [dbo].[PersonaContacto]
(
	[Contacto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PersonaContacto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PersonaContacto] ON [dbo].[PersonaContacto]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPersona_PersonaContacto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPersona_PersonaContacto] ON [dbo].[PersonaContacto]
(
	[Persona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PersonaDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PersonaDocumento] ON [dbo].[PersonaDocumento]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iNombre_PersonaDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iNombre_PersonaDocumento] ON [dbo].[PersonaDocumento]
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPersona_PersonaDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPersona_PersonaDocumento] ON [dbo].[PersonaDocumento]
(
	[Persona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_PersonaDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_PersonaDocumento] ON [dbo].[PersonaDocumento]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PersonaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PersonaTelefono] ON [dbo].[PersonaTelefono]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPersona_PersonaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPersona_PersonaTelefono] ON [dbo].[PersonaTelefono]
(
	[Persona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTelefono_PersonaTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTelefono_PersonaTelefono] ON [dbo].[PersonaTelefono]
(
	[Telefono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PlanMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PlanMedico] ON [dbo].[PlanMedico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_PlanMedicoDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_PlanMedicoDetalle] ON [dbo].[PlanMedicoDetalle]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPlanMedico_PlanMedicoDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPlanMedico_PlanMedicoDetalle] ON [dbo].[PlanMedicoDetalle]
(
	[PlanMedico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRegla_PlanMedicoDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRegla_PlanMedicoDetalle] ON [dbo].[PlanMedicoDetalle]
(
	[Regla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDiagnostico_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDiagnostico_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Diagnostico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iFrecuencia_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFrecuencia_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Frecuencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iGravedad_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGravedad_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Gravedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iReaccion_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iReaccion_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Reaccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iResultado_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iResultado_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Resultado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_ProblemaMedico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_ProblemaMedico] ON [dbo].[ProblemaMedico]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_GloCategoriaProducto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_GloCategoriaProducto] ON [dbo].[ProCategoria]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPadre_GloCategoriaProducto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPadre_GloCategoriaProducto] ON [dbo].[ProCategoria]
(
	[Padre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCategoria_Producto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCategoria_Producto] ON [dbo].[Producto]
(
	[Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_Producto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_Producto] ON [dbo].[Producto]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Producto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Producto] ON [dbo].[Producto]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_Producto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_Producto] ON [dbo].[Producto]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPresentacion_Producto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPresentacion_Producto] ON [dbo].[Producto]
(
	[Presentacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iAtributo_ProductoAtributo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAtributo_ProductoAtributo] ON [dbo].[ProductoAtributo]
(
	[Atributo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProductoAtributo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoAtributo] ON [dbo].[ProductoAtributo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoAtributo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoAtributo] ON [dbo].[ProductoAtributo]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCodigoBarra_ProductoCodigoBarra]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iCodigoBarra_ProductoCodigoBarra] ON [dbo].[ProductoCodigoBarra]
(
	[CodigoBarra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProductoCodigoBarra]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoCodigoBarra] ON [dbo].[ProductoCodigoBarra]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoCodigoBarra]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoCodigoBarra] ON [dbo].[ProductoCodigoBarra]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProductoEnsamble]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoEnsamble] ON [dbo].[ProductoEnsamble]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iItem_ProductoEnsamble]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iItem_ProductoEnsamble] ON [dbo].[ProductoEnsamble]
(
	[Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoEnsamble]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoEnsamble] ON [dbo].[ProductoEnsamble]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEquivalente_ProductoEquivalente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEquivalente_ProductoEquivalente] ON [dbo].[ProductoEquivalente]
(
	[Equivalente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProductoEquivalente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoEquivalente] ON [dbo].[ProductoEquivalente]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoEquivalente]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoEquivalente] ON [dbo].[ProductoEquivalente]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_InventarioLote]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_InventarioLote] ON [dbo].[InventarioLote]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_InventarioLote]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_InventarioLote] ON [dbo].[InventarioLote]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
create nonclustered index [idxCodigoLote_InventarioLote] on [dbo].[InventarioLote]
(
   [CodigoLote]
)
GO

/****** Object:  Index [iGCRecord_ProductoPrecio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoPrecio] ON [dbo].[ProductoPrecio]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoPrecio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoPrecio] ON [dbo].[ProductoPrecio]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_ProductoPrecio]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_ProductoPrecio] ON [dbo].[ProductoPrecio]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iFabricante_ProductoProveedor]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iFabricante_ProductoProveedor] ON [dbo].[ProductoProveedor]
(
	[Fabricante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProductoProveedor]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProductoProveedor] ON [dbo].[ProductoProveedor]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iPaisOrigen_ProductoProveedor]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaisOrigen_ProductoProveedor] ON [dbo].[ProductoProveedor]
(
	[PaisOrigen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProducto_ProductoProveedor]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProducto_ProductoProveedor] ON [dbo].[ProductoProveedor]
(
	[Producto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iProveedor_ProductoProveedor]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProveedor_ProductoProveedor] ON [dbo].[ProductoProveedor]
(
	[Proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Profesion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Profesion] ON [dbo].[Profesion]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ProdPresentacion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ProdPresentacion] ON [dbo].[ProPresentacion]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_RecordatorioClinico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_RecordatorioClinico] ON [dbo].[RecordatorioClinico]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iPaciente_RecordatorioClinico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPaciente_RecordatorioClinico] ON [dbo].[RecordatorioClinico]
(
	[Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iRegla_RecordatorioClinico]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iRegla_RecordatorioClinico] ON [dbo].[RecordatorioClinico]
(
	[Regla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Regla]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Regla] ON [dbo].[Regla]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ReportDataV2]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ReportDataV2] ON [dbo].[ReportDataV2]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Resource]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Resource] ON [dbo].[Resource]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEvents_ResourceResources_EventEvents]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEvents_ResourceResources_EventEvents] ON [dbo].[ResourceResources_EventEvents]
(
	[Events] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEventsResources_ResourceResources_EventEvents]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iEventsResources_ResourceResources_EventEvents] ON [dbo].[ResourceResources_EventEvents]
(
	[Events] ASC,
	[Resources] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iResources_ResourceResources_EventEvents]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iResources_ResourceResources_EventEvents] ON [dbo].[ResourceResources_EventEvents]
(
	[Resources] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Signo]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Signo] ON [dbo].[Signo]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_SysAuditoriaProceso]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_SysAuditoriaProceso] ON [dbo].[SysAuditoriaProceso]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_SysConsulta]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_SysConsulta] ON [dbo].[SysConsulta]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TablaIMC]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TablaIMC] ON [dbo].[TablaIMC]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Telefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Telefono] ON [dbo].[Telefono]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDireccionPrincipal_Tercero]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDireccionPrincipal_Tercero] ON [dbo].[Tercero]
(
	[DireccionPrincipal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Tercero]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Tercero] ON [dbo].[Tercero]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_Tercero]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_Tercero] ON [dbo].[Tercero]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroContacto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroContacto] ON [dbo].[TerceroContacto]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroContacto]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroContacto] ON [dbo].[TerceroContacto]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCliente_TerceroCredito]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCliente_TerceroCredito] ON [dbo].[TerceroCredito]
(
	[Cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iDireccionCobro_TerceroCredito]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iDireccionCobro_TerceroCredito] ON [dbo].[TerceroCredito]
(
	[DireccionCobro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_TerceroCredito]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_TerceroCredito] ON [dbo].[TerceroCredito]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroCredito]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroCredito] ON [dbo].[TerceroCredito]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCiudad_TerceroDireccion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCiudad_TerceroDireccion] ON [dbo].[TerceroDireccion]
(
	[Ciudad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroDireccion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroDireccion] ON [dbo].[TerceroDireccion]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iPais_TerceroDireccion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iPais_TerceroDireccion] ON [dbo].[TerceroDireccion]
(
	[Pais] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iProvincia_TerceroDireccion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iProvincia_TerceroDireccion] ON [dbo].[TerceroDireccion]
(
	[Provincia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroDireccion]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroDireccion] ON [dbo].[TerceroDireccion]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idxTerceroNoDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [idxTerceroNoDocumento] ON [dbo].[TerceroDocumento]
(
	[Numero] ASC,
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroDocumento] ON [dbo].[TerceroDocumento]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroDocumento] ON [dbo].[TerceroDocumento]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_TerceroDocumento]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_TerceroDocumento] ON [dbo].[TerceroDocumento]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iCliente_TerceroGarantia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCliente_TerceroGarantia] ON [dbo].[TerceroGarantia]
(
	[Cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_TerceroGarantia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_TerceroGarantia] ON [dbo].[TerceroGarantia]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroGarantia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroGarantia] ON [dbo].[TerceroGarantia]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTipo_TerceroGarantia]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTipo_TerceroGarantia] ON [dbo].[TerceroGarantia]
(
	[Tipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iActEconomica_TerceroGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iActEconomica_TerceroGiro] ON [dbo].[TerceroGiro]
(
	[ActEconomica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroGiro] ON [dbo].[TerceroGiro]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroGiro]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroGiro] ON [dbo].[TerceroGiro]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroNota]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroNota] ON [dbo].[TerceroNota]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroNota]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroNota] ON [dbo].[TerceroNota]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroRole]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroRole] ON [dbo].[TerceroRole]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroRole]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroRole] ON [dbo].[TerceroRole]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroSucursal]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroSucursal] ON [dbo].[TerceroSucursal]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroSucursal]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroSucursal] ON [dbo].[TerceroSucursal]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerceroTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerceroTelefono] ON [dbo].[TerceroTelefono]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iTelefono_TerceroTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTelefono_TerceroTelefono] ON [dbo].[TerceroTelefono]
(
	[Telefono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTercero_TerceroTelefono]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTercero_TerceroTelefono] ON [dbo].[TerceroTelefono]
(
	[Tercero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iCategoria_TerminologiaAnatomica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iCategoria_TerminologiaAnatomica] ON [dbo].[TerminologiaAnatomica]
(
	[Categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idxTerminologiaAnatomica_CodigoTA]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idxTerminologiaAnatomica_CodigoTA] ON [dbo].[TerminologiaAnatomica]
(
	[CodigoTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_TerminologiaAnatomica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_TerminologiaAnatomica] ON [dbo].[TerminologiaAnatomica]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_UltraSonografiaObstetrica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_UltraSonografiaObstetrica] ON [dbo].[UltraSonografiaObstetrica]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_UltraSonografiaObstetrica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_UltraSonografiaObstetrica] ON [dbo].[UltraSonografiaObstetrica]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTecnico_UltraSonografiaObstetrica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTecnico_UltraSonografiaObstetrica] ON [dbo].[UltraSonografiaObstetrica]
(
	[Tecnico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_UltrasonografiaObstetricaDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_UltrasonografiaObstetricaDetalle] ON [dbo].[UltrasonografiaObstetricaDetalle]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iUltrasonografia_UltrasonografiaObstetricaDetalle]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iUltrasonografia_UltrasonografiaObstetricaDetalle] ON [dbo].[UltrasonografiaObstetricaDetalle]
(
	[Ultrasonografia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iConsulta_UltrasonografiaPelvica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iConsulta_UltrasonografiaPelvica] ON [dbo].[UltrasonografiaPelvica]
(
	[Consulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_UltrasonografiaPelvica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_UltrasonografiaPelvica] ON [dbo].[UltrasonografiaPelvica]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTecnico_UltrasonografiaPelvica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTecnico_UltrasonografiaPelvica] ON [dbo].[UltrasonografiaPelvica]
(
	[Tecnico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_UnidadMedida]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_UnidadMedida] ON [dbo].[UnidadMedida]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iAgencia_Usuario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iAgencia_Usuario] ON [dbo].[Usuario]
(
	[Agencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpleado_Usuario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpleado_Usuario] ON [dbo].[Usuario]
(
	[Empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iEmpresa_Usuario]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iEmpresa_Usuario] ON [dbo].[Usuario]
(
	[Empresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_Vacuna]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_Vacuna] ON [dbo].[Vacuna]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iMedicamento_Vacuna]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMedicamento_Vacuna] ON [dbo].[Vacuna]
(
	[Medicamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTypeName_XPObjectType]    Script Date: 21/1/2021 00:34:32 ******/
CREATE UNIQUE NONCLUSTERED INDEX [iTypeName_XPObjectType] ON [dbo].[XPObjectType]
(
	[TypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_XPWeakReference]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_XPWeakReference] ON [dbo].[XPWeakReference]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iObjectType_XPWeakReference]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iObjectType_XPWeakReference] ON [dbo].[XPWeakReference]
(
	[ObjectType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iTargetType_XPWeakReference]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iTargetType_XPWeakReference] ON [dbo].[XPWeakReference]
(
	[TargetType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [iGCRecord_ZonaGeografica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iGCRecord_ZonaGeografica] ON [dbo].[ZonaGeografica]
(
	[GCRecord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iMoneda_ZonaGeografica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iMoneda_ZonaGeografica] ON [dbo].[ZonaGeografica]
(
	[Moneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [iZonaPadre_ZonaGeografica]    Script Date: 21/1/2021 00:34:32 ******/
CREATE NONCLUSTERED INDEX [iZonaPadre_ZonaGeografica] ON [dbo].[ZonaGeografica]
(
	[ZonaPadre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Regla] ADD  DEFAULT ((0)) FOR [Tipo]
GO
ALTER TABLE [dbo].[ActividadEconomica]  WITH NOCHECK ADD  CONSTRAINT [FK_ActividadEconomica_ActividadPadre] FOREIGN KEY([ActividadPadre])
REFERENCES [dbo].[ActividadEconomica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ActividadEconomica] CHECK CONSTRAINT [FK_ActividadEconomica_ActividadPadre]
GO
ALTER TABLE [dbo].[AuditDataItemPersistent]  WITH NOCHECK ADD  CONSTRAINT [FK_AuditDataItemPersistent_AuditedObject] FOREIGN KEY([AuditedObject])
REFERENCES [dbo].[AuditedObjectWeakReference] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[AuditDataItemPersistent] CHECK CONSTRAINT [FK_AuditDataItemPersistent_AuditedObject]
GO
ALTER TABLE [dbo].[AuditDataItemPersistent]  WITH NOCHECK ADD  CONSTRAINT [FK_AuditDataItemPersistent_NewObject] FOREIGN KEY([NewObject])
REFERENCES [dbo].[XPWeakReference] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[AuditDataItemPersistent] CHECK CONSTRAINT [FK_AuditDataItemPersistent_NewObject]
GO
ALTER TABLE [dbo].[AuditDataItemPersistent]  WITH NOCHECK ADD  CONSTRAINT [FK_AuditDataItemPersistent_OldObject] FOREIGN KEY([OldObject])
REFERENCES [dbo].[XPWeakReference] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[AuditDataItemPersistent] CHECK CONSTRAINT [FK_AuditDataItemPersistent_OldObject]
GO
ALTER TABLE [dbo].[AuditedObjectWeakReference]  WITH NOCHECK ADD  CONSTRAINT [FK_AuditedObjectWeakReference_Oid] FOREIGN KEY([Oid])
REFERENCES [dbo].[XPWeakReference] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[AuditedObjectWeakReference] CHECK CONSTRAINT [FK_AuditedObjectWeakReference_Oid]
GO
ALTER TABLE [dbo].[Cita]  WITH NOCHECK ADD  CONSTRAINT [FK_Cita_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_ObjectType]
GO
ALTER TABLE [dbo].[Cita]  WITH NOCHECK ADD  CONSTRAINT [FK_Cita_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Paciente]
GO
ALTER TABLE [dbo].[Cita]  WITH NOCHECK ADD  CONSTRAINT [FK_Cita_RecurrencePattern] FOREIGN KEY([RecurrencePattern])
REFERENCES [dbo].[Cita] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_RecurrencePattern]
GO
ALTER TABLE [dbo].[ConCatalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_ConCatalogo_CtaPadre] FOREIGN KEY([CtaPadre])
REFERENCES [dbo].[ConCatalogo] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConCatalogo] CHECK CONSTRAINT [FK_ConCatalogo_CtaPadre]
GO
ALTER TABLE [dbo].[ConCatalogo]  WITH NOCHECK ADD  CONSTRAINT [FK_ConCatalogo_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConCatalogo] CHECK CONSTRAINT [FK_ConCatalogo_Empresa]
GO
ALTER TABLE [dbo].[Consulta]  WITH NOCHECK ADD  CONSTRAINT [FK_Consulta_Consultorio] FOREIGN KEY([Consultorio])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Consultorio]
GO
ALTER TABLE [dbo].[Consulta]  WITH NOCHECK ADD  CONSTRAINT [FK_Consulta_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Empresa]
GO
ALTER TABLE [dbo].[Consulta]  WITH NOCHECK ADD  CONSTRAINT [FK_Consulta_Medico] FOREIGN KEY([Medico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Medico]
GO
ALTER TABLE [dbo].[Consulta]  WITH NOCHECK ADD  CONSTRAINT [FK_Consulta_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Paciente]
GO
ALTER TABLE [dbo].[ConsultaDiagnostico]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaDiagnostico_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaDiagnostico] CHECK CONSTRAINT [FK_ConsultaDiagnostico_Consulta]
GO
ALTER TABLE [dbo].[ConsultaDiagnostico]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaDiagnostico_Problema] FOREIGN KEY([Problema])
REFERENCES [dbo].[ProblemaMedico] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaDiagnostico] CHECK CONSTRAINT [FK_ConsultaDiagnostico_Problema]
GO
ALTER TABLE [dbo].[ConsultaExamen]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamen_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamen] CHECK CONSTRAINT [FK_ConsultaExamen_Consulta]
GO
ALTER TABLE [dbo].[ConsultaExamen]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamen_Documento] FOREIGN KEY([Documento])
REFERENCES [dbo].[PacienteFileData] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamen] CHECK CONSTRAINT [FK_ConsultaExamen_Documento]
GO
ALTER TABLE [dbo].[ConsultaExamen]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamen_Examen] FOREIGN KEY([Examen])
REFERENCES [dbo].[Examen] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamen] CHECK CONSTRAINT [FK_ConsultaExamen_Examen]
GO
ALTER TABLE [dbo].[ConsultaExamen]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamen_Laboratorio] FOREIGN KEY([Laboratorio])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamen] CHECK CONSTRAINT [FK_ConsultaExamen_Laboratorio]
GO
ALTER TABLE [dbo].[ConsultaExamenFisico]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamenFisico_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamenFisico] CHECK CONSTRAINT [FK_ConsultaExamenFisico_Consulta]
GO
ALTER TABLE [dbo].[ConsultaExamenFisico]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaExamenFisico_Documento] FOREIGN KEY([Documento])
REFERENCES [dbo].[PacienteFileData] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaExamenFisico] CHECK CONSTRAINT [FK_ConsultaExamenFisico_Documento]
GO
ALTER TABLE [dbo].[ConsultaIncapacidad]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaIncapacidad_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaIncapacidad] CHECK CONSTRAINT [FK_ConsultaIncapacidad_Consulta]
GO
ALTER TABLE [dbo].[ConsultaParametro]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaParametro_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[SysConsulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaParametro] CHECK CONSTRAINT [FK_ConsultaParametro_Consulta]
GO
ALTER TABLE [dbo].[ConsultaReceta]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaReceta_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaReceta] CHECK CONSTRAINT [FK_ConsultaReceta_Consulta]
GO
ALTER TABLE [dbo].[ConsultaReceta]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaReceta_Farmaceutica] FOREIGN KEY([Farmaceutica])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaReceta] CHECK CONSTRAINT [FK_ConsultaReceta_Farmaceutica]
GO
ALTER TABLE [dbo].[ConsultaReceta]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaReceta_Medicamento] FOREIGN KEY([Medicamento])
REFERENCES [dbo].[Medicamento] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaReceta] CHECK CONSTRAINT [FK_ConsultaReceta_Medicamento]
GO
ALTER TABLE [dbo].[ConsultaReceta]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaReceta_ViaAdministracion] FOREIGN KEY([ViaAdministracion])
REFERENCES [dbo].[MedicamentoVia] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaReceta] CHECK CONSTRAINT [FK_ConsultaReceta_ViaAdministracion]
GO
ALTER TABLE [dbo].[ConsultaSigno]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSigno_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSigno] CHECK CONSTRAINT [FK_ConsultaSigno_Consulta]
GO
ALTER TABLE [dbo].[ConsultaSigno]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSigno_Imc] FOREIGN KEY([Imc])
REFERENCES [dbo].[TablaIMC] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSigno] CHECK CONSTRAINT [FK_ConsultaSigno_Imc]
GO
ALTER TABLE [dbo].[ConsultaSigno]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSigno_PercentilTablaDetalle] FOREIGN KEY([PercentilTablaDetalle])
REFERENCES [dbo].[PercentilTablaDetalle] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSigno] CHECK CONSTRAINT [FK_ConsultaSigno_PercentilTablaDetalle]
GO
ALTER TABLE [dbo].[ConsultaSigno]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSigno_Signo] FOREIGN KEY([Signo])
REFERENCES [dbo].[Signo] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSigno] CHECK CONSTRAINT [FK_ConsultaSigno_Signo]
GO
ALTER TABLE [dbo].[ConsultaSintoma]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSintoma_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSintoma] CHECK CONSTRAINT [FK_ConsultaSintoma_Consulta]
GO
ALTER TABLE [dbo].[ConsultaSintoma]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSintoma_Intensidad] FOREIGN KEY([Intensidad])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ConsultaSintoma] CHECK CONSTRAINT [FK_ConsultaSintoma_Intensidad]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Banco] FOREIGN KEY([Banco])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Banco]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Cargo] FOREIGN KEY([Cargo])
REFERENCES [dbo].[Cargo] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Cargo]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Empresa]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Estado] FOREIGN KEY([Estado])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Estado]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Nacionalidad] FOREIGN KEY([Nacionalidad])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Nacionalidad]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_OID] FOREIGN KEY([OID])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_OID]
GO
ALTER TABLE [dbo].[Empleado]  WITH NOCHECK ADD  CONSTRAINT [FK_Empleado_Unidad] FOREIGN KEY([Unidad])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Unidad]
GO
ALTER TABLE [dbo].[EmpleadoCapacitacion]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoCapacitacion_Codigo] FOREIGN KEY([Codigo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoCapacitacion] CHECK CONSTRAINT [FK_EmpleadoCapacitacion_Codigo]
GO
ALTER TABLE [dbo].[EmpleadoCapacitacion]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoCapacitacion_Empleado] FOREIGN KEY([Empleado])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoCapacitacion] CHECK CONSTRAINT [FK_EmpleadoCapacitacion_Empleado]
GO
ALTER TABLE [dbo].[EmpleadoMembresia]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoMembresia_AsociacionProfesional] FOREIGN KEY([AsociacionProfesional])
REFERENCES [dbo].[AsociacionProfesional] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoMembresia] CHECK CONSTRAINT [FK_EmpleadoMembresia_AsociacionProfesional]
GO
ALTER TABLE [dbo].[EmpleadoMembresia]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoMembresia_Empleado] FOREIGN KEY([Empleado])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoMembresia] CHECK CONSTRAINT [FK_EmpleadoMembresia_Empleado]
GO
ALTER TABLE [dbo].[EmpleadoPariente]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoPariente_Empleado] FOREIGN KEY([Empleado])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoPariente] CHECK CONSTRAINT [FK_EmpleadoPariente_Empleado]
GO
ALTER TABLE [dbo].[EmpleadoPariente]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoPariente_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoPariente] CHECK CONSTRAINT [FK_EmpleadoPariente_Tipo]
GO
ALTER TABLE [dbo].[EmpleadoProfesion]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoProfesion_Empleado] FOREIGN KEY([Empleado])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoProfesion] CHECK CONSTRAINT [FK_EmpleadoProfesion_Empleado]
GO
ALTER TABLE [dbo].[EmpleadoProfesion]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpleadoProfesion_Profesion] FOREIGN KEY([Profesion])
REFERENCES [dbo].[Profesion] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpleadoProfesion] CHECK CONSTRAINT [FK_EmpleadoProfesion_Profesion]
GO
ALTER TABLE [dbo].[Empresa]  WITH NOCHECK ADD  CONSTRAINT [FK_Empresa_Ciudad] FOREIGN KEY([Ciudad])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empresa] CHECK CONSTRAINT [FK_Empresa_Ciudad]
GO
ALTER TABLE [dbo].[Empresa]  WITH NOCHECK ADD  CONSTRAINT [FK_Empresa_Pais] FOREIGN KEY([Pais])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empresa] CHECK CONSTRAINT [FK_Empresa_Pais]
GO
ALTER TABLE [dbo].[Empresa]  WITH NOCHECK ADD  CONSTRAINT [FK_Empresa_Provincia] FOREIGN KEY([Provincia])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Empresa] CHECK CONSTRAINT [FK_Empresa_Provincia]
GO
ALTER TABLE [dbo].[EmpresaGiro]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpresaGiro_ActEconomica] FOREIGN KEY([ActEconomica])
REFERENCES [dbo].[ActividadEconomica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpresaGiro] CHECK CONSTRAINT [FK_EmpresaGiro_ActEconomica]
GO
ALTER TABLE [dbo].[EmpresaGiro]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpresaGiro_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpresaGiro] CHECK CONSTRAINT [FK_EmpresaGiro_Empresa]
GO
ALTER TABLE [dbo].[EmpresaTelefono]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpresaTelefono_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpresaTelefono] CHECK CONSTRAINT [FK_EmpresaTelefono_Empresa]
GO
ALTER TABLE [dbo].[EmpresaTelefono]  WITH CHECK ADD  CONSTRAINT [FK_EmpresaTelefono_Telefono] FOREIGN KEY([Telefono])
REFERENCES [dbo].[Telefono] ([Numero])
GO
ALTER TABLE [dbo].[EmpresaTelefono] CHECK CONSTRAINT [FK_EmpresaTelefono_Telefono]
GO
ALTER TABLE [dbo].[EmpresaUnidad]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpresaUnidad_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpresaUnidad] CHECK CONSTRAINT [FK_EmpresaUnidad_Empresa]
GO
ALTER TABLE [dbo].[EmpresaUnidad]  WITH NOCHECK ADD  CONSTRAINT [FK_EmpresaUnidad_UnidadPadre] FOREIGN KEY([UnidadPadre])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EmpresaUnidad] CHECK CONSTRAINT [FK_EmpresaUnidad_UnidadPadre]
GO
ALTER TABLE [dbo].[Enfermedad]  WITH NOCHECK ADD  CONSTRAINT [FK_Enfermedad_Categoria] FOREIGN KEY([Categoria])
REFERENCES [dbo].[Enfermedad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Enfermedad] CHECK CONSTRAINT [FK_Enfermedad_Categoria]
GO
ALTER TABLE [dbo].[EstiloVida]  WITH NOCHECK ADD  CONSTRAINT [FK_EstiloVida_Factor] FOREIGN KEY([Factor])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EstiloVida] CHECK CONSTRAINT [FK_EstiloVida_Factor]
GO
ALTER TABLE [dbo].[EstiloVida]  WITH NOCHECK ADD  CONSTRAINT [FK_EstiloVida_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[EstiloVida] CHECK CONSTRAINT [FK_EstiloVida_Paciente]
GO
ALTER TABLE [dbo].[Event]  WITH NOCHECK ADD  CONSTRAINT [FK_Event_RecurrencePattern] FOREIGN KEY([RecurrencePattern])
REFERENCES [dbo].[Event] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_RecurrencePattern]
GO
ALTER TABLE [dbo].[Examen]  WITH NOCHECK ADD  CONSTRAINT [FK_Examen_Categoria] FOREIGN KEY([Categoria])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Examen] CHECK CONSTRAINT [FK_Examen_Categoria]
GO
ALTER TABLE [dbo].[FactorRiesgo]  WITH NOCHECK ADD  CONSTRAINT [FK_FactorRiesgo_Diagnostico] FOREIGN KEY([Diagnostico])
REFERENCES [dbo].[Enfermedad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[FactorRiesgo] CHECK CONSTRAINT [FK_FactorRiesgo_Diagnostico]
GO
ALTER TABLE [dbo].[HistoriaFamiliar]  WITH NOCHECK ADD  CONSTRAINT [FK_HistoriaFamiliar_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[HistoriaFamiliar] CHECK CONSTRAINT [FK_HistoriaFamiliar_Paciente]
GO

ALTER TABLE [dbo].[InventarioTipoMovimiento]  WITH NOCHECK ADD  CONSTRAINT [FK_InventarioTipoMovimiento_Padre] FOREIGN KEY([Padre])
REFERENCES [dbo].[InventarioTipoMovimiento] ([OID])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[InventarioTipoMovimiento] CHECK CONSTRAINT [FK_InventarioTipoMovimiento_Padre]
GO


ALTER TABLE [dbo].[Inventario]  WITH NOCHECK ADD  CONSTRAINT [FK_Inventario_Bodega] FOREIGN KEY([Bodega])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Bodega]
GO
ALTER TABLE [dbo].[Inventario]  WITH NOCHECK ADD  CONSTRAINT [FK_Inventario_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Producto]
GO

ALTER TABLE [dbo].[Inventario]  WITH NOCHECK ADD  CONSTRAINT [FK_Inventario_TipoMovimiento] FOREIGN KEY([TipoMovimiento])
REFERENCES [dbo].[InventarioTipoMovimiento] ([OID])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_TipoMovimiento]
GO

ALTER TABLE [dbo].[Kardex]  WITH NOCHECK ADD  CONSTRAINT [FK_Kardex_Bodega] FOREIGN KEY([Bodega])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Kardex] CHECK CONSTRAINT [FK_Kardex_Bodega]
GO
ALTER TABLE [dbo].[Kardex]  WITH NOCHECK ADD  CONSTRAINT [FK_Kardex_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Kardex] CHECK CONSTRAINT [FK_Kardex_Producto]
GO
ALTER TABLE [dbo].[Medicamento]  WITH NOCHECK ADD  CONSTRAINT [FK_Medicamento_OID] FOREIGN KEY([OID])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Medicamento] CHECK CONSTRAINT [FK_Medicamento_OID]
GO
ALTER TABLE [dbo].[MedicamentoDosis]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicamentoDosis_Medicamento] FOREIGN KEY([Medicamento])
REFERENCES [dbo].[Medicamento] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicamentoDosis] CHECK CONSTRAINT [FK_MedicamentoDosis_Medicamento]
GO
ALTER TABLE [dbo].[MedicamentoVia]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicamentoVia_Medicamento] FOREIGN KEY([Medicamento])
REFERENCES [dbo].[Medicamento] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicamentoVia] CHECK CONSTRAINT [FK_MedicamentoVia_Medicamento]
GO
ALTER TABLE [dbo].[MedicamentoVia]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicamentoVia_Via] FOREIGN KEY([Via])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicamentoVia] CHECK CONSTRAINT [FK_MedicamentoVia_Via]
GO
ALTER TABLE [dbo].[Medico-Citas]  WITH NOCHECK ADD  CONSTRAINT [FK_Medico-Citas_Citas] FOREIGN KEY([Citas])
REFERENCES [dbo].[Cita] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Medico-Citas] CHECK CONSTRAINT [FK_Medico-Citas_Citas]
GO
ALTER TABLE [dbo].[Medico-Citas]  WITH NOCHECK ADD  CONSTRAINT [FK_Medico-Citas_Medicos] FOREIGN KEY([Medicos])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Medico-Citas] CHECK CONSTRAINT [FK_Medico-Citas_Medicos]
GO
ALTER TABLE [dbo].[MedicoConsultorio]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicoConsultorio_Consultorio] FOREIGN KEY([Consultorio])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicoConsultorio] CHECK CONSTRAINT [FK_MedicoConsultorio_Consultorio]
GO
ALTER TABLE [dbo].[MedicoConsultorio]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicoConsultorio_Medico] FOREIGN KEY([Medico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicoConsultorio] CHECK CONSTRAINT [FK_MedicoConsultorio_Medico]
GO
ALTER TABLE [dbo].[MedicoEspecialidad]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicoEspecialidad_Especialidad] FOREIGN KEY([Especialidad])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicoEspecialidad] CHECK CONSTRAINT [FK_MedicoEspecialidad_Especialidad]
GO
ALTER TABLE [dbo].[MedicoEspecialidad]  WITH NOCHECK ADD  CONSTRAINT [FK_MedicoEspecialidad_Medico] FOREIGN KEY([Medico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MedicoEspecialidad] CHECK CONSTRAINT [FK_MedicoEspecialidad_Medico]
GO
ALTER TABLE [dbo].[ModelDifferenceAspect]  WITH NOCHECK ADD  CONSTRAINT [FK_ModelDifferenceAspect_Owner] FOREIGN KEY([Owner])
REFERENCES [dbo].[ModelDifference] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ModelDifferenceAspect] CHECK CONSTRAINT [FK_ModelDifferenceAspect_Owner]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_Aseguradora] FOREIGN KEY([Aseguradora])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Aseguradora]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Empresa]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_Nacionalidad] FOREIGN KEY([Nacionalidad])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Nacionalidad]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_OID] FOREIGN KEY([OID])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_OID]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_Patrono] FOREIGN KEY([Patrono])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Patrono]
GO
ALTER TABLE [dbo].[Paciente]  WITH NOCHECK ADD  CONSTRAINT [FK_Paciente_TipoSeguro] FOREIGN KEY([TipoSeguro])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_TipoSeguro]
GO
ALTER TABLE [dbo].[PacienteFileData]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteFileData_Categoria] FOREIGN KEY([Categoria])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteFileData] CHECK CONSTRAINT [FK_PacienteFileData_Categoria]
GO
ALTER TABLE [dbo].[PacienteFileData]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteFileData_File] FOREIGN KEY([File])
REFERENCES [dbo].[FileData] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteFileData] CHECK CONSTRAINT [FK_PacienteFileData_File]
GO
ALTER TABLE [dbo].[PacienteFileData]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteFileData_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteFileData] CHECK CONSTRAINT [FK_PacienteFileData_Paciente]
GO
ALTER TABLE [dbo].[PacienteMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteMedico_Medico] FOREIGN KEY([Medico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteMedico] CHECK CONSTRAINT [FK_PacienteMedico_Medico]
GO
ALTER TABLE [dbo].[PacienteMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteMedico_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteMedico] CHECK CONSTRAINT [FK_PacienteMedico_Paciente]
GO
ALTER TABLE [dbo].[PacienteVacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteVacunas_Farmaceutica] FOREIGN KEY([Farmaceutica])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteVacuna] CHECK CONSTRAINT [FK_PacienteVacunas_Farmaceutica]
GO
ALTER TABLE [dbo].[PacienteVacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteVacunas_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteVacuna] CHECK CONSTRAINT [FK_PacienteVacunas_Paciente]
GO
ALTER TABLE [dbo].[PacienteVacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteVacunas_ParteDeCuerpo] FOREIGN KEY([ParteDeCuerpo])
REFERENCES [dbo].[TerminologiaAnatomica] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteVacuna] CHECK CONSTRAINT [FK_PacienteVacunas_ParteDeCuerpo]
GO
ALTER TABLE [dbo].[PacienteVacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_PacienteVacunas_Vacuna] FOREIGN KEY([Vacuna])
REFERENCES [dbo].[Medicamento] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PacienteVacuna] CHECK CONSTRAINT [FK_PacienteVacunas_Vacuna]
GO
ALTER TABLE [dbo].[Pariente]  WITH NOCHECK ADD  CONSTRAINT [FK_Pariente_Diagnostico] FOREIGN KEY([Diagnostico])
REFERENCES [dbo].[Enfermedad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Pariente] CHECK CONSTRAINT [FK_Pariente_Diagnostico]
GO
ALTER TABLE [dbo].[Pariente]  WITH NOCHECK ADD  CONSTRAINT [FK_Pariente_IdPariente] FOREIGN KEY([IdPariente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Pariente] CHECK CONSTRAINT [FK_Pariente_IdPariente]
GO
ALTER TABLE [dbo].[Pariente]  WITH NOCHECK ADD  CONSTRAINT [FK_Pariente_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Pariente] CHECK CONSTRAINT [FK_Pariente_Paciente]
GO
ALTER TABLE [dbo].[Pariente]  WITH NOCHECK ADD  CONSTRAINT [FK_Pariente_Parentesco] FOREIGN KEY([Parentesco])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Pariente] CHECK CONSTRAINT [FK_Pariente_Parentesco]
GO
ALTER TABLE [dbo].[PercentilTabla]  WITH CHECK ADD  CONSTRAINT [FKPercentilTablaSigno] FOREIGN KEY([Signo])
REFERENCES [dbo].[Signo] ([OID])
GO
ALTER TABLE [dbo].[PercentilTabla] CHECK CONSTRAINT [FKPercentilTablaSigno]
GO
ALTER TABLE [dbo].[PercentilTablaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_PercentilTablaDetalle_PercentilTabla] FOREIGN KEY([PercentilTabla])
REFERENCES [dbo].[PercentilTabla] ([OID])
GO
ALTER TABLE [dbo].[PercentilTablaDetalle] CHECK CONSTRAINT [FK_PercentilTablaDetalle_PercentilTabla]
GO
ALTER TABLE [dbo].[PermissionPolicyActionPermissionObject]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyActionPermissionObject_Role] FOREIGN KEY([Role])
REFERENCES [dbo].[PermissionPolicyRole] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyActionPermissionObject] CHECK CONSTRAINT [FK_PermissionPolicyActionPermissionObject_Role]
GO
ALTER TABLE [dbo].[PermissionPolicyMemberPermissionsObject]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject] FOREIGN KEY([TypePermissionObject])
REFERENCES [dbo].[PermissionPolicyTypePermissionsObject] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyMemberPermissionsObject] CHECK CONSTRAINT [FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject]
GO
ALTER TABLE [dbo].[PermissionPolicyNavigationPermissionsObject]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyNavigationPermissionsObject_Role] FOREIGN KEY([Role])
REFERENCES [dbo].[PermissionPolicyRole] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyNavigationPermissionsObject] CHECK CONSTRAINT [FK_PermissionPolicyNavigationPermissionsObject_Role]
GO
ALTER TABLE [dbo].[PermissionPolicyObjectPermissionsObject]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject] FOREIGN KEY([TypePermissionObject])
REFERENCES [dbo].[PermissionPolicyTypePermissionsObject] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyObjectPermissionsObject] CHECK CONSTRAINT [FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject]
GO
ALTER TABLE [dbo].[PermissionPolicyRole]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyRole_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyRole] CHECK CONSTRAINT [FK_PermissionPolicyRole_ObjectType]
GO
ALTER TABLE [dbo].[PermissionPolicyTypePermissionsObject]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyTypePermissionsObject_Role] FOREIGN KEY([Role])
REFERENCES [dbo].[PermissionPolicyRole] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyTypePermissionsObject] CHECK CONSTRAINT [FK_PermissionPolicyTypePermissionsObject_Role]
GO
ALTER TABLE [dbo].[PermissionPolicyUser]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyUser_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyUser] CHECK CONSTRAINT [FK_PermissionPolicyUser_ObjectType]
GO
ALTER TABLE [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Roles] FOREIGN KEY([Roles])
REFERENCES [dbo].[PermissionPolicyRole] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles] CHECK CONSTRAINT [FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Roles]
GO
ALTER TABLE [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Users] FOREIGN KEY([Users])
REFERENCES [dbo].[PermissionPolicyUser] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles] CHECK CONSTRAINT [FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Users]
GO
ALTER TABLE [dbo].[Persona]  WITH NOCHECK ADD  CONSTRAINT [FK_Persona_Ciudad] FOREIGN KEY([Ciudad])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Ciudad]
GO
ALTER TABLE [dbo].[Persona]  WITH NOCHECK ADD  CONSTRAINT [FK_Persona_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_ObjectType]
GO
ALTER TABLE [dbo].[Persona]  WITH NOCHECK ADD  CONSTRAINT [FK_Persona_Pais] FOREIGN KEY([Pais])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Pais]
GO
ALTER TABLE [dbo].[Persona]  WITH NOCHECK ADD  CONSTRAINT [FK_Persona_Provincia] FOREIGN KEY([Provincia])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_Provincia]
GO
ALTER TABLE [dbo].[Persona]  WITH NOCHECK ADD  CONSTRAINT [FK_Persona_TipoSangre] FOREIGN KEY([TipoSangre])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Persona] CHECK CONSTRAINT [FK_Persona_TipoSangre]
GO
ALTER TABLE [dbo].[PersonaContacto]  WITH NOCHECK ADD  CONSTRAINT [FK_PersonaContacto_Contacto] FOREIGN KEY([Contacto])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PersonaContacto] CHECK CONSTRAINT [FK_PersonaContacto_Contacto]
GO
ALTER TABLE [dbo].[PersonaContacto]  WITH NOCHECK ADD  CONSTRAINT [FK_PersonaContacto_Persona] FOREIGN KEY([Persona])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PersonaContacto] CHECK CONSTRAINT [FK_PersonaContacto_Persona]
GO
ALTER TABLE [dbo].[PersonaDocumento]  WITH NOCHECK ADD  CONSTRAINT [FK_PersonaDocumento_Persona] FOREIGN KEY([Persona])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PersonaDocumento] CHECK CONSTRAINT [FK_PersonaDocumento_Persona]
GO
ALTER TABLE [dbo].[PersonaDocumento]  WITH NOCHECK ADD  CONSTRAINT [FK_PersonaDocumento_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PersonaDocumento] CHECK CONSTRAINT [FK_PersonaDocumento_Tipo]
GO
ALTER TABLE [dbo].[PersonaTelefono]  WITH NOCHECK ADD  CONSTRAINT [FK_PersonaTelefono_Persona] FOREIGN KEY([Persona])
REFERENCES [dbo].[Persona] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PersonaTelefono] CHECK CONSTRAINT [FK_PersonaTelefono_Persona]
GO
ALTER TABLE [dbo].[PersonaTelefono]  WITH CHECK ADD  CONSTRAINT [FK_PersonaTelefono_Telefono] FOREIGN KEY([Telefono])
REFERENCES [dbo].[Telefono] ([Numero])
GO
ALTER TABLE [dbo].[PersonaTelefono] CHECK CONSTRAINT [FK_PersonaTelefono_Telefono]
GO
ALTER TABLE [dbo].[PlanMedicoDetalle]  WITH NOCHECK ADD  CONSTRAINT [FK_PlanMedicoDetalle_PlanMedico] FOREIGN KEY([PlanMedico])
REFERENCES [dbo].[PlanMedico] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PlanMedicoDetalle] CHECK CONSTRAINT [FK_PlanMedicoDetalle_PlanMedico]
GO
ALTER TABLE [dbo].[PlanMedicoDetalle]  WITH NOCHECK ADD  CONSTRAINT [FK_PlanMedicoDetalle_Regla] FOREIGN KEY([Regla])
REFERENCES [dbo].[Regla] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PlanMedicoDetalle] CHECK CONSTRAINT [FK_PlanMedicoDetalle_Regla]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Diagnostico] FOREIGN KEY([Diagnostico])
REFERENCES [dbo].[Enfermedad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Diagnostico]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Frecuencia] FOREIGN KEY([Frecuencia])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Frecuencia]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Gravedad] FOREIGN KEY([Gravedad])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Gravedad]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Paciente]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Reaccion] FOREIGN KEY([Reaccion])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Reaccion]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Resultado] FOREIGN KEY([Resultado])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Resultado]
GO
ALTER TABLE [dbo].[ProblemaMedico]  WITH NOCHECK ADD  CONSTRAINT [FK_ProblemaMedico_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProblemaMedico] CHECK CONSTRAINT [FK_ProblemaMedico_Tipo]
GO
ALTER TABLE [dbo].[ProCategoria]  WITH NOCHECK ADD  CONSTRAINT [FK_GloCategoriaProducto_Padre] FOREIGN KEY([Padre])
REFERENCES [dbo].[ProCategoria] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProCategoria] CHECK CONSTRAINT [FK_GloCategoriaProducto_Padre]
GO
ALTER TABLE [dbo].[Producto]  WITH NOCHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([Categoria])
REFERENCES [dbo].[ProCategoria] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]
GO
ALTER TABLE [dbo].[Producto]  WITH NOCHECK ADD  CONSTRAINT [FK_Producto_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Empresa]
GO
ALTER TABLE [dbo].[Producto]  WITH NOCHECK ADD  CONSTRAINT [FK_Producto_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_ObjectType]
GO
ALTER TABLE [dbo].[Producto]  WITH NOCHECK ADD  CONSTRAINT [FK_Producto_Presentacion] FOREIGN KEY([Presentacion])
REFERENCES [dbo].[ProPresentacion] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Presentacion]
GO
ALTER TABLE [dbo].[ProductoAtributo]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoAtributo_Atributo] FOREIGN KEY([Atributo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoAtributo] CHECK CONSTRAINT [FK_ProductoAtributo_Atributo]
GO
ALTER TABLE [dbo].[ProductoAtributo]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoAtributo_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoAtributo] CHECK CONSTRAINT [FK_ProductoAtributo_Producto]
GO
ALTER TABLE [dbo].[ProductoCodigoBarra]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoCodigoBarra_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoCodigoBarra] CHECK CONSTRAINT [FK_ProductoCodigoBarra_Producto]
GO
ALTER TABLE [dbo].[ProductoEnsamble]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoEnsamble_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoEnsamble] CHECK CONSTRAINT [FK_ProductoEnsamble_Item]
GO
ALTER TABLE [dbo].[ProductoEnsamble]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoEnsamble_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoEnsamble] CHECK CONSTRAINT [FK_ProductoEnsamble_Producto]
GO
ALTER TABLE [dbo].[ProductoEquivalente]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoEquivalente_Equivalente] FOREIGN KEY([Equivalente])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoEquivalente] CHECK CONSTRAINT [FK_ProductoEquivalente_Equivalente]
GO
ALTER TABLE [dbo].[ProductoEquivalente]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoEquivalente_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoEquivalente] CHECK CONSTRAINT [FK_ProductoEquivalente_Producto]
GO
ALTER TABLE [dbo].[InventarioLote]  WITH NOCHECK ADD  CONSTRAINT [FK_InventarioLote_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[InventarioLote] CHECK CONSTRAINT [FK_InventarioLote_Producto]
GO
ALTER TABLE [dbo].[ProductoPrecio]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoPrecio_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoPrecio] CHECK CONSTRAINT [FK_ProductoPrecio_Producto]
GO
ALTER TABLE [dbo].[ProductoPrecio]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoPrecio_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoPrecio] CHECK CONSTRAINT [FK_ProductoPrecio_Tipo]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoProveedor_Fabricante] FOREIGN KEY([Fabricante])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_ProductoProveedor_Fabricante]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoProveedor_PaisOrigen] FOREIGN KEY([PaisOrigen])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_ProductoProveedor_PaisOrigen]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoProveedor_Producto] FOREIGN KEY([Producto])
REFERENCES [dbo].[Producto] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_ProductoProveedor_Producto]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH NOCHECK ADD  CONSTRAINT [FK_ProductoProveedor_Proveedor] FOREIGN KEY([Proveedor])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_ProductoProveedor_Proveedor]
GO
ALTER TABLE [dbo].[RecordatorioClinico]  WITH NOCHECK ADD  CONSTRAINT [FK_RecordatorioClinico_Paciente] FOREIGN KEY([Paciente])
REFERENCES [dbo].[Paciente] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[RecordatorioClinico] CHECK CONSTRAINT [FK_RecordatorioClinico_Paciente]
GO
ALTER TABLE [dbo].[RecordatorioClinico]  WITH NOCHECK ADD  CONSTRAINT [FK_RecordatorioClinico_Regla] FOREIGN KEY([Regla])
REFERENCES [dbo].[Regla] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[RecordatorioClinico] CHECK CONSTRAINT [FK_RecordatorioClinico_Regla]
GO
ALTER TABLE [dbo].[ResourceResources_EventEvents]  WITH NOCHECK ADD  CONSTRAINT [FK_ResourceResources_EventEvents_Events] FOREIGN KEY([Events])
REFERENCES [dbo].[Event] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ResourceResources_EventEvents] CHECK CONSTRAINT [FK_ResourceResources_EventEvents_Events]
GO
ALTER TABLE [dbo].[ResourceResources_EventEvents]  WITH NOCHECK ADD  CONSTRAINT [FK_ResourceResources_EventEvents_Resources] FOREIGN KEY([Resources])
REFERENCES [dbo].[Resource] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ResourceResources_EventEvents] CHECK CONSTRAINT [FK_ResourceResources_EventEvents_Resources]
GO
ALTER TABLE [dbo].[SysAuditoriaProceso]  WITH NOCHECK ADD  CONSTRAINT [FK_SysAuditoriaProceso_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[SysAuditoriaProceso] CHECK CONSTRAINT [FK_SysAuditoriaProceso_Empresa]
GO
ALTER TABLE [dbo].[Tercero]  WITH NOCHECK ADD  CONSTRAINT [FK_Tercero_DireccionPrincipal] FOREIGN KEY([DireccionPrincipal])
REFERENCES [dbo].[TerceroDireccion] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Tercero] CHECK CONSTRAINT [FK_Tercero_DireccionPrincipal]
GO
ALTER TABLE [dbo].[Tercero]  WITH NOCHECK ADD  CONSTRAINT [FK_Tercero_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Tercero] CHECK CONSTRAINT [FK_Tercero_ObjectType]
GO
ALTER TABLE [dbo].[TerceroContacto]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroContacto_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroContacto] CHECK CONSTRAINT [FK_TerceroContacto_Tercero]
GO
ALTER TABLE [dbo].[TerceroCredito]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroCredito_Cliente] FOREIGN KEY([Cliente])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroCredito] CHECK CONSTRAINT [FK_TerceroCredito_Cliente]
GO
ALTER TABLE [dbo].[TerceroCredito]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroCredito_DireccionCobro] FOREIGN KEY([DireccionCobro])
REFERENCES [dbo].[TerceroDireccion] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroCredito] CHECK CONSTRAINT [FK_TerceroCredito_DireccionCobro]
GO
ALTER TABLE [dbo].[TerceroCredito]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroCredito_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroCredito] CHECK CONSTRAINT [FK_TerceroCredito_Empresa]
GO
ALTER TABLE [dbo].[TerceroDireccion]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDireccion_Ciudad] FOREIGN KEY([Ciudad])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDireccion] CHECK CONSTRAINT [FK_TerceroDireccion_Ciudad]
GO
ALTER TABLE [dbo].[TerceroDireccion]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDireccion_Pais] FOREIGN KEY([Pais])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDireccion] CHECK CONSTRAINT [FK_TerceroDireccion_Pais]
GO
ALTER TABLE [dbo].[TerceroDireccion]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDireccion_Provincia] FOREIGN KEY([Provincia])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDireccion] CHECK CONSTRAINT [FK_TerceroDireccion_Provincia]
GO
ALTER TABLE [dbo].[TerceroDireccion]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDireccion_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDireccion] CHECK CONSTRAINT [FK_TerceroDireccion_Tercero]
GO
ALTER TABLE [dbo].[TerceroDocumento]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDocumento_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDocumento] CHECK CONSTRAINT [FK_TerceroDocumento_Tercero]
GO
ALTER TABLE [dbo].[TerceroDocumento]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroDocumento_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroDocumento] CHECK CONSTRAINT [FK_TerceroDocumento_Tipo]
GO
ALTER TABLE [dbo].[TerceroGarantia]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroGarantia_Cliente] FOREIGN KEY([Cliente])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroGarantia] CHECK CONSTRAINT [FK_TerceroGarantia_Cliente]
GO
ALTER TABLE [dbo].[TerceroGarantia]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroGarantia_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroGarantia] CHECK CONSTRAINT [FK_TerceroGarantia_Empresa]
GO
ALTER TABLE [dbo].[TerceroGarantia]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroGarantia_Tipo] FOREIGN KEY([Tipo])
REFERENCES [dbo].[Listas] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroGarantia] CHECK CONSTRAINT [FK_TerceroGarantia_Tipo]
GO
ALTER TABLE [dbo].[TerceroGiro]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroGiro_ActEconomica] FOREIGN KEY([ActEconomica])
REFERENCES [dbo].[ActividadEconomica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroGiro] CHECK CONSTRAINT [FK_TerceroGiro_ActEconomica]
GO
ALTER TABLE [dbo].[TerceroGiro]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroGiro_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroGiro] CHECK CONSTRAINT [FK_TerceroGiro_Tercero]
GO
ALTER TABLE [dbo].[TerceroNota]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroNota_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroNota] CHECK CONSTRAINT [FK_TerceroNota_Tercero]
GO
ALTER TABLE [dbo].[TerceroRole]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroRole_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroRole] CHECK CONSTRAINT [FK_TerceroRole_Tercero]
GO
ALTER TABLE [dbo].[TerceroSucursal]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroSucursal_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroSucursal] CHECK CONSTRAINT [FK_TerceroSucursal_Tercero]
GO
ALTER TABLE [dbo].[TerceroTelefono]  WITH CHECK ADD  CONSTRAINT [FK_TerceroTelefono_Telefono] FOREIGN KEY([Telefono])
REFERENCES [dbo].[Telefono] ([Numero])
GO
ALTER TABLE [dbo].[TerceroTelefono] CHECK CONSTRAINT [FK_TerceroTelefono_Telefono]
GO
ALTER TABLE [dbo].[TerceroTelefono]  WITH NOCHECK ADD  CONSTRAINT [FK_TerceroTelefono_Tercero] FOREIGN KEY([Tercero])
REFERENCES [dbo].[Tercero] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerceroTelefono] CHECK CONSTRAINT [FK_TerceroTelefono_Tercero]
GO
ALTER TABLE [dbo].[TerminologiaAnatomica]  WITH NOCHECK ADD  CONSTRAINT [FK_TerminologiaAnatomica_Categoria] FOREIGN KEY([Categoria])
REFERENCES [dbo].[MedLista] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[TerminologiaAnatomica] CHECK CONSTRAINT [FK_TerminologiaAnatomica_Categoria]
GO
ALTER TABLE [dbo].[UltraSonografiaObstetrica]  WITH NOCHECK ADD  CONSTRAINT [FK_UltraSonografiaObstetrica_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[UltraSonografiaObstetrica] CHECK CONSTRAINT [FK_UltraSonografiaObstetrica_Consulta]
GO
ALTER TABLE [dbo].[UltraSonografiaObstetrica]  WITH NOCHECK ADD  CONSTRAINT [FK_UltraSonografiaObstetrica_Tecnico] FOREIGN KEY([Tecnico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[UltraSonografiaObstetrica] CHECK CONSTRAINT [FK_UltraSonografiaObstetrica_Tecnico]
GO
ALTER TABLE [dbo].[UltrasonografiaObstetricaDetalle]  WITH NOCHECK ADD  CONSTRAINT [FK_UltrasonografiaObstetricaDetalle_Ultrasonografia] FOREIGN KEY([Ultrasonografia])
REFERENCES [dbo].[UltraSonografiaObstetrica] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[UltrasonografiaObstetricaDetalle] CHECK CONSTRAINT [FK_UltrasonografiaObstetricaDetalle_Ultrasonografia]
GO
ALTER TABLE [dbo].[UltrasonografiaPelvica]  WITH NOCHECK ADD  CONSTRAINT [FK_UltrasonografiaPelvica_Consulta] FOREIGN KEY([Consulta])
REFERENCES [dbo].[Consulta] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[UltrasonografiaPelvica] CHECK CONSTRAINT [FK_UltrasonografiaPelvica_Consulta]
GO
ALTER TABLE [dbo].[UltrasonografiaPelvica]  WITH NOCHECK ADD  CONSTRAINT [FK_UltrasonografiaPelvica_Tecnico] FOREIGN KEY([Tecnico])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[UltrasonografiaPelvica] CHECK CONSTRAINT [FK_UltrasonografiaPelvica_Tecnico]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [FK_Usuario_Agencia] FOREIGN KEY([Agencia])
REFERENCES [dbo].[EmpresaUnidad] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Agencia]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [FK_Usuario_Empleado] FOREIGN KEY([Empleado])
REFERENCES [dbo].[Empleado] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Empleado]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [FK_Usuario_Empresa] FOREIGN KEY([Empresa])
REFERENCES [dbo].[Empresa] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Empresa]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [FK_Usuario_Oid] FOREIGN KEY([Oid])
REFERENCES [dbo].[PermissionPolicyUser] ([Oid])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Oid]
GO
ALTER TABLE [dbo].[Vacuna]  WITH NOCHECK ADD  CONSTRAINT [FK_Vacuna_Medicamento] FOREIGN KEY([Medicamento])
REFERENCES [dbo].[Medicamento] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Vacuna] CHECK CONSTRAINT [FK_Vacuna_Medicamento]
GO
ALTER TABLE [dbo].[XPWeakReference]  WITH NOCHECK ADD  CONSTRAINT [FK_XPWeakReference_ObjectType] FOREIGN KEY([ObjectType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[XPWeakReference] CHECK CONSTRAINT [FK_XPWeakReference_ObjectType]
GO
ALTER TABLE [dbo].[XPWeakReference]  WITH NOCHECK ADD  CONSTRAINT [FK_XPWeakReference_TargetType] FOREIGN KEY([TargetType])
REFERENCES [dbo].[XPObjectType] ([OID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[XPWeakReference] CHECK CONSTRAINT [FK_XPWeakReference_TargetType]
GO
ALTER TABLE [dbo].[ZonaGeografica]  WITH NOCHECK ADD  CONSTRAINT [FK_ZonaGeografica_Moneda] FOREIGN KEY([Moneda])
REFERENCES [dbo].[Moneda] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ZonaGeografica] CHECK CONSTRAINT [FK_ZonaGeografica_Moneda]
GO
ALTER TABLE [dbo].[ZonaGeografica]  WITH NOCHECK ADD  CONSTRAINT [FK_ZonaGeografica_ZonaPadre] FOREIGN KEY([ZonaPadre])
REFERENCES [dbo].[ZonaGeografica] ([Codigo])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[ZonaGeografica] CHECK CONSTRAINT [FK_ZonaGeografica_ZonaPadre]
GO
