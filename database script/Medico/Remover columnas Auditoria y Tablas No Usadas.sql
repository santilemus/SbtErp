use Medico
go
alter table ActividadEconomica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table AsociacionProfesional
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'ArchivoAdjunto'))
begin
  drop table ArchivoAdjunto
end
go
alter table Cargo
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ActividadEconomica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'CdoPercentilPesoEstaturaBMI'))
begin
  drop table CdoPercentilPesoEstaturaBMI
end
go
alter table Cita
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConCatalogo
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConPeriodo
 drop constraint FK_ConPeriodo_cod_emp
go
drop index icod_emp_ConPeriodo on ConPeriodo
go
drop index icod_empnumero_ConPeriodo on ConPeriodo
go
alter table ConPeriodo
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod, cod_emp, numero, fecha_inicio, fecha_fin;
go
alter table ConsultaExamen
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConsultaExamenFisico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConsultaReceta
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConsultaSigno
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ConsultaSintoma
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpleadoCapacitacion
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpleadoMembresia
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpleadoPariente
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpleadoProfesion
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Empresa
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpresaGiro
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EmpresaUnidad
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'Enfermedad_antes'))
begin
  drop table Enfermedad_antes
end
go
alter table Enfermedad
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table EstiloVida
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Examen
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table FactorRiesgo
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table HistoriaFamiliar
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'HistorialCrecimiento'))
begin
  drop table HistorialCrecimiento
end
go
alter table Inventario
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Kardex
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Listas
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table MedicamentoDosis
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table MedicamentoVia
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table MedicoConsultorio
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table MedicoEspecialidad
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table MedLista
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Moneda
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table PacienteMedico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'PacienteVacunas'))
begin
  drop table PacienteVacunas
end
go
alter table PacienteVacunas
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Pariente
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table HistoriaFamiliar
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table PersonaContacto
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table PersonaDocumento
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table PlanMedico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table PlanMedicoDetalle
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProblemaMedico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProCategoria
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoAtributo
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoCodigoBarra
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoEnsamble
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoEquivalente
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoImpuesto
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'ProductoLote'))
begin
  drop table ProductoLote
end
alter table ProductoPrecio
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProductoProveedor
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Profesion
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table ProPresentacion
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table RecordatorioClinico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table RecordatorioClinico
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Regla
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table SysConsulta
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TablaIMC
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Telefono
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroContacto
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroDireccion
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroDocumento
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroGarantia
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroGiro
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroNota
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroRole
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroSucursal
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerminologiaAnatomica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table TerceroSucursal
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'TMP_ACT_ECONOMICA'))
begin
  drop table TMP_ACT_ECONOMICA
end
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'TMP_CVX_VACUNA'))
begin
  drop table TMP_CVX_VACUNA
end
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'TMP_CIE_10'))
begin
  drop table TMP_CIE_10
end
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'tmp_GrupoMedicamentos'))
begin
  drop table tmp_GrupoMedicamentos
end
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'tmp_GrupoMedicamentos_copia'))
begin
  drop table tmp_GrupoMedicamentos_Copia
end
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'tmp_Medicamento'))
begin
  drop table tmp_Medicamento
end
go
alter table UltraSonografiaObstetrica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table UltrasonografiaObstetricaDetalle
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table UltrasonografiaPelvica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table UnidadMedida
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
alter table Vacuna
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go
if (exists(select * from INFORMATION_SCHEMA.TABLES
            where TABLE_SCHEMA = 'dbo'
              and TABLE_NAME = 'WhoPercentilPesoLong'))
begin
  drop table WhoPercentilPesoLong
end
go
alter table ZonaGeografica
  drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod;
go