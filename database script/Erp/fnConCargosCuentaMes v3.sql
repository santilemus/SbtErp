USE [SbtErp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnConCargosCuentaMes]    Script Date: 12/1/2022 12:25:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 27/02/2020
-- Description:	Retorna la suma de los cargos contable de una cuenta para periodo y mes de las operaciones
--              normales (TipoSaldo = 1). No incluye cargos que corresponden a las partidas de Apertura, 
--              liquidacion o cierre
-- Parametros :
--   @Periodo     = Periodo (Año) contable de las operaciones
--   @Mes         = Mes cuyos cargos nos interesan obtener
--   @Cuenta      = Oid del registro del catalogo con el código de la cuenta contable. Es diferente por
--                  empresa, aun cuando el codigo de la cuenta sea similar, por eso ya no es necesario
--                  el parametro @Empresa
-- Modificado : Por SELM el 12/enero/2022. En el caso de la partida de liquidacion las cuentas
--              de capital y pasivo afectadas por la determinacion de la utilidad, reservas e impuestos
--              por pagar deben verse afectadas
-- ================================================================================================
ALTER function [dbo].[fnConCargosCuentaMes](
     @Periodo     smallint,
	 @Mes         smallint,
     @OidCuenta   int)
returns money
as
begin
  declare @cargo money
  select @cargo = sum(coalesce(s.Debe, 0))
    from ConSaldoDiario s
   inner join ConCatalogo c
      on s.Cuenta = c.Oid
   where s.Cuenta  = @OidCuenta
	 and s.Periodo = @Periodo
	 and month(s.Fecha)  = @Mes
	 and (s.TipoSaldoDia = 1
	  or (s.TipoSaldoDia = 2 And c.TipoCuenta in (1, 2, 3, 7)))
  return coalesce(@cargo, 0)
end
