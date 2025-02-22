USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spConGeneraSaldosMes]    Script Date: 30/4/2023 23:46:00 ******/
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
  set nocount on

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
		        where (c.Empresa = @Empresa
		          and s.Periodo = year(@FechaHasta)
		          and s.Fecha <= @FechaHasta)
                   -- las siguientes condiciones solo considera los saldos de Apertura y Operaciones Diarias TipoSaldoDia <= 1
				   -- o cuando el saldo es de liquidacion (TipoSaldoDia = 2) y la cuenta es de Activo, Pasivo, Patrimonio y Perdidas y Ganancias
		          and (s.TipoSaldoDia <= 1 
				   or  (s.TipoSaldoDia = 2 And c.TipoCuenta in (1, 2, 3, 7) And c.CuentaEspecial not in (6, 7)))
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
  -- el siguiente bloque solo cuando hay partida de liquidacion
  begin try
    begin tran t2 
	  declare @OidCta int = 0
	  declare @Saldo decimal(14,2) = 0
	  declare @OidPadre int
	  declare @OidTemp int;
	  declare @EsCtaMayor bit
	  -- Cuando @Bandera es cero y las columnas Debe y Haber tienen valor se ponen a cero desde la cuenta de detalle de la utilidad
	  -- o perdida hasta la cuenta de mayor. En los siguientes niveles se debe respetar el contenido de @Debe y @Haber porque pudo
	  -- generarse en otras subcuentas
	  declare @Bandera bit = 0;

	  select @OidCta = s.Cuenta, @OidPadre = c.CtaPadre, @EsCtaMayor = c.CtaMayor,
	         @Saldo = iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0), coalesce(s.Haber, 0) - coalesce(s.Debe, 0))
		from ConSaldoDiario s
	   inner join ConCatalogo c
		  on s.Cuenta = c.Oid
	   where c.Empresa = @Empresa
		 and s.Periodo = Year(@FechaHasta)
		 and s.TipoSaldoDia = 2         -- Saldos originados en la partida de liquidacion
		 and c.CuentaEspecial in (6, 7) -- UtilidadEjercicio, Perdida del Ejercicio 
	  --print 'Inicio OidCta = ' + cast(@OidCta as varchar(5)) + '    OidPadre = ' + cast(@OidPadre as Varchar(5)) + '    Bandera = ' + cast(@Bandera as varchar(1))
	  while (@@rowcount > 0)
	  begin
	    update m 
		   set m.Debe = iif(@Bandera = 1, m.Debe, 0),  
		       m.Haber = iif(@Bandera = 1, m.Haber, 0),
		       m.SaldoFin = iif(@Bandera = 1, coalesce(m.SaldoFin, 0) + @Saldo, @Saldo)
		  from ConSaldoMes m
		 inner join ConCatalogo c
		    on m.Cuenta = c.Oid
		 where c.Empresa = @Empresa 
		   and Periodo   = Year(@FechaHasta)
		   and MesAnio   = @mesanio
		   and m.Cuenta  = @OidCta
	    if (@@rowcount = 0)
		begin
		  insert into ConSaldoMes
		         (Periodo, MesAnio, Cuenta, Mes, SaldoInicio, Debe, Haber, SaldoFin)
		  values (Year(@FechaHasta), @mesanio, @OidCta, @iMes, 0.0, 0.0, 0.0, @saldo)
        end
		if (@EsCtaMayor = 1)
		   set @Bandera = 1

		set @OidTemp = @OidPadre 
		select @OidCta = c.Oid, @OidPadre = c.CtaPadre, @EsCtaMayor = c.CtaMayor
          from ConCatalogo c
         where c.Oid = @OidTemp
		--print 'entro OidCta = ' + cast(@OidCta as varchar(5)) + '    OidPadre = ' + cast(@OidPadre as Varchar(5)) + '    Bandera = ' + cast(@Bandera as varchar(1))
	  end
	commit tran t2
  end try
  begin catch
    rollback tran t2
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
end
