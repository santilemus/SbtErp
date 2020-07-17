use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaProfesionEmpleado' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaProfesionEmpleado;
end
go
/****** Object:  UserDefinedFunction [dbo].[plaProfesionEmpleado]    Script Date: 15/7/2020 00:20:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creacion : 15/07/2020
Propósito      : Retorna la descripcion de la primera profesion del empleado
Parámetros
  @OidEmpleado : Código del empleado
Retorna        : Retorna la descripcion de la primera profesion del empleado
****************************************************************************************/
CREATE function [dbo].[fnPlaProfesionEmpleado](
   @OidEmpleado int)
returns varchar(25)
as
begin
  declare @descripcion varchar(25)
  select top 1 @descripcion = prof.TituloCorto from EmpleadoProfesion ep
   inner join Profesion prof
      on ep.Profesion = prof.Oid 
   where ep.Empleado  = @OidEmpleado 
  return @descripcion
end
GO
grant execute on dbo.fnPlaProfesionEmpleado to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'retorna la descripcion de la primera profesion del empleado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaProfesionEmpleado'
GO


