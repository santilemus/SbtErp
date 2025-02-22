USE [SbtErp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnConAbonosCuentaMes]    Script Date: 12/11/2021 16:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 27/02/2020
-- Description:	Retorna la suma de los abonos contable de una cuenta para periodo y mes de las operaciones
--              normales (TipoSaldo = 1). No incluye abonos que corresponden a las partidas de Apertura, 
--              liquidacion o cierre
-- Parametros :
--   @Periodo     = Periodo (Año) contable de las operaciones
--   @Mes         = Mes cuyos abonos nos interesan obtener
--   @Cuenta      = Oid del registro del catalogo con el código de la cuenta contable. Es diferente por
--                  empresa, aun cuando el codigo de la cuenta sea similar, por eso ya no es necesario
--                  el parametro @Empresa
-- ================================================================================================
ALTER function [dbo].[fnConAbonosCuentaMes](
     @Periodo     smallint,
	 @Mes         smallint,
     @OidCuenta   int)
returns money
as
begin
  declare @abono money
  select @abono = sum(coalesce(Haber, 0))
    from ConSaldoDiario 
   where Cuenta  = @OidCuenta
	 and Periodo = @Periodo
	 and month(Fecha)  = @Mes
	 and TipoSaldoDia = 1
  return coalesce(@abono, 0)
end
