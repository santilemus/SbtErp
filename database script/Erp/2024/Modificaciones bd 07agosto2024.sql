select * from ReportDataV2
 where Oid = '3D68127D-2591-4D0B-A5C8-2DBEBA052813'

update ReportDataV2
   set ObjectTypeName = 'SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion'
 where Oid = '3D68127D-2591-4D0B-A5C8-2DBEBA052813'

Select * from XPObjectType
 where AssemblyName = 'SBT.Apps.Facturacion.Module'


select * from XPObjectType
 where TypeName like '%CxC%'

-- 1. insertar CxCTransaccion BO en XPObjectType
Insert into XPObjectType
       (TypeName, AssemblyName)
values ('SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion', 'SBT.Apps.Facturacion.Module')
go

-- 2. Obtener la información de la impresión de la nota de crédito y crear el insert
select * from ReportDataV2
 where Oid = '3D68127D-2591-4D0B-A5C8-2DBEBA052813'

 -- 3. Cambios a la tabla de Partidas contables y Partida Detalle

 -- 4. Cambios a la tabla de Cuentas por Cobrar (CXCTransaccion y CxcDocumento, CxcDocumentoDetalle

 -- 5. Borrar de Listas, valores que no son válidos (revisar los que se borraron y no se documento
 
 select * from Listas 

 -- 6. Revisar los registros de CxCTipoTransaccion (relación padre, porque se deben corregir inconsistencias

 select * from CxCTipoTransaccion
   

