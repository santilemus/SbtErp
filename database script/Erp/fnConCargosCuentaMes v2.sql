USE [SbtErp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnConCargosCuentaMes]    Script Date: 12/11/2021 16:04:40 ******/
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
-- ================================================================================================
ALTER function [dbo].[fnConCargosCuentaMes](
     @Periodo     smallint,
	 @Mes         smallint,
     @OidCuenta   int)
returns money
as
begin
  declare @cargo money
  select @cargo = sum(coalesce(Debe, 0))
    from ConSaldoDiario 
   where Cuenta  = @OidCuenta
	 and Periodo = @Periodo
	 and month(Fecha)  = @Mes
	 and TipoSaldoDia = 1
  return coalesce(@cargo, 0)
end
