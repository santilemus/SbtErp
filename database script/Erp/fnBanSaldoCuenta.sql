USE [Sbt_Erp]
GO

/****** Object:  UserDefinedFunction [dbo].[banSaldoCuenta]    Script Date: 12/5/2020 16:00:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===========================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 12/05/2020
-- Description:	Retorna el saldo de una cuenta bancaria a una fecha específica
-- ===========================================================================
create function [dbo].[fnBanSaldoCuenta] 
(
	-- Add the parameters for the function here
	@OidCuenta int,
	@FechaHasta   datetime
)
returns money
as
begin
  declare @valor numeric(14,2);

  select @valor = sum(iif(c.Tipo = 1 or c.Tipo = 2, coalesce(Monto, 0) / coalesce(ValorMoneda, 1),
                          -coalesce(Monto, 0) / coalesce(ValorMoneda, 1)))
    from BanTransaccion t
   inner join BanClasifTransaccion c
      on t.Clasificacion = c.OID
   where t.NumeroCuenta = @OidCuenta
     and t.Fecha  <= @FechaHasta
     and t.Estado <> 3 -- anulado 
  return coalesce(@Valor, 0)
END
go
grant execute on fnBanSaldoCuenta to public
go


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retorna el saldo de una cuenta bancaria a una fecha específica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnBanSaldoCuenta'
GO


