USE [Sbt_Erp]
GO

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
CREATE procedure [dbo].[spConCierreDiario](
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

  --- procesamos, la partida de apertura (si se encuentra en @fecha_desde)
  begin try
    begin tran t2  
	  /* obtenemos la fecha de la partida de apertura */
	  select @fecha = min(Fecha) from ConPartida
	   where Empresa     = @Empresa
		 and Periodo     = @Periodo
		 and Tipo        = 0 /* apertura */
		 and Fecha >= @FechaDesde
	  if (@fecha is not null) --and (@fecha_desde <= @fecha))
	  begin
		/* en este caso debemos procesar la partida de apertura */
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, e.Fecha, sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = Year(@FechaDesde) 
					 and e.Fecha   = @fecha
					 and e.Tipo = 0  -- APERTURA
				   group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
               on (destino.Empresa  = origen.Empresa
			  and  destino.Periodo  = origen.Periodo
			  and  destino.Fecha    = origen.Fecha
			  and  destino.Cuenta   = origen.Cuenta
			  and  destino.TipoSaldoDia = 0 /* apertura */)
		     when not matched by target then
			      insert (Empresa, Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida,
				          FechaCrea, UsuarioCrea)
				  values (origen.Empresa, origen.Periodo, origen.Cuenta, origen.Fecha, 0, origen.Debe, origen.Haber,
				          0, 0, @Usuario, current_timestamp)
		    when not matched by source then
			     Delete
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber,
					UsuarioMod = @Usuario,
					FechaMod = current_timestamp;    
	  end  
	commit tran t2
  end try
  begin catch
    rollback tran t2
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
  /* insertamos los saldos de las cuentas de detalle de las partidas de Diario, Ingresos y Egresos*/
  begin try
    begin tran t3 
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, e.Fecha, sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = Year(@FechaDesde) 
					 and e.Fecha   = @fecha
					 and e.Tipo in (1, 2, 3)  -- Diario, Ingreso, Egreso
				   group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
               on (destino.Empresa  = origen.Empresa
			  and  destino.Periodo  = origen.Periodo
			  and  destino.Fecha    = origen.Fecha
			  and  destino.Cuenta   = origen.Cuenta
			  and  destino.TipoSaldoDia = 1 /* operaciones del ejercicio */)
		     when not matched by target then
			      insert (Empresa, Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida,
				          FechaCrea, UsuarioCrea)
				  values (origen.Empresa, origen.Periodo, origen.Cuenta, origen.Fecha, 1, origen.Debe, origen.Haber,
				          0, 0, @Usuario, current_timestamp)
		    when not matched by source then
			     Delete
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber,
					UsuarioMod = @Usuario,
					FechaMod = current_timestamp;    
	commit tran t3
  end try
  begin catch
    rollback tran t3
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 	   
   /* insertamos los saldos de las cuentas de de detalle cuando es partida de liquidacion */  
  begin try
    begin tran t4 
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, e.Fecha, sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = Year(@FechaDesde) 
					 and e.Fecha   = @fecha
					 and e.Tipo = 4  -- Liquidacion
				   group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
               on (destino.Empresa  = origen.Empresa
			  and  destino.Periodo  = origen.Periodo
			  and  destino.Fecha    = origen.Fecha
			  and  destino.Cuenta   = origen.Cuenta
			  and  destino.TipoSaldoDia = 2 /* Liquidacion */)
		     when not matched by target then
			      insert (Empresa, Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida,
				          FechaCrea, UsuarioCrea)
				  values (origen.Empresa, origen.Periodo, origen.Cuenta, origen.Fecha, 2, origen.Debe, origen.Haber,
				          0, 0, @Usuario, current_timestamp)
		    when not matched by source then
			     Delete
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber,
					UsuarioMod = @Usuario,
					FechaMod = current_timestamp;   
	commit tran t4
  end try
  begin catch
    rollback tran t4
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch    
   /* insertamos los saldos de las cuentas de de detalle cuando es partida de cierre */ 
  begin try
    begin tran t5  
		merge ConSaldoDiario as destino
		   using (select e.Empresa, e.Periodo, d.Cuenta, e.Fecha, sum(coalesce(d.Debe / e.ValorMoneda, 0)),
					     sum(coalesce(d.Haber / e.ValorMoneda, 0))
					from ConPartidaDetalle d 
					inner join ConPartida e 
					  on d.Partida = e.Oid
				   where e.Empresa = @Empresa   
				 	 and e.Periodo = Year(@FechaDesde) 
					 and e.Fecha   = @fecha
					 and e.Tipo = 5  -- Cierre
				   group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
				  (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
               on (destino.Empresa  = origen.Empresa
			  and  destino.Periodo  = origen.Periodo
			  and  destino.Fecha    = origen.Fecha
			  and  destino.Cuenta   = origen.Cuenta
			  and  destino.TipoSaldoDia = 3 /* Cierre */)
		     when not matched by target then
			      insert (Empresa, Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida,
				          FechaCrea, UsuarioCrea)
				  values (origen.Empresa, origen.Periodo, origen.Cuenta, origen.Fecha, 3, origen.Debe, origen.Haber,
				          0, 0, @Usuario, current_timestamp)
		    when not matched by source then
			     Delete
			when matched then
			     update set
				    Debe = origen.Debe,
					Haber = origen.Haber,
					UsuarioMod = @Usuario,
					FechaMod = current_timestamp;   
	commit tran t5
  end try
  begin catch
    rollback tran t5
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch      
  /* insertamos los saldos de las cuentas de detalle cuando son partidas de ajuste por consolidacion */
  begin try
    begin tran t6 
	  merge ConSaldoDiario as destino
	     using (select e.Empresa, e.Periodo, d.Cuenta, e.Fecha, sum(coalesce(d.Debe / e.ValorMoneda, 0)), 
					   sum(coalesce(d.Haber / e.ValorMoneda, 0))
				  from ConPartidaDetalle d 
			     inner join ConPartida e 
				    on d.Partida = e.Oid
			     where e.Empresa     = @Empresa   
				   and e.Periodo     = Year(@FechaDesde) 
				   and e.Fecha between @FechaDesde and @FechaHasta 
				   and d.AjusteConsolida = 1
			     group by e.Empresa, e.Periodo, d.Cuenta, e.Fecha) as origen
		       (Empresa, Periodo, Cuenta, Fecha, Debe, Haber)
            on (destino.Empresa = origen.Empresa
		   and  destino.periodo = origen.Periodo
		   and  destino.Fecha   = origen.Fecha
		   and  destino.Cuenta  = origen.Cuenta
		   and  destino.TipoSaldoDia = 1)   /* saldo de las operaciones diarias del ejercicio */
          when matched then
		       update set
			          HaberAjusteConsolida = origen.Debe,
					  DebeAjusteConsolida  = origen.Haber;
	commit tran t6
  end try
  begin catch
    rollback tran t6
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch  
  /* borramos los saldos de las cuentas de resumen, no se hace con el merge porque se deben borrar todos
     los niveles en un solo paso, para evitar que resulten inconsistencias, cuando una cuenta de resumen
	 se acumula desde cuentas de diferente nivel, esto ultimo no deberia de pasar con un catalogo bien 
	 estructurado, pero es factible que suceda
	*/
   begin try
	 begin tran t7
	   delete s
	     from ConSaldoDiario s
		inner join ConCatalogo c
		   on s.Cuenta = c.OID
	    where s.Empresa = @Empresa
		  and s.Periodo = Year(@FechaDesde)
		  and s.Fecha between @FechaDesde and @FechaHasta
	      and c.CtaResumen = 1  -- solo los saldos de las cuentas de resumen
	   commit tran t7
     end try
   begin catch
      rollback tran t7
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
      begin tran t8  
	    merge ConSaldoDiario as Destino
		   using (select s.Empresa, s.Periodo, c.CtaPadre, s.Fecha, s.TipoSaldoDia,
						 sum(coalesce(s.Debe, 0)), sum(coalesce(s.Haber, 0)), 
						 sum(coalesce(s.DebeAjusteConsolida, 0)), sum(coalesce(s.HaberAjusteConsolida, 0))
					from ConSaldoDiario s 
				   inner join ConCatalogo c 
					  on s.cuenta = c.OID
				   where s.Empresa   = @Empresa
					 and s.Periodo   = Year(@FechaDesde) 
					 and s.Fecha between @FechaDesde and @FechaHasta 
					 and c.Nivel       = @nivel 
				   group by s.Empresa, s.Periodo, c.CtaPadre, s.Fecha, s.TipoSaldoDia) as origen
			     (Empresa, Periodo, CtaPadre, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida)
			  on (destino.Empresa  = origen.Empresa
			 and  destino.Periodo  = origen.Periodo
			 and  destino.Cuenta   = origen.CtaPadre
			 and  destino.Fecha    = origen.Fecha
			 and  destino.TipoSaldoDia = origen.TipoSaldoDia)
            when not matched by target then
		         insert (Empresa, Periodo, Cuenta, Fecha, TipoSaldoDia, Debe, Haber, DebeAjusteConsolida, HaberAjusteConsolida, 
				         UsuarioCrea, FechaCrea)
				 values (origen.Empresa, origen.Periodo, origen.CtaPadre, origen.Fecha, origen.TipoSaldoDia, origen.Debe, origen.Haber,
				         origen.DebeAjusteConsolida, origen.HaberAjusteConsolida, @Usuario, current_timestamp)
			when matched then
			     update set
				        Debe                = coalesce(Destino.Debe, 0) + coalesce(origen.Debe, 0),
						Haber               = coalesce(Destino.Haber, 0) + coalesce(origen.Haber, 0),
						DebeAjusteConsolida = coalesce(destino.DebeAjusteConsolida, 0) + coalesce(origen.DebeAjusteConsolida, 0),
						HaberAjusteConsolida= coalesce(destino.HaberAjusteConsolida, 0) + coalesce(origen.HaberAjusteConsolida, 0),
						UsuarioMod          = @Usuario,
						FechaMod            = current_timestamp; 
	  commit tran t8
	end try
	begin catch
	  rollback tran t8
	  select @error_msg = error_message(), @error_sev = error_severity()
	  raiserror(@error_msg, @error_sev, 1)
	end catch  		
--    print 'Filas Procesadas: ' + convert(varchar(8), @filas) + ' en el nivel ' + convert(varchar(4), @nivel)
    select @nivel = @nivel - 1
  end  
--  print 'fin de la actualizacion de saldos'
  /* ahora actualizamos los saldos del mes */ 
  begin try
    begin tran t9 
	  exec spConGeneraSaldosMes @Empresa, @FechaHasta, @Usuario
	  /* posteamos las transacciones contables mayorizadas */
	  update ConPartida
		 set mayorizada = 1
	   where Empresa    = @Empresa
		 and Periodo    = Year(@FechaDesde)
		 and Fecha between @FechaDesde and @FechaHasta  
	  /* se cierran los días */
	  merge ConCierre as Destino
	    using (select distinct Empresa, Fecha from ConSaldoDiario
		        where Empresa = @Empresa
				  and Fecha between @FechaDesde and @FechaHasta) as origen
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

	commit tran t9
  end try
  begin catch
    rollback tran t9
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch  
end
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Realizar el cierre diario contable, incluyendo la mayorización y actualización de
los saldos presupuestarios. 
Además se invoca al final la ejecución del procedimiento que calcula los saldos
mensuales y postea las partidas contables del rango de días pasados como parámetros
para evitar que sean modificadas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'SpConCierreDiario'
GO

grant execute on spConCierreDiario to public
