USE [Sbt_Erp]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =====================================================================================
-- Author     :	Santiago Enrique Lemus
-- Create date: 12/05/2020
-- Description: retorna el numero de documento del argumento traducido a texto
-- Parametros :
---		 @NumDoc = Numero de documento a retornar traducido a texto
--- Creado por    : SELM
--- Fecha Creación: 12/05/2020
-- =====================================================================================

create function [dbo].[fnPlaNumDocumentoATexto](@NumDoc varchar(20))
returns varchar(250)
as
begin
  declare @resultado varchar(250)
  declare @i smallint = 1
  declare @c integer;
  if ltrim(rtrim(@NumDoc)) = ''
    return ''
  set @resultado = @NumDoc
  set @resultado = replace(@resultado, '0', 'cero ')
  set @resultado = replace(@resultado, '1', 'uno ')
  set @resultado = replace(@resultado, '2', 'dos ')
  set @resultado = replace(@resultado, '3', 'tres ')
  set @resultado = replace(@resultado, '4', 'cuatro ')
  set @resultado = replace(@resultado, '5', 'cinco ')
  set @resultado = replace(@resultado, '6', 'seis ')
  set @resultado = replace(@resultado, '7', 'siete ')
  set @resultado = replace(@resultado, '8', 'ocho ')
  set @resultado = replace(@resultado, '9', 'nueve ')  
  set @resultado = replace(@resultado, '-', '')
  return ltrim(rtrim(@resultado))
end
go
grant execute on [dbo].[fnPlaNumDocumentoATexto] to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retorna el numero de documento del argumento convertido en texto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaNumDocumentoATexto'
go


