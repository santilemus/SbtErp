USE [Medico]
GO
drop table RecordatorioClinico
go
drop table ProblemaMedico
go
drop table HistoriaFamiliar
go
drop table HistorialCrecimiento
go
drop table EstiloVida
go
drop table PersonaContacto
go
drop table PersonaDocumento
go
drop table PersonaTelefono
go
drop table Pariente 
go
drop table Paciente
go
drop table Persona
go
drop table Profesion
go
drop table gloListas
go

/****** Object:  Table [dbo].[gloListas]    Script Date: 1/11/2019 14:25:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[gloListas](
    Oid int identity(1,1) not null,
	[Codigo] [nvarchar](12) NOT NULL,
	[Nombre] [nvarchar](100) NULL,
	[Categoria] [int] NULL,
	[Comentario] [nvarchar](250) NULL,
	[Activo] [bit] NULL,
	[UsuarioCrea] [varchar](25) NULL,
	[FechaCrea] [datetime] NULL,
	[UsuarioMod] [varchar](25) NULL,
	[FechaMod] [datetime] NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PK_gloListas] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


