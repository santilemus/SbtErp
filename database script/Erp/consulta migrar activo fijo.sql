select * from siaf_sql_pruebas..AFI_CATALOGO 
 where cod_familia in (select cod_familia from siaf_sql_pruebas..AFI_FAMILIA where activo = 1)
sp_help ActivoCategoria




insert into ActivoCatalogo
       (Empresa, Codigo, Unidad, Moneda, Categoria, Empleado, FechaCompra, 
	    Nombre, Marca, Modelo, NoSerie, InicioDepreciacion, VidaUtil, ValorCompra, ValorResidual, EstadoDepreciacion,
		Observaciones, MesesGarantia, FechaDescarga, TotalDepreciacion, TotalMejora, TotalAjuste,
		EstadoUso, FechaInicioGarantia, ValorMoneda)
select 2 as emp, substring(c.cod_activo, 1, 20) as cod_activo, iif(c.cod_unidad > 9, 9, c.cod_unidad), c.cod_moneda, 
       coalesce((select top 1 Oid from ActivoCategoria where Codigo = c.cod_familia COLLATE DATABASE_DEFAULT), null),
	   c.cod_emple, 
	   c.fecha_compra,
       c.descripcion, substring(c.marca, 1, 40), substring(c.modelo, 1, 40), 
	   c.no_serie, c.fecha_ini_depre, c.vida_util, c.valor_original, c.valor_residual, 
	   c.estado_activo, c.observaciones, c.periodo_garantia, c.fecha_descarga, c.acum_depre,
	   c.acum_mejoras, c.acum_ajustes, c.estado_uso, c.fecha_ini_gar, c.val_mone
  from siaf_sql_pruebas..AFI_CATALOGO c
 where c.cod_emp = 1


select * from ActivoCategoria

01152020101010001

 select top 1 Oid from ConCatalogo where substring(CodigoCuenta, 1, 1) = '1'

 select * from Empleado

 