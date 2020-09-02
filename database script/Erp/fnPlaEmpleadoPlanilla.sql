use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaEmpleadoPlanilla' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaEmpleadoPlanilla;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrique Lemus
Fecha Creación : 18/07/2020
Propósito      : Retornar datos adicionales al empleado y requeridos para el calculo de la planilla
Parámetros
  @OidEmpresa  : Id de la empresa para la cual se ejecuta la funcion y que corresponde a la empresa
                 para la cual se calcula la planilla
  @OidEmpleado : Id del empleado para el cual se ejecuta la funcion y quien corresponden los datos obtenidos
  @FechaInicio : Fecha de inicio de la planilla a calcular, normalmente inicio de la quincena
  @FechaFin    : Fecha de finalizacion de la planilla a calcular, normalemnte el fin de la quincena
  @FechaPago   : Fecha de pago de la planilla
Retorna        : Retornar datos adicionales al empleado y requeridos para el calculo de la planilla
Comentarios    : PENDIENTE DE VER SI INCORPORAMOS AQUI LA CONDICION PARA FILTRAR POR TIPO DE PLANILLA
                 (tabla EmpleadoTipoPlanilla)
****************************************************************************************/
create function fnPlaEmpleadoPlanilla(
   @OidEmpresa   int,
   @OidEmpleado  int,
   @FechaInicio datetime2,
   @FechaFin    datetime2,
   @FechaPago   date
)
returns @EmpleadoPlanilla table (       
    Empleado               int,
	DiasLicenciaSinSueldo  smallint,
	DiasInasistencia       smallint,
	DiasAmonestacion       smallint,
	DiasIncapacidad        smallint,
	DiasMaternidad         smallint,
	TotalHorasExtra        numeric(14,2),
	CotizaAcumuladaIsss    numeric(14,2),
	CotizaAcumuladaAfp     numeric(14,2),
	CotizaAcumuladaRenta   numeric(14,2),
	IngresoBrutoQuincena   numeric(14,2),
	FechaInicio            datetime2,
	FechaFin               datetime2,
	FechaPago              datetime, 
	ParametroEmpresa       int)
as
begin
  declare @InicioMes datetime2 = DateFromParts(Year(@FechaFin), Month(@FechaFin), 1)
  declare @FinMes    datetime2 = dbo.fnEndDay('mcs', dateadd(day, -1, dateadd(month, 1, @InicioMes)))
  insert into @EmpleadoPlanilla
  select emple.Oid, /*afp.Comision, afp.AporteAfiliado, afp.AporteEmpresa, c.Salario,  */
         dbo.fnPlaAcumuladoDiasAccion(emple.Oid, 1, @FechaInicio, @FechaFin),  -- dias_lic_ss, 
         dbo.fnPlaAcumuladoDiasAccion(emple.Oid, 11, @FechaInicio, @FechaFin), -- dias_inasist, 
         dbo.fnPlaAcumuladoDiasAccion(emple.Oid, 15, @FechaInicio, @FechaFin), -- dias_amonest, 
         dbo.fnPlaAcumuladoDiasAccion(emple.Oid, 2, @FechaInicio, @FechaFin),  -- dias_incap, 
         dbo.fnPlaAcumuladoDiasAccion(emple.Oid, 10, @FechaInicio, @FechaFin) as dias_mat,
         dbo.fnPlaTotalHorasExtra(emple.Oid, @FechaPago),  
         -- los siguientes ID de operacion en la invocacion de la funcion fn_OperacionAcumulada, 
         -- revisarlos cuando se hallan parametrizado porque pueden ser diferentes
         dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes, 17), -- cot_acum_isss, 
         dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes, 18), -- cot_acum_afp, 
         dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes, 19), -- cot_acum_renta,          
         -- obtenemos los ingresos gravados acumulados del mes: 
         -- Ingresos Gravados Brutos Quincena (16) incluye vacacion + Aguinaldo Bruto (6)
         dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes,  16) +
         iif(month(@FechaPago) < 12, 0.0, 
         dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes, 6)), -- ingreso_bruto, 
         @FechaInicio, @FechaFin, @FechaPago, p.Oid
    from Empleado emple
   inner join Persona p
	  on emple.Oid = p.Oid 
--   left join Cargo c
--     on emple.Cargo  = c.Oid
--   left join Afp afp
--     on emple.AFP = afp.Proveedor
    left join PlaParametroEmpresa pe
	  on emple.Empresa = pe.Empresa
   where emple.Oid = @OidEmpleado
     and p.FechaRetiro is null      
     and emple.Estado in ('EMPL01', 'EMPL04', 'EMPL05', 'EMPL06')
     and emple.TipoContrato in (0, 1)  -- excluye subcontratos y otros
     and not exists(select Oid from Empleado
                     where Oid = emple.Oid 
                       and (month(FechaIngreso) = month(@FechaInicio) 
                       and day(FechaIngreso) between day(@FechaInicio) and day(@FechaFin)
                       and year(FechaIngreso) < year(@FechaInicio)))   
  return; 
end
go
grant select, references on fnPlaEmpleadoPlanilla to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar datos adicionales al empleado y requeridos para el calculo de la planilla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaEmpleadoPlanilla'
GO
