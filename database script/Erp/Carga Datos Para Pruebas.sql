sp_help PLA_CARGOS

insert into Empresa 
       (RazonSocial, Pais, Provincia, Ciudad, Direccion, EMail, Nit, Logo, Activa)
select razon_social, 'SLV', 'SLV06', 'SLV0614', direccion, e_mail, nit, logo, activa
  from PlaniDemo..EMPRESAS

insert into EmpresaUnidad 
       (Empresa, Nombre, Direccion, Telefono, IdRole, Codigo, Activa)
select cod_emp + 1, descripcion, null, telefono_unidad, 1, null, unidad_activa 
  from PlaniDemo..AREA_INSTITUC

insert into Tercero
       (Nombre, TipoPersona, TipoContribuyente, EMail, SitioWeb, Activo)
select razon_social, iif(tipo_persona = 'N', 1, 2), 0, e_mail, cod_entidad, activo 
  from PlaniDemo..ENTIDAD_EXTERNA

insert into TerceroDireccion
       (Tercero, Pais, Provincia, Ciudad, Direccion, Activa, UsuarioCrea, FechaCrea)
select t.OID, 'SLV', 'SLV' + e.cod_dpto, 'SLV' + e.cod_muni, e.direccion, e.activo, 
       current_user, current_timestamp
  from PlaniDemo..ENTIDAD_EXTERNA e
 inner join Tercero t
    on e.cod_entidad = t.SitioWeb COLLATE DATABASE_DEFAULT
 where e.direccion is not null

insert into TerceroDocumento 
       (Tercero, Tipo, Numero, Vigente, UsuarioCrea, FechaCrea)
select t.OID, 'NIT', e.no_doc, 1, current_user, current_timestamp
  from PlaniDemo..ENTIDAD_EXTERNA e
 inner join Tercero t
    on e.cod_entidad = t.SitioWeb COLLATE DATABASE_DEFAULT
 where tipo_doc is not null and no_doc is not null

update t
   set t.SitioWeb = null,
       t.DireccionPrincipal = d.OID
  from Tercero t
 inner join TerceroDireccion d
    on t.OID = d.Tercero

-- insertar las afp
insert into Afp 
       (Proveedor, Siglas, AporteAfiliado, AporteEmpresa, Comision, UsuarioCrea, FechaCrea)
select distinct iif(e.cod_afp = 'AFP01', 1, 2), a.cod_afp as siglas, a.porc_apor_afil, a.porc_apor_emp, a.porc_comi_afp, 
       current_user, current_timestamp
  from PlaniDemo..PLA_EMPLEADO e
 inner join PlaniDemo..ENTIDAD_EXTERNA a
    on e.cod_afp = a.cod_entidad
 where e.cod_afp != 'BC04'

-- insertar los cargos
 SET IDENTITY_INSERT Cargo ON
 insert into Cargo 
        (OID, Nombre, TipoContrato, TipoSalario, Salario, Obligaciones, Activo, FechaCrea, UsuarioCrea)
  select cod_cargo, descripcion, 0, 0, salario, obligaciones, 1, current_timestamp, current_user
    from PlaniDemo..PLA_CARGOS
 SET IDENTITY_INSERT Cargo OFF

 -- insertar Persona
insert into Persona 
       (Nombre, Apellido, Pais, Provincia, Ciudad, Direccion, Genero, EstadoCivil, FechaNacimiento, 
	    FechaIngreso, FechaRetiro, EMail, Fotografia, Idioma, FechaCrea, UsuarioCrea)
select e.prim_nombre + ' ' + coalesce(e.seg_nombre, ''), 
       e.prim_apelli + ' ' + iif(e.ape_casada = '', coalesce(e.seg_apelli, ''), coalesce(e.ape_casada, '')),
	   iif(e.cod_pais = 'US', 'USA', 'SLV'), 'SLV' + coalesce(e.cod_dpto, ''), + 'SLV' + coalesce(e.cod_muni, ''),
	   e.direccion, iif(e.genero = 'F', 1, 0), 
	   case e.estado_civil when 'S' then 1 when 'C' then 2 when 'D' then 5 when 'V' then 4 end, 
	   e.fecha_nacim, e.fecha_contrato, e.fecha_retiro, e.e_mail, e.fotografia, 
	   cast(e.cod_emp as varchar(1)) + cast(e.cod_emple as varchar(6)), 
	   current_timestamp, current_user
  from PlaniDemo..PLA_EMPLEADO e

  -- insertar Empleado, empresa 1
insert into Empleado 
       (Oid, Empresa, Unidad, TipoSalario, Salario, Estado, Nacionalidad, TipoCuenta, NumeroCuenta, Titulo,
	    Cargo, TipoContrato, Banco, DiaDescanso, AFP, NumeroCarne)
select p.Oid, e.cod_emp + 1, iif(e.cod_emp = 1, e.cod_unidad + 1, e.cod_unidad + 9) /* Unidad */, 1, e.salario, 
       case e.estado_empl when 1 then 'EMPL01' when 2 then 'EMPL02' when 5 then 'EMPL05' when 8 then 'EMPL08' end, 
       iif(e.nacionalidad = 'US', 'USA', 'SLV'), iif(e.tipo_ctab = 'A', 1, 2), e.num_cta, null, e.cod_cargo, 0 /* Indefinido */, 
       9 /* BANCO */, 0 /* Domingo */, iif(e.cod_afp = 'AFP01', 1, 2), e.cod_emple 
   from PlaniDemo..PLA_EMPLEADO e
  inner join Persona p
	 on '1' + cast(e.cod_emple as varchar(5)) = p.Idioma
  where e.cod_emp = 1

-- insertar empleado empresa 2
insert into Empleado 
       (Oid, Empresa, Unidad, TipoSalario, Salario, Estado, Nacionalidad, TipoCuenta, NumeroCuenta, Titulo,
	    Cargo, TipoContrato, Banco, DiaDescanso, AFP, NumeroCarne)
select p.Oid, e.cod_emp + 1, iif(e.cod_emp = 1, e.cod_unidad + 1, e.cod_unidad + 9) /* Unidad */, 1, e.salario, 
       case e.estado_empl when 1 then 'EMPL01' when 2 then 'EMPL02' when 5 then 'EMPL05' when 8 then 'EMPL08' end, 
       iif(e.nacionalidad = 'US', 'USA', 'SLV'), iif(e.tipo_ctab = 'A', 1, 2), e.num_cta, null, e.cod_cargo, 0 /* Indefinido */, 
       9 /* BANCO */, 0 /* Domingo */, iif(e.cod_afp = 'AFP01', 1, 2), e.cod_emple 
   from PlaniDemo..PLA_EMPLEADO e
  inner join Persona p
	 on '2' + cast(e.cod_emple as varchar(5)) = p.Idioma
  where e.cod_emp = 2

-- insertar documentos de empleado empresa 1
-- documentos de empleados
insert into PersonaDocumento
       (Persona, Tipo, Numero, Vigente, FechaCrea, UsuarioCrea)
select e.Oid, d.tipo_doc, ltrim(rtrim(replace(d.num_doc, '-',''))), d.vigente, current_timestamp, 'Admin'
  from PlaniDemo..PLA_DOCS_EMPLE d
 inner join Empleado e
    on d.cod_emp = 1
   and d.cod_emple = e.NumeroCarne
 where e.Empresa  = 2

-- insertar documentos de empleado empresa 2
-- documentos de empleados
insert into PersonaDocumento
       (Persona, Tipo, Numero, Vigente, FechaCrea, UsuarioCrea)
select e.Oid, d.tipo_doc, ltrim(rtrim(replace(d.num_doc, '-',''))), d.vigente, current_timestamp, 'Admin'
  from PlaniDemo..PLA_DOCS_EMPLE d
 inner join Empleado e
    on d.cod_emp = 2
   and d.cod_emple = e.NumeroCarne
 where e.Empresa  = 3

 -- insertar tabla de renta
insert into PlaParametroRenta
       (Pais, TipoTabla, SueldoDesde, SueldoHasta, RentaFija, Porcentaje, Limite, UsuarioCrea, FechaCrea)
select 'SLV', case tipo_tabla when 'Mensual' then 0 when 'Quincenal' then 1 when 'Semanal' then 2
       when 'Junio' then 3 when 'Diciembre' then 4 end, sueldo_desde, sueldo_hasta, monto_retenc, 
	   porcent_renta, lim_aplicar_porc, 'Admin', current_timestamp
  from PlaniDemo..PLA_PARAM_RENTA

