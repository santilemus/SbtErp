USE [Sbt_Erp]
GO

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
--   @Empresa     = Código de la empresa
--   @Periodo     = Periodo (Año) contable de las operaciones
--   @Mes         = Mes cuyos abonos nos interesan obtener
--   @Cuenta      = Oid del registro del catalogo con el código de la cuenta contable
-- ================================================================================================
CREATE function [dbo].[fnConAbonosCuentaMes](
     @Empresa     smallint,
     @Periodo     smallint,
	 @Mes         smallint,
     @OidCuenta   int)
returns money
as
begin
  declare @abono money
  select @abono = sum(coalesce(Haber, 0))
    from ConSaldoDiario 
   where Empresa = @Empresa
	 and Cuenta  = @OidCuenta
	 and Periodo = @Periodo
	 and month(Fecha)  = @Mes
	 and TipoSaldoDia = 1
  return coalesce(@abono, 0)
end
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retorna los abonos de las operaciones de una cuenta en un periodo y mes. No considera los que corresponden a partidas de Apertura, Liquidacion o Cierre' , 
     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnConAbonosCuentaMes'
GO
grant execute on fnConAbonosCuentaMes to public
go

