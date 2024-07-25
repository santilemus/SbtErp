-- 1. Copiar el catálogo contable, primero  nivel 1, luego nivel 2...hasta nivel 9
insert into ConCatalogo
       (Empresa, CodigoCuenta, Nombre, CtaPadre, TipoCuenta, CtaResumen, CtaMayor, TipoSaldoCta, Activa)
select 3 as EmpresaOid, c.cod_cuenta as CodigoCuenta, c.descripcion, 
       (select Oid from ConCatalogo x
	     where x.Empresa = 3
		   and x.CodigoCuenta = c.cta_ctrl collate SQL_Latin1_General_CP1_CI_AS), 
       choose(c.tipo_cta, 1, 2, 3, 6, 4, 5, 8) as TipoCuenta, c.cta_resumen, c.cta_mayor, 
	   iif(c.tipo_saldo = 'D', 0, iif(c.tipo_saldo = 'A', 1, null)) as TipoSaldo, 1 as Activa
  from siaf_sql_pruebas..CON_CATALOGOS c
 where c.cod_emp = 1
   and c.cod_periodo = 2017
   and c.cod_cuenta not in ('901', '902', '903')
   and c.nivel = 1 -- luego nivel = 2, nivel = 3, nivel = 4, nivel = 5, nivel = 6, nivel = 7, nivel = 8, nivel = 9

-- 2. Migrar las familias de ACTIVOS
-- 2.1 Migrar las familias con padre nullo
   insert into ActivoCategoria 
          (Nombre, Padre, CuentaActivo, CuentaDepreciacion, Codigo, CuentaGasto, VidaUtil, ValorResidual, Activo)
   select f.descripcion, null as Padre,
       (select c1.Oid from ConCatalogo c1
	     where c1.Empresa = 3
		   and c1.CodigoCuenta = f.cta_activo collate SQL_Latin1_General_CP1_CI_AS) as CuentaActivo,
	   (select c2.Oid from ConCatalogo c2
	     where c2.Empresa = 3
		   and c2.CodigoCuenta = f.cta_depre collate SQL_Latin1_General_CP1_CI_AS) as CuentaDepreciacion,
	   f.cod_familia,
	   (select c3.Oid from ConCatalogo c3
	     where c3.Empresa = 3
		   and c3.CodigoCuenta = f.cta_gasto collate SQL_Latin1_General_CP1_CI_AS) as CuentaGasto,
	   f.vida_util, f.valor_residual, f.activo
  from siaf_sql_pruebas..AFI_FAMILIA f
 where f.familia_padre is null

-- 2.2. Migrar las familias con padre diferente de nulo
        -- los codigos padre de 5 digitos primero, luego los código padre de 7 digitos
	insert into ActivoCategoria 
			  (Nombre, Padre, CuentaActivo, CuentaDepreciacion, Codigo, CuentaGasto, VidaUtil, ValorResidual, Activo)
	select f.descripcion, 
		   (select Oid from ActivoCategoria x
			 where x.Codigo = f.familia_padre collate SQL_Latin1_General_CP1_CI_AS) as Padre,
		   (select c1.Oid from ConCatalogo c1
			 where c1.Empresa = 3
			   and c1.CodigoCuenta = f.cta_activo collate SQL_Latin1_General_CP1_CI_AS) as CuentaActivo,
		   (select c2.Oid from ConCatalogo c2
			 where c2.Empresa = 3
			   and c2.CodigoCuenta = f.cta_depre collate SQL_Latin1_General_CP1_CI_AS) as CuentaDepreciacion,
		   f.cod_familia,
		   (select c3.Oid from ConCatalogo c3
			 where c3.Empresa = 3
			   and c3.CodigoCuenta = f.cta_gasto collate SQL_Latin1_General_CP1_CI_AS) as CuentaGasto,
		   f.vida_util, f.valor_residual, f.activo
	  from siaf_sql_pruebas..AFI_FAMILIA f
	 where f.familia_padre is not null
	   and len(f.familia_padre) = 7 -- len(f.familia_padre) = 5, len(f.familia_padre) = 7

--- 3. Copiar los proveedores que aparecen en el catálogo de Activo Fijo
--- excluir '00000000014'
--- reeemplazar '0614-161274-00' por '06141612740019'
--- reemplazar '200999' por '06142506041059'

insert into Tercero 
       (Nombre, TipoPersona, TipoContribuyente, DireccionPrincipal, EMail, Activo, ObjectType, UsuarioMod, Clasificacion, 
	    UsuarioCrea, FechaCrea)
select razon_social, iif(tipo_persona = 'J', 2, 1) as TipoPersona, 
       iif(tipo_contrib = 'G', 0, iif(tipo_contrib = 'E', 1, 2)) as TipoContribuyente,
	   null as DireccionPrincipal,
	   e_mail as EMail, activo,  1015 as ObjectType, 
	   iif(cod_entidad = '0614-161274-00', '06141612740019', iif(cod_entidad = '200999', '06142506041059', cod_entidad)) as UsuarioMod,
	   iif(clasif_contrib = 'CC001', 2, iif(clasif_contrib = 'CC002', 1, 0)) as Clasificacion,
	   Usuario_Crea, Fecha_Crea
  from siaf_sql_pruebas..ENTIDAD_EXTERNA
 where cod_entidad in ('02102603710016', '05111602991010', '06140109001021', '06140110091050', '06140206011031',
					   '06140209710022', '06140210750020', '06140304971035', '06140410061021', '06140512021016', 
					   '06140703011010', '06140704141066', '06140706021033', '06140710991020', '06140810901010',
					   '06140901041057', '06140901041111', '06141002011071', '06141003951019', '06141112041014',
					   '06141402031012', '06141512971029', '06141602770015', '06141602901011', '0614-161274-00',
					   '06141805850029', '06141811911016', '06141908981051', '06141912850013', '06142209141010',
					   '06142302051080', '06142306881010', '06142406870019', '06142408121010', '06142502081030',
					   '06142507891013', '06142807061055', '06142904941043', '06143103901020', '06143110790016',
					   '200999', '94832211011010')
--- 4. Generar la direccion de los terceros copiados
--- 4.1. Insertar la dirección
		insert into TerceroDireccion
			   (Tercero, Pais, Provincia, Ciudad, Direccion, Activa)
		select (select t.Oid from Tercero t
				 where t.UsuarioMod = x.cod_entidad collate SQL_Latin1_General_CP1_CI_AS) as Tercero,
			   iif(x.cod_pais = 'SV', 'SLV', iif(x.cod_pais = 'US', 'USA', null)) as Pais,
			   iif(x.cod_dpto = 'SS', 'SLV06', iif(x.cod_dpto = 'LL', 'SLV05', null)) as Departamento, -- SS, LL
			   iif (x.cod_muni = 'SS', 'SLV0614', iif(x.cod_muni = 'AC', 'SLV0501', null)) as Municipio, -- SS, AC
			   x.direccion, x.activo
		  from siaf_sql_pruebas..ENTIDAD_EXTERNA x
		 where x.cod_entidad in ('02102603710016', '05111602991010', '06140109001021', '06140110091050', '06140206011031',
							   '06140209710022', '06140210750020', '06140304971035', '06140410061021', '06140512021016', 
							   '06140703011010', '06140704141066', '06140706021033', '06140710991020', '06140810901010',
							   '06140901041057', '06140901041111', '06141002011071', '06141003951019', '06141112041014',
							   '06141402031012', '06141512971029', '06141602770015', '06141602901011', '0614-161274-00',
							   '06141805850029', '06141811911016', '06141908981051', '06141912850013', '06142209141010',
							   '06142302051080', '06142306881010', '06142406870019', '06142408121010', '06142502081030',
							   '06142507891013', '06142807061055', '06142904941043', '06143103901020', '06143110790016',
							   '200999', '94832211011010')
---5.2. Vincular al tercero como dirección principal
  update x
     set x.DireccionPrincipal = d.Oid 
    from Tercero x
   inner join TerceroDireccion d
      on x.Oid = d.Tercero
   where x.UsuarioMod in   ('02102603710016', '05111602991010', '06140109001021', '06140110091050', '06140206011031',
	   						'06140209710022', '06140210750020', '06140304971035', '06140410061021', '06140512021016', 
							'06140703011010', '06140704141066', '06140706021033', '06140710991020', '06140810901010',
							'06140901041057', '06140901041111', '06141002011071', '06141003951019', '06141112041014',
							'06141402031012', '06141512971029', '06141602770015', '06141602901011', '0614-161274-00',
							'06141805850029', '06141811911016', '06141908981051', '06141912850013', '06142209141010',
							'06142302051080', '06142306881010', '06142406870019', '06142408121010', '06142502081030',
							'06142507891013', '06142807061055', '06142904941043', '06143103901020', '06143110790016',
							'200999', '94832211011010')
---5.3. Insertar los documentos del tercero (NIT)
	insert into TerceroDocumento
		   (Tercero, Tipo, Numero, Vigente)
	select (select t.Oid from Tercero t
			 where t.UsuarioMod = x.cod_entidad collate SQL_Latin1_General_CP1_CI_AS) as Tercero, 'NIT',
		   x.cod_entidad, x.activo
	  from siaf_sql_pruebas..ENTIDAD_EXTERNA x
	 where x.cod_entidad in ('02102603710016', '05111602991010', '06140109001021', '06140110091050', '06140206011031',
						   '06140209710022', '06140210750020', '06140304971035', '06140410061021', '06140512021016', 
						   '06140703011010', '06140704141066', '06140706021033', '06140710991020', '06140810901010',
						   '06140901041057', '06140901041111', '06141002011071', '06141003951019', '06141112041014',
						   '06141402031012', '06141512971029', '06141602770015', '06141602901011', '0614-161274-00',
						   '06141805850029', '06141811911016', '06141908981051', '06141912850013', '06142209141010',
						   '06142302051080', '06142306881010', '06142406870019', '06142408121010', '06142502081030',
						   '06142507891013', '06142807061055', '06142904941043', '06143103901020', '06143110790016',
						   '200999', '94832211011010')
--- 6. Migrar los estados de uso de los activos
insert into Listas   
       (Codigo, Nombre, Categoria, Comentario, Activo)
select cod_valor, descripcion, 1 as categoria, null, activo 
  from siaf_sql_pruebas..PAR_LISTAS
 where tipo_lista = 3

--- 7. Migrar catalogo de activos
insert into ActivoCatalogo
       (Empresa, Codigo, Nombre, Categoria, Unidad, Empleado, Proveedor, FechaCompra, Marca, Modelo, NoSerie, InicioDepreciacion,
	    Moneda, ValorMoneda, VidaUtil, ValorCompra, ValorResidual, TotalDepreciacion, TotalMejora, TotalAjuste, EstadoDepreciacion,
		FechaDescarga, MesesGarantia, FechaInicioGarantia, EstadoUso, OrdenCompra, Observaciones)
select 3 as Empresa, left(trim(c.cod_activo), 20) as Codigo,  c.descripcion, 
       (select Oid from ActivoCategoria x
	     where x.Codigo = c.cod_familia collate SQL_Latin1_General_CP1_CI_AS) as Categoria,
	   floor(rand()*(21-10)+10) as Unidad, null as cod_emple, 
	   (select t.Oid from Tercero t
	     inner join TerceroDocumento d
		    on d.Tipo = 'NIT' and d.Tercero = t.Oid
		 where d.Numero = c.cod_proveedor collate SQL_Latin1_General_CP1_CI_AS) as Proveedor,
	   c.fecha_compra, c.marca, c.modelo, c.no_serie, c.fecha_ini_depre, c.cod_moneda, c.val_mone, c.vida_util, c.valor_original, 
	   c.valor_residual, c.acum_depre, c.acum_mejoras, c.acum_ajustes, 
	   iif(c.estado_activo = 1, 0, iif(c.estado_activo = 2, 1, 2)) as EstadoDepreciacion, 
	   c.fecha_descarga, c.periodo_garantia, c.fecha_ini_gar, c.estado_uso, c.num_ordenc, c.observaciones
 from siaf_sql_pruebas..AFI_CATALOGO c
go

select * from ConCatalogo
 where Empresa = 3

select * from Empresa

select max(Oid) from ConCatalogo

dbcc checkident('ConCatalogo', reseed, 1387)



