USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spConGeneraSaldosMes]    Script Date: 12/11/2021 16:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 28/02/2020
-- Description:	Generar los saldos mensuales por cuenta y mes a partir del cierre diario, para la 
--              aplicacion: SBT.Erp, modulo Contabilidad. Se generan desde la tabla ConSaldosMes,
--              los estado financieros y los comparativos <pivot table>
-- NOTA       : Migrado desde el ERP SIAF
-- ================================================================================================
ALTER   procedure [dbo].[spConGeneraSaldosMes] (
        @Empresa     int,
        @FechaHasta  datetime,
		@Usuario     varchar(25))
as
begin
  declare @mesanio int = cast(ltrim(rtrim(cast(@Empresa as varchar(2)))) + Format(@FechaHasta, 'MMyyyy')  as int)
  declare @iMes smallint = month(@FechaHasta)
  declare @fechaDesde datetime = cast(dateadd(dd, -day(@FechaHasta) + 1, @FechaHasta) as date) 
  declare @error_msg nvarchar(4000)
  declare @error_sev int;  
  
  begin try
    begin tran t0 
	update m
	   set SaldoInicio = 0.0,
	       Debe = 0.0,
		   Haber = 0.0,
		   SaldoFin = 0.0
	  from ConSaldoMes m
	 inner join ConCatalogo c
	    on m.Cuenta = c.Oid 
	 where c.Empresa = @Empresa 
	   and m.Periodo = Year(@FechaHasta)
	   and m.Mes = Month(@FechaHasta)
	commit tran t0
  end try
  begin catch
    rollback tran t0
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 

  begin try
    begin tran t1 
	  merge ConSaldoMes as destino
	    using (select c.Empresa, s.Periodo, s.Cuenta, @MesAnio, dbo.fnConSaldoCuentaOid(c.Empresa, @FechaDesde, s.Cuenta),
		              dbo.fnConCargosCuentaMes(s.Periodo, @iMes, s.Cuenta), dbo.fnConAbonosCuentaMes(s.Periodo, @iMes, s.Cuenta),
					  sum(iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0), coalesce(s.Haber, 0) - coalesce(s.Debe, 0)))
		         from ConSaldoDiario s
				inner join ConCatalogo c
				   on s.Cuenta = c.OID
		        where c.Empresa = @Empresa
		          and s.Periodo = year(@FechaHasta)
		          and s.Fecha <= @FechaHasta
		          and s.TipoSaldoDia <= 1 -- no considera de liquidacion = 2 y de cierre = 3
		        group by c.Empresa, s.Periodo, s.Cuenta) as origen
			  (Empresa, Periodo, Cuenta, MesAnio, SaldoInicioMes, Debe, Haber, SaldoFin)
		    on (destino.MesAnio  = origen.MesAnio
		   and destino.Cuenta   = origen.Cuenta)
		  when matched then
		       update set
			          SaldoInicio = origen.SaldoInicioMes,
					  Debe = origen.Debe,
					  Haber = origen.Haber,
					  SaldoFin = origen.SaldoFin
		  when not matched by target then
		       insert (Periodo, MesAnio, Cuenta, Mes, SaldoInicio, Debe, Haber, SaldoFin)
			   values (Origen.Periodo, Origen.MesAnio, origen.Cuenta, month(@FechaHasta), origen.SaldoInicioMes, 
			           origen.Debe, origen.Haber, origen.SaldoFin);
	commit tran t1
  end try
  begin catch
    rollback tran t1
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
end
