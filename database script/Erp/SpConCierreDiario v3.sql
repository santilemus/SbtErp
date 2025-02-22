USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spConCierreDiario]    Script Date: 12/11/2021 15:48:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 28/02/2020
-- Description:	Generar el Cierre Diario Contable, para la aplicacion: SBT.Erp, modulo Contabilidad
-- NOTA       : Migrado desde el ERP SIAF
-- ================================================================================================
ALTER   procedure [dbo].[spConCierreDiario](
   @Empresa     int,
   @FechaDesde  datetime,
   @FechaHasta  datetime,
   @Usuario     varchar(25))
as
begin
  set nocount on
  declare @Periodo smallint = Year(@FechaDesde)
  declare @fecha datetime         -- para obtener la fecha de la partida de apertura, liquidacion y cierre
  declare @Cuenta  varchar(20) 
  declare @Debe  money
  declare @Haber money
  declare @nivel smallint  -- para guardar el último nivel de cuentas y comenzar a acumular
  declare @error_msg nvarchar(4000)
  declare @error_sev int;   
  --- ponemos los saldos del rango de fechas a cerrar para evitar conflictos con detalles de partidas que se borraron o cambio la cuenta
  begin try
    begin tran t0
	  update s
	     set s.Debe = 0.0,
	         s.Haber = 0.0,
			 s.DebeAjusteConsolida = 0.0,
			 s.HaberAjusteConsolida = 0.0
		from ConSaldoDiario s
	   inner join ConCatalogo c
	      on s.Cuenta = c.Oid
	   where c.Empresa = @Empresa
	     and s.Periodo = @Periodo
	     and cast(s.Fecha as date) between cast(@FechaDesde as date) and cast(@FechaHasta as Date)
	commit tran t0
  end try
  begin catch
    rollback tran t0
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
  --- procesamos, las partidas
  begin try
    begin tran t1  
        --- procesamos, la partida de apertura (si se encuentra)
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), 0,
		                 sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = @Periodo 
					 and cast(e.Fecha as date)  between cast(@FechaDesde as date) and cast(@FechaHasta as date)
					 and e.Tipo = 0
				   group by e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date)) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Tipo, Debe, Haber)
               on (destino.Cuenta   = origen.Cuenta
			  and  destino.Fecha    = origen.Fecha
			  and  destino.TipoSaldoDia = origen.Tipo)
		     when not matched by target then
			      insert (Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
				  values (origen.Periodo, origen.Cuenta, origen.Fecha, origen.Tipo, origen.Debe, origen.Haber,
				          0, 0)
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber; 
        --- procesamos, la partidas de diario, ingresos y egresos
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), 1,
		                 sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = @Periodo 
					 and cast(e.Fecha as date)  between cast(@FechaDesde as date) and cast(@FechaHasta as date)
					 and e.Tipo in (1, 2, 3)
				   group by e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date)) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Tipo, Debe, Haber)
               on (destino.Cuenta   = origen.Cuenta
			  and  destino.Fecha    = origen.Fecha
			  and  destino.TipoSaldoDia = origen.Tipo)
		     when not matched by target then
			      insert (Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
				  values (origen.Periodo, origen.Cuenta, origen.Fecha, origen.Tipo, origen.Debe, origen.Haber,
				          0, 0)
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber; 	
					
        --- procesamos, la partidas de liquidacion
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), 2 /* liquidacion */,
		                 sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = @Periodo 
					 and cast(e.Fecha as date)  between cast(@FechaDesde as date) and cast(@FechaHasta as date)
					 and e.Tipo = 4
				   group by e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), e.Tipo) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Tipo, Debe, Haber)
               on (destino.Cuenta   = origen.Cuenta
			  and  destino.Fecha    = origen.Fecha
			  and  destino.TipoSaldoDia = origen.Tipo)
		     when not matched by target then
			      insert (Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
				  values (origen.Periodo, origen.Cuenta, origen.Fecha, origen.Tipo, origen.Debe, origen.Haber,
				          0, 0)
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber; 	

        --- procesamos, la partidas de cierre
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), 3 /* cierre */,
		                 sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = @Periodo 
					 and cast(e.Fecha as date)  between cast(@FechaDesde as date) and cast(@FechaHasta as date)
					 and e.Tipo = 5
				   group by e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), e.Tipo) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Tipo, Debe, Haber)
               on (destino.Cuenta   = origen.Cuenta
			  and  destino.Fecha    = origen.Fecha
			  and  destino.TipoSaldoDia = origen.Tipo)
		     when not matched by target then
			      insert (Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
				  values (origen.Periodo, origen.Cuenta, origen.Fecha, origen.Tipo, origen.Debe, origen.Haber,
				          0, 0)
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber; 

	commit tran t1
  end try
  begin catch
    rollback tran t1
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 

  /* insertamos los saldos de las cuentas de detalle cuando son partidas de ajuste por consolidacion */
  begin try
    begin tran t2 
	  merge ConSaldoDiario as destino
	     using (select e.Empresa, e.Periodo, d.Cuenta, cast(e.Fecha as date), sum(coalesce(d.Debe / e.ValorMoneda, 0)), 
					   sum(coalesce(d.Haber / e.ValorMoneda, 0))
				  from ConPartidaDetalle d 
			     inner join ConPartida e 
				    on d.Partida = e.Oid
			     where e.Empresa = @Empresa   
				   and e.Periodo = Year(@FechaDesde) 
				   and cast(e.Fecha as date) between cast(@FechaDesde as date) and cast(@FechaHasta as date)
				   and d.AjusteConsolida = 1
			     group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
		       (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
            on (destino.periodo = origen.Periodo
		   and  destino.Fecha   = origen.Fecha
		   and  destino.Cuenta  = origen.Cuenta
		   and  destino.TipoSaldoDia = 1)   /* saldo de las operaciones diarias del ejercicio */
          when matched then
		       update set
			          HaberAjusteConsolida = origen.Debe,
					  DebeAjusteConsolida  = origen.Haber;
	commit tran t2
  end try
  begin catch
    rollback tran t2
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch  
  /* hacemos cero los saldos de las cuentas de resumen, antes de actualizarlos de nuevo */
   begin try
	 begin tran t3
	   update s
	      set s.Debe = 0.0,
		      s.Haber= 0.0,
			  s.DebeAjusteConsolida = 0.0,
			  s.HaberAjusteConsolida= 0.0
	     from ConSaldoDiario s
		inner join ConCatalogo c
		   on s.Cuenta = c.Oid
	    where c.Empresa = @Empresa
		  and s.Periodo = @Periodo
		  and cast(s.Fecha as date) between cast(@FechaDesde as date) and cast(@FechaHasta as date)
	      and c.CtaResumen = 1  -- solo los saldos de las cuentas de resumen
	   commit tran t3
     end try
   begin catch
      rollback tran t3
      select @error_msg = error_message(), @error_sev = error_severity()
      raiserror(@error_msg, @error_sev, 1)
   end catch  	  
  /* se procede  actualizar los saldos del presupuesto */
  /****  OJO PENDIENTE                               ***/
  --exec SP_PRE_ACTUALIZA_SALDOS @cod_emp, @cod_periodo, @fecha_desde, @fecha_hasta
  /****  OJO PENDIENTE                               ***/

  /* para obtener el nivel más bajo (detalle) de las cuentas del catálogo vigente */
  select @nivel  = max(Nivel) from ConCatalogo
   where Empresa = @Empresa
  /* iteramos por todos los niveles para obtener el saldo acumulado en las cuentas de resumen */
 
  declare @TipoSaldoDia smallint  -- tipo_saldo_dia cuando se acumula en niveles superiores
  declare @DebeAjusteCo  money 
  declare @HaberAjusteCo money 
  declare @filas int;
  while (@nivel > 1)
  begin
    begin try
      begin tran t4  
	    merge ConSaldoDiario as Destino
		   using (select c.Empresa, s.Periodo, c.CtaPadre, s.Fecha, s.TipoSaldoDia,
						 sum(coalesce(s.Debe, 0)), sum(coalesce(s.Haber, 0)), 
						 sum(coalesce(s.DebeAjusteConsolida, 0)), sum(coalesce(s.HaberAjusteConsolida, 0))
					from ConSaldoDiario s 
				   inner join ConCatalogo c 
					  on s.cuenta = c.Oid
				   where c.Empresa   = @Empresa
					 and s.Periodo   = Year(@FechaDesde) 
					 and cast(s.Fecha as date) between cast(@FechaDesde as date) and cast(@FechaHasta as date)
					 and c.Nivel       = @nivel 
				   group by c.Empresa, s.Periodo, c.CtaPadre, s.Fecha, s.TipoSaldoDia) as origen
			     (Empresa, Periodo, CtaPadre, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
			  on (destino.Periodo  = origen.Periodo
			 and  destino.Cuenta   = origen.CtaPadre
			 and  destino.Fecha    = origen.Fecha
			 and  destino.TipoSaldoDia = origen.TipoSaldoDia)
            when not matched by target then
		         insert (Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
				 values (origen.Periodo, origen.CtaPadre, origen.Fecha, origen.TipoSaldoDia, origen.Debe, origen.Haber,
				         origen.DebeAjusteConsolida, origen.HaberAjusteConsolida)
			when matched then
			     update set
				        Debe                = coalesce(Destino.Debe, 0) + coalesce(origen.Debe, 0),
						Haber               = coalesce(Destino.Haber, 0) + coalesce(origen.Haber, 0),
						DebeAjusteConsolida = coalesce(destino.DebeAjusteConsolida, 0) + coalesce(origen.DebeAjusteConsolida, 0),
						HaberAjusteConsolida= coalesce(destino.HaberAjusteConsolida, 0) + coalesce(origen.HaberAjusteConsolida, 0); 
	  commit tran t4
	end try
	begin catch
	  rollback tran t4
	  select @error_msg = error_message(), @error_sev = error_severity()
	  raiserror(@error_msg, @error_sev, 1)
	end catch  		
--    print 'Filas Procesadas: ' + convert(varchar(8), @filas) + ' en el nivel ' + convert(varchar(4), @nivel)
    select @nivel = @nivel - 1
  end  
--  print 'fin de la actualizacion de saldos'
  /* ahora actualizamos los saldos del mes */ 
  begin try
    begin tran t5 
	  exec spConGeneraSaldosMes @Empresa, @FechaHasta, @Usuario
	  /* posteamos las transacciones contables mayorizadas */
	  update ConPartida
		 set mayorizada = 1
	   where Empresa    = @Empresa
		 and Periodo    = Year(@FechaDesde)
		 and cast(Fecha as date) between cast(@FechaDesde as date) and cast(@FechaHasta as date)
	  /* se cierran los días */
	  merge ConCierre as Destino
	    using (select distinct c.Empresa, s.Fecha from ConSaldoDiario s
		        inner join ConCatalogo c
				   on s.Cuenta = c.Oid
		        where c.Empresa = @Empresa
				  and s.Fecha between @FechaDesde and @FechaHasta) as origen
		      (Empresa, Fecha)
		   on (origen.Empresa     = destino.Empresa
		  and  origen.Fecha       = destino.FechaCierre)
	     when not matched then
		      insert (Empresa, FechaCierre, DiaCerrado, MesCerrado, FechaCierreAudit)
			  values (origen.Empresa, origen.Fecha, 1, 0, null)
		 when matched then
		      update set
			         DiaCerrado = 1,
					 MesCerrado = 0,
					 FechaCierreAudit = null;
	     
	  /* se borran las partidas preliminares digitadas con las fechas de cierre 
		 y que ya generaron la correspondiente partida contable
		 ******* OJO PARTE PENDIENTE *******
	  */
	  
	  --delete from CON_PARTIDAPRE_DETA
	  -- where cod_emp     = @cod_emp 
		 --and cod_periodo = @cod_periodo 
		 --and corr_ptda in (select corr_ptda from CON_PARTIDAPRE_ENCA
			--				where cod_emp     = @cod_emp 
			--				  and cod_periodo = @cod_periodo
			--				  and fecha_ptda between @fecha_desde and @fecha_hasta
			--				  and estado_ptda = 'G')
	  --delete from CON_PARTIDAPRE_ENCA
	  -- where cod_emp     = @cod_emp 
		 --and cod_periodo = @cod_periodo
		 --and fecha_ptda between @fecha_desde and @fecha_hasta
		 --and estado_ptda = 'G'  
      --- ********************************

	commit tran t5
  end try
  begin catch
    rollback tran t5
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch  
end
