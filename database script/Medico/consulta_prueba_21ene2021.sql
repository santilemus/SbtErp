USE [Medico]
GO

/****** Object:  Table [dbo].[WhoPercentilPesoLong]    Script Date: 17/1/2021 13:53:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PercentilTabla](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Genero]  [smallint] null,
	[Nombre] varchar(60) null,
	[Signo] [int] null,
CONSTRAINT [PK_PercentilTabla] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
))

go
alter table PercentilTabla
  add constraint FKPercentilTablaSigno foreign key(Signo) references signo(Oid)
go

CREATE TABLE [dbo].[PercentilTablaDetalle](
	[OID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[PercentilTabla] [int] not null,
	[EdadMes] [smallint] NULL,
	[Lambda] [numeric](8, 5) NULL,
	[Mu] [numeric](8, 5) NULL,
	[Sigma] [numeric](8, 5) NULL,
	[DesviacionStandard] [numeric](8,5) null,
	[P1st][numeric](8,5) null,
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
 CONSTRAINT [PK_PercentilTablaDetalle] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

alter table PercentilTablaDetalle
  add constraint FK_PercentilTablaDetalle_PercentilTabla foreign key(PercentilTabla) references PercentilTabla(Oid)
go

USE [Medico]
GO

alter table ConsultaSigno
  add PercentilTablaDetalle int null
go
alter table ConsultaSigno
  drop column PercentilWho
go

ALTER TABLE [dbo].[ConsultaSigno]  WITH NOCHECK ADD  CONSTRAINT [FK_ConsultaSigno_PercentilTablaDetalle] FOREIGN KEY([PercentilTablaDetalle])
REFERENCES [dbo].[PercentilTablaDetalle] ([OID])
NOT FOR REPLICATION 
GO




--select * from Signo


alter table ConsultaSigno
  drop constraint FK_ConsultaSigno_PercentilWho
go

alter table ConsultaSigno
 drop column PercentilWho
go


select * from TablaIMC

select * from Signo
select * from PercentilTabla
select * from PercentilTablaDetalle
 where PercentilTabla = 1

select * from TablaIMC


select * from XPObjectType
 where Oid > 37

set identity_insert dbo.XPObjectType on
go
update XPObjectType
   set Oid = Oid - 35
 where OId >= 73
go
set identity_insert dbo.XPObjectType off
go
dbcc checkident('dbo.XPObjectType', reseed, 75)
go