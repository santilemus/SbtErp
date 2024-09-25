select * from Constante

/*
CAT-001. Ambiente Destino  -- Archivo de Configuracion
	00 Pruebas
	01 Produccion

CAT-002. Tipo de Documento
	01 Factura
	03 Comprobante de crédito fiscal
	04 Nota de remisión
	05 Nota de crédito
	06 Nota de débito
	07 Comprobante de retención
	08 Comprobante de liquidación
	09 Documento contable de liquidación
	11 Facturas de exportación
	14 Factura de sujeto excluido
	15 Comprobante de donación

CAT-003. Modelo de Facturación
	1 Modelo Facturación previo
	2 Modelo Facturación diferido 

CAT-004. Tipo de Transmisión
	1 Transmisión normal
	2 Transmisión por contingencia

CAT-005 Tipo de Contingencia
	1 No disponibilidad de sistema del MH
	2 No disponibilidad de sistema del emisor
	3 Falla en el suministro de servicio de Internet del Emisor
	4 Falla en el suministro de servicio de energía eléctrica del emisor
	que impida la transmisión de los DTE
	5 Otro (deberá digitar un máximo de 500 caracteres explicando el motivo) 

CAT-006 Retención IVA MH
	22 Retención IVA 1%
	C4 Retención IVA 13%
	C9 Otras retenciones IVA casos especiales 

CAT-007 Tipo de Generación del Documento
	1 Físico
	2 Electrónico

CAT-009 Tipo de establecimiento
	01 Sucursal / Agencia
	02 Casa matriz
	04 Bodega
	07 Predio y/o patio
	20 Otro 

CAT-011 Tipo de ítem
	1 Bienes
	2 Servicios
	3 Ambos (Bienes y Servicios, incluye los dos inherente a los Productos o servicios)
	4 Otros tributos por ítem 

CAT-012 Departamentos (no va en nuestro caso ZonaGeografica)
CAT-013 Municipios (no va en nuestro caso ZonaGeografica)
CAT-014 Unidad de Medida => UnidadMedida
CAT-015 Tributos. 
Modificar la tabla tributo para agregar el código MH, debe ser único

CAT-016 Condición de la Operación
	1 Contado 
	2 A crédito
	3 Otro 

Los valores actuales de la enumeración son:
	Contado = 0,
	Credito = 1,
	Otro = 3  -- agregado por Dte

	Evaluar si se hace un update y se cambian los valores. Parece que eso procede. Tomar en cuenta que pueden haber errores

CAT-017 Forma de Pago
	01 Billetes y monedas
	02 Tarjeta Débito
	03 Tarjeta Crédito
	04 Cheque
	05 Transferencia_ Depósito Bancario
	06 Vales o Cupones
	08 Dinero electrónico
	09 Monedero electrónico
	10 Certificado o tarjeta de regalo
	11 Bitcoin
	12 Otras Criptomonedas
	13 Cuentas por pagar del receptor
	14 Giro bancario
	99 Otros (se debe indicar el medio de pago) 

NOTA: Se tiene que agregar columna para el valor de equivalencia en la aplicación o se hace un update a la bd
y se implementa tabla


CAT-018 Plazo

	01 Días
	02 Meses
	03 Años 

Nota: No existe actualmente en el erp, evaluar si es necesaria


CAT-019 Código de Actividad Económica
ya esta implementado y es similar


CAT-020 PAÍS
Modificar el catálogo de zonas geográficas para agregar codigo de país según MH

CAT-021 Otros Documentos Asociados
	1 Emisor
	2 Receptor
	3 Médico (solo aplica para contribuyentes obligados a la presentación de F-958)
	4 Transporte (solo aplica para Factura de exportación) 


CAT-022 Tipo de documento de identificación del Receptor
	36 NIT
	13 DUI 
	37 Otro
	03 Pasaporte
	02 Carnet de Residente

CAT-023 Tipo de Documento en Contingencia
	01 Factura Electrónico
	03 Comprobante de Crédito Fiscal Electrónico
	04 Nota de Remisión Electrónica
	05 Nota de Crédito Electrónica
	06 Nota de Débito Electrónica
	11 Factura de Exportación Electrónica
	14 Factura de Sujeto Excluido Electrónica 

CAT-024 Tipo de Invalidación
1 Error en la Información del Documento Tributario Electrónico a invalidar.
2 Rescindir de la operación realizada.
3 Otro 

CAT-025 Título a que se remiten los bienes
	01 Depósito
	02 Propiedad
	03 Consignación
	04 Traslado
	05 Otros 

CAT-026 Tipo de Donación
	1 Efectivo
	2 Bien
	3 Servicio 

CAT-027 Recinto fiscal
-- no lo usamos, evaluar si se agrega

CAT-028 Régimen
-- no se utiliza, evaluar si se agrega

CAT-029 Tipo de persona
	1 Persona Natural
	2 Persona Jurídica

CAT-030 Transporte
-- no lo usamos, evaluar si se agrega

CAT-031 INCOTERMS
-- no lo usamos, evaluar si se agrega
*/



select * from ZonaGeografica

-- 1. Modificar catalogo de Unidad de Medida
select * from UnidadMedida
sp_help UnidadMedida

alter table UnidadMedida
 drop column UsuarioCrea, FechaCrea, UsuarioMod, FechaMod
go

alter table UnidadMedida
  drop constraint PK_UnidadMedida
go

alter table UnidadMedida
  drop column Oid
go
alter table UnidadMedida
  add Oid varchar(2) not null
go
alter table UnidadMedida
  add constraint PKUnidadMedida primary key(Oid)
go
alter table UnidadMedida
  alter column Magnitud numeric(22, 11)

-- 2. Cargar Catalogo de Unidad de Medida
-- C:\sistemas\SBT\Apps\database script\Erp\UnidadMedida Data 22ago2024.sql

-- 3. Modificar Tributo
alter table Tributo
  add CodigoDte varchar(2) null

-- 4. Modificar ZonaGeografica para agregar codigo de pais de catálogo 020 de 
--    normas para la emisión de facturas electrónicas

alter table ZonaGeografica
  add CodigoPaisMH varchar(4) null
go

-- 5. Crear tabla para los catálogos dte
create table DteCatalogo (
  Oid int identity(1,1) not null,
  Tipo smallint not null,
  Codigo varchar(4) not null,
  Concepto varchar(60) not null,
  Equivalente  varchar(6) null,
  Activo bit default 1 not null,
  OptimisticLockField int null,
  GCRecord int null,
  constraint PKDteCatalogo primary key (Oid))
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que corresponde a los catálogos de apoyo del sistema de transmisión de Dte, excepto Actividad Economica, Pais, Departamento, Municipio, Tributos, Regimen',
   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Oid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificar los diferentes catálogos del sistema de transmisión' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Tipo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del registro correspondiente al catálogo identificado de IdCatalogo' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Codigo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concepto o etiqueta' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Concepto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor correspondiente en el ERP' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Equivalente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica cuando el registro está activo' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Activo'
GO

-- 6. Llenar la tabla DteCatalogo con los registros que utilizamos
insert into DteCatalogo  
       (Tipo, Codigo, Concepto, Equivalente, Activo)
values (1, '00', 'Pruebas', null, 1),
	(1, '01', 'Produccion', null, 1),
	(2, '01', 'Factura', 'COVE02', 1),
	(2, '03', 'Comprobante de crédito fiscal', 'COVE01', 1),
	(2, '04', 'Nota de remisión', 'COVE08', 1),
	(2, '05', 'Nota de crédito', 'DACV02', 1),
	(2, '06', 'Nota de débito', 'DACV03', 1),
	(2, '07', 'Comprobante de retención', 'COVE09', 1),
	(2, '08', 'Comprobante de liquidación', 'COVE07', 1),
	(2, '09', 'Documento contable de liquidación', null, 1),
	(2, '11', 'Facturas de exportación', 'COVE03', 1),
	(2, '14', 'Factura de sujeto excluido', 'COVE06', 1),
	(2, '15', 'Comprobante de donación', null, 1),
	(3, '1', 'Modelo Facturación previo', null, 1),
	(3, '2', 'Modelo Facturación diferido', null, 1),
	(4, '1', 'Transmisión normal', null, 1),
	(4, '2', 'Transmisión por contingencia', null, 1),
	(5, '1', 'No disponibilidad de sistema del MH', null, 1),
	(5, '2', 'No disponibilidad de sistema del emisor', null, 1),
	(5, '3', 'Falla en el suministro de servicio de Internet del Emisor', null, 1),
	(5, '4', 'Falla suministro de servicio de energía eléctrica del emisor', null, 1),
	(5, '5', 'Otro (máximo de 500 caracteres explicando el motivo)', null, 1),
	(6, '22', 'Retención IVA 1%', null, 1),
	(6, 'C4', 'Retención IVA 13%', null, 1),
	(6, 'C9', 'Otras retenciones IVA casos especiales', null, 1),	
	(7, '1', 'Físico', null, 1),
	(7, '2', 'Electrónico', null, 1),	
	(9, '01', 'Sucursal / Agencia', null, 1),
	(9, '02', 'Casa matriz', null, 1),
	(9, '04', 'Bodega', null, 1),	
	(9, '07', 'Predio y/o patio', null, 1),
	(9, '20', 'Otro', null, 1),	
	(11, '1', 'Bienes', null, 1),
	(11, '2', 'Servicios', null, 1),	
	(11, '3', 'Ambos (Bienes y Servicios)', null, 1),
	(11, '4', 'Otros tributos por ítem', null, 1),	
	(16, '1', 'Contado', '0', 1),
	(16, '2', 'A crédito', '1', 1),	
	(16, '3', 'Otro', '2', 1),
	(17, '01', 'Billetes y monedas', '0', 1),
	(17, '02', 'Tarjeta Débito', '2', 1),
	(17, '03', 'Tarjeta Crédito', '2', 1),
	(17, '04', 'Cheque', '1', 1),
	(17, '05', 'Transferencia_ Depósito Bancario', '3', 1),
	(17, '06', 'Vales o Cupones', '4', 1),
	(17, '08', 'Dinero electrónico', '4', 1),
	(17, '09', 'Monedero electrónico', '4', 1),
	(17, '10', 'Certificado o tarjeta de regalo', '4', 1),
	(17, '11', 'Bitcoin', '4', 1),
	(17, '12', 'Otras Criptomonedas', '4', 1),
	(17, '13', 'Cuentas por pagar del receptor', '4', 1),
	(17, '14', 'Giro bancario', '3', 1),
	(17, '99', 'Otros (se debe indicar el medio de pago)', '4', 1),
	(18, '01', 'Días', null, 1),
	(18, '02', 'Meses', null, 1),
	(18, '03', 'Años', null, 1),	
	(21, '1', 'Emisor', null, 1),
	(21, '2', 'Receptor', null, 1),	
	(21, '3', 'Médico (Contribuyentes obligados a presentar F-958)', null, 1),
	(21, '4', 'Transporte (solo aplica para Factura de exportación)', null, 1),	
	(22, '36', 'NIT', 'NIT', 1),
	(22, '13', 'DUI', 'DUI', 1),
	(22, '37', 'Otro', null, 1),	
	(22, '03', 'Pasaporte', 'PAS', 1),
	(22, '02', 'Carnet de Residente', 'RES', 1),
	(23, '01', 'Factura Electrónico', 'COVE02', 1),
	(23, '03', 'Comprobante de Crédito Fiscal Electrónico', 'COVE03', 1),	
	(23, '04', 'Nota de Remisión Electrónica', 'COVE08', 1),
	(23, '05', 'Nota de Crédito Electrónica', 'DACV02', 1),
	(23, '06', 'Nota de Débito Electrónica', 'DACV01', 1),	
	(23, '11', 'Factura de Exportación Electrónica', 'COVE03', 1),
	(23, '14', 'Factura de Sujeto Excluido Electrónica', 'COVE06', 1),
	(24, '1', 'Error en la Información del Dte a invalidar', null, 1),	
	(24, '2', 'Rescindir de la operación realizada', null, 1),
	(24, '3', 'Otro', null, 1),
	(25, '01', 'Depósito', null, 1),
	(25, '02', 'Propiedad', null, 1),
	(25, '03', 'Consignación', null, 1),	
	(25, '04', 'Traslado', null, 1),
	(25, '05', 'Otros', null, 1),
	(26, '1', 'Efectivo', null, 1),	
	(26, '2', 'Bien', null, 1),
	(26, '3', 'Servicio', null, 1),
	(29, '1', 'Persona Natural', '1', 1),	
	(29, '2', 'Persona Jurídica', '2', 1)
go

