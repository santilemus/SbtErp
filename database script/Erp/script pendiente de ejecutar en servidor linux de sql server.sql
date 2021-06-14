USE [Sbt_Erp]
GO

/****** Object:  Index [idxCodigoCuenta_Catalogo]    Script Date: 22/5/2021 18:19:50 ******/
DROP INDEX [idxCodigoCuenta_Catalogo] ON [dbo].[ConCatalogo]
GO
USE [Sbt_Erp]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [idxCodigoCuenta_Catalogo]    Script Date: 22/5/2021 18:20:10 ******/
CREATE UNIQUE NONCLUSTERED INDEX [idxCodigoCuenta_Catalogo] ON [dbo].[ConCatalogo]
(
    [Empresa] asc,
	[CodigoCuenta] ASC
)
GO

/****** Object:  Index [idx_MesAnio_Cuenta]    Script Date: 22/5/2021 18:21:22 ******/
DROP INDEX [idx_MesAnio_Cuenta] ON [dbo].[ConSaldoMes]
GO

/****** Object:  Index [idx_MesAnio_Cuenta]    Script Date: 22/5/2021 18:21:36 ******/
CREATE unique NONCLUSTERED INDEX [idxMesAnioCuenta_SaldoMes] ON [dbo].[ConSaldoMes]
(
	[MesAnio] ASC,
	[Cuenta] ASC
)
GO

alter table ActividadEconomica
 drop column FechaCrea
go
alter table ActividadEconomica
 drop column UsuarioCrea
go
alter table ActividadEconomica
 drop column FechaMod
go
alter table ActividadEconomica
 drop column UsuarioMod
go
/*
sp_help AFI_CATALOGO
select * from PAR_LISTAS
*/

select * from Listas

select * from db_siaf..AFI_DEPRECIACION
 where year(fecha_depre) >= 2015