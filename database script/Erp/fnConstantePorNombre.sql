USE [Sbt_Erp]
GO

/****** Object:  UserDefinedFunction [dbo].[constantePorNombre]    Script Date: 13/3/2020 14:15:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Propósito      : Retornar el valor de una constante global
Parámetros
  @codigo      : Código de la constante
Retorna        : El valor de la constante
****************************************************************************************/
create or alter function [dbo].[fnConstantePorNombre](
     @codigo  varchar(25)
)
returns varchar(150)
as
begin
  declare @valor varchar(150)
  select @valor = Valor
    from Constante
   where codigo = @codigo
  return @valor
end
GO
go
grant execute on dbo.fnConstantePorNombre to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retorna el valor de la constante cuyo código se recibe en el parámetro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnConstantePorNombre'
GO


