use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaParametroVacacion' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaParametroVacacion;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 16/07/2020
Propósito      : Retornar el parametro para el calculo de vacacion, a partir del tramo de anios laborados
Parámetros
  @OidEmpresa  : Id de la empresa para la cual se obtiene la politica de vacacion
  @Anios       : Cantidad de anios laborados
Retorna        : El parametro para el calculo de vacacion, a partir del tramo de anios laborados
Comentarios    : 
****************************************************************************************/
create function fnPlaParametroVacacion(
    @OidEmpresa int,
    @Anios      smallint) 
returns numeric(6,2)
as
begin
  declare @valor smallint;
  select top 1 @valor = Valor from ParametroVacacion
   where Empresa = @OidEmpresa 
     and @Anios >= Desde and @Anios < Hasta
	 and Activo = 1
  return coalesce(@valor, 0);  
end 
go
grant execute on fnPlaParametroVacacion to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el parametro para el calculo de vacacion, a partir del tramo de anios laborados' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaParametroVacacion'
GO