USE [Sbt_Erp]
GO

/****** Object:  StoredProcedure [dbo].[spBanDetalleConciliacion]    Script Date: 18/5/2020 22:38:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- Author     :	Santiago Enrique Lemus
-- Create date: 18/05/2020
-- Description: Genera la información del detalle de la conciliacion bancaria
-- Parametros :
--     @OidConciliacion int
CREATE procedure [dbo].[spBanDetalleConciliacion]
  @OidConciliacion  int  -- es el numero de la conciliacion que se esta creando (la nueva)
as
begin
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CodBanco varchar(14)
	declare @CodCta varchar(14)
	declare @FechaInicio datetime
	declare @FechaFin datetime
	declare @Numero int = 0

	-- obtenemos el banco, cuenta, fecha inicio y fecha fin de la conciliacion nueva
    select @CodBanco = cod_banco, @CodCta = cod_cta, @FechaInicio = fecha_inicio, @FechaFin = fecha_fin 
	 from BAN_CONC_ENCA
	 where cod_emp   = @CodEmp
	   and num_conc  = @NumeroConc
	print 'Conciliacion Nueva: ' + Format(@NumeroConc, 'N0') + '	Fecha Inicio: ' + Format(@FechaInicio, 'D') +
	      '		Fecha Fin ' + Format(@FechaFin, 'D')
	-- obtenemos la conciliacion anterior a la que estamos creando, para obtener las filas 
	-- con estados diferentes de cero (aplicado y contabilizado)
    select @Numero = max(num_conc) from BAN_CONC_ENCA
	 where cod_emp   = @CodEmp
	   and cod_banco = @CodBanco
	   and cod_cta   = @CodCta
	   and num_conc < @NumeroConc
	   and fecha_fin < @FechaInicio
	if @Numero is not null And @Numero > 0
	    merge BAN_CONC_DETA as destino
		using (select cod_emp, num_movb, fecha_mov, concepto, valor, tipo_movim, estado_movcb, fecha_crea, usuario_crea
		         from BAN_CONC_DETA 
		        where cod_emp   = @CodEmp
		          and num_conc  = @Numero   /* @Numero, es la conciliación del mes anterior (ultima) de la cual se obtienen los movimientos
		                                       con estado diferente de aplicado y contabilizado */
				  and estado_movcb > 0) as origen
			  (cod_emp, num_movb, fecha_mov, concepto, valor, tipo_movim, estado_movcb, fecha_crea, usuario_crea)
		   on (destino.cod_emp  = origen.cod_emp
		  and destino.num_conc  = @NumeroConc 
		  and destino.num_movb  = origen.num_movb) 
		 when not matched then
		     insert (cod_emp, num_conc, num_movb, fecha_mov, concepto, valor, tipo_movim, estado_movcb, fecha_crea, usuario_crea) 
			 values (origen.cod_emp, @NumeroConc, origen.num_movb, origen.fecha_mov, origen.concepto, origen.valor, 
			         origen.tipo_movim, origen.estado_movcb, origen.fecha_crea, origen.usuario_crea)
		 when matched then
		     update set
			     destino.fecha_mov = origen.fecha_mov,
				 destino.concepto = origen.concepto,
				 destino.valor = origen.valor,
				 destino.tipo_movim = origen.tipo_movim,
				 destino.fecha_mod  = current_timestamp,
				 destino.usuario_crea = current_user;
	-- ahora se insertan los movimientos del mes
	merge BAN_CONC_DETA as destino
	using (select cod_emp, num_movb, fecha_movb, concepto, 
	              iif(tipo_movim in ('NC', 'RM'), valor_cargo, valor_abono) as valor, tipo_movim
	         from BAN_MOVIM_ENCA e
            where cod_emp   = @CodEmp
	          and cod_banco = @CodBanco
	          and cod_cta   = @CodCta
	          and fecha_movb between @FechaInicio and @FechaFin
	          and estado_mov <> 'A') as origen
		  (cod_emp, num_movb, fecha_movb, concepto, valor, tipo_movim)
	   on (destino.cod_emp = origen.cod_emp
	  and destino.num_conc = @NumeroConc
	  and destino.num_movb = origen.num_movb)
	 when not matched then
		  insert (cod_emp, num_conc, num_movb, fecha_mov, concepto, valor, tipo_movim, estado_movcb, fecha_crea, usuario_crea)  
		  values (origen.cod_emp, @NumeroConc, origen.num_movb, origen.fecha_movb, origen.concepto, origen.valor, 
		          origen.tipo_movim, 0, current_timestamp, current_user)
	 when matched then
		  update set
			     destino.fecha_mov = origen.fecha_movb,
				 destino.concepto = origen.concepto,
				 destino.valor = origen.valor,
				 destino.tipo_movim = origen.tipo_movim,
				 destino.fecha_mod  = current_timestamp,
				 destino.usuario_crea = current_user;	 	        
end
GO


