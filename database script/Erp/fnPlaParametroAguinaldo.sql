use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaParametroAguinaldo' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaParametroAguinaldo;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 16/07/2020
Propósito      : Retornar el parametro para el calculo del aguinaldo, a partir del tramo de anios laborados
Parámetros
  @OidEmpresa  : Id de la empresa para la cual se obtiene la politica de aguinaldo
  @Anios       : Cantidad de anios laborados
  @Metodo      : Metodo de calculo => Representa el tipo de valor: DiasSalario = 0, Porcentaje = 1, Monto Fijo = 2
Retorna        : El parametro para el calculo del aguinaldo, a partir del tramo de anios laborados
Comentarios    : 
****************************************************************************************/
create function fnPlaParametroAguinaldo(
    @OidEmpresa int,
    @Anios      smallint,
	@Metodo     smallint) 
returns numeric(6,2)
as
begin
  declare @valor smallint;
  select top 1 @valor = Valor from ParametroAguinaldo 
   where Empresa = @OidEmpresa 
     and @Anios >= Desde and @Anios < Hasta
     and Metodo = @Metodo and Activo = 1  
  return coalesce(@valor, 0);  
end 
go
grant execute on fnPlaParametroAguinaldo to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el acumulado de los ingresos gravados en los empleos previos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaParametroAguinaldo'
GO