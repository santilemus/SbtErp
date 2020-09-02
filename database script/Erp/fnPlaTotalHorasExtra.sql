use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaTotalHorasExtra' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaTotalHorasExtra;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 16/07/2020
Propósito      : Retornar el monto a pagar en concepto de horas extras para un empleado
Parámetros
  @OidEmpleado : Código del empleado que se quiere conocer su acumulado de acciones,
  @FechaPago   : Fecha de Pago de las horas extras. Debe corresponder con la fecha de pago 
                 de la planilla en la cual seran canceladas las horas extras
Retorna        : El monto total a pagar en concepto de horas extras
Comentarios    : En este caso las horas extras se alimentan a través del mantenimiento,
                 con título Registro de Horas Extras. Es un reporte de horas extras para
				 un período por empleado. Después de ingresar las horas extras, el usuario
				 debe calcular el resumen, para que el sistema calcule los montos en función
				 del tipo de hora extra
****************************************************************************************/
CREATE function [dbo].[fnPlaTotalHorasExtra](
     @OidEmpleado int,
     @FechaPago   datetime2
) 
returns numeric(6,2)
as
begin
  declare @monto money = 0
  select @monto = sum(coalesce(rhe.valor, 0))
    from ReporteHoraExtraResumen rhe
   inner join ReporteHoraExtra e
      on rhe.ReporteHoraExtra = e.Oid
   where e.Empleado   = @OidEmpleado
     and cast(e.FechaPago as date)  = cast(@FechaPago as date)
	 and e.Planilla is null
  return coalesce(@monto, 0);
end
GO
grant execute on fnPlaTotalHorasExtra to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el monto total a pagar en concepto de horas extras' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaTotalHorasExtra'
GO



