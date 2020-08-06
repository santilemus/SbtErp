use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaCalcularRenta' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaCalcularRenta;
end
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =====================================================================================
-- Author     :	Santiago Enrique Lemus
-- Create date: 12/05/2020
-- Description: retorna el monto de la retencion de renta de acuerdo al salario, y 
--              tipo de tabla que se recibe en los parametros
-- Parametros :
---		 @NumDoc = Numero de documento a retornar traducido a texto
--- Creado por    : SELM
--- Fecha Creación: 12/05/2020
-- =====================================================================================
create function fnPlaCalcularRenta(@Pais varchar(8), @TipoTabla smallint, @Salario numeric(14,2))
returns numeric(14,2)
as
begin
  declare @RentaFija  numeric(12,2)
  declare @Porcentaje numeric(8, 4)
  declare @Limite     numeric(12,2)

  select @RentaFija = RentaFija, @Porcentaje = Porcentaje, @Limite = Limite
    from ParametroRenta
   where Pais      = @Pais
     and TipoTabla = @TipoTabla
     and @Salario between SueldoDesde and SueldoHasta
  return coalesce(@RentaFija, 0) + (@Salario - coalesce(@Limite, 0)) * coalesce(@Porcentaje, 0)
end
go
grant execute on [dbo].[fnPlaCalcularRenta] to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el monto de la renta a descontar' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaCalcularRenta'
go

