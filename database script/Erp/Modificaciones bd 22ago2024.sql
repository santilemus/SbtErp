select * from Constante

/*
CAT-001. Ambiente Destino  -- Archivo de Configuracion
	00 Pruebas
	01 Produccion

CAT-002. Tipo de Documento
	01 Factura
	03 Comprobante de cr�dito fiscal
	04 Nota de remisi�n
	05 Nota de cr�dito
	06 Nota de d�bito
	07 Comprobante de retenci�n
	08 Comprobante de liquidaci�n
	09 Documento contable de liquidaci�n
	11 Facturas de exportaci�n
	14 Factura de sujeto excluido
	15 Comprobante de donaci�n

CAT-003. Modelo de Facturaci�n
	1 Modelo Facturaci�n previo
	2 Modelo Facturaci�n diferido 

CAT-004. Tipo de Transmisi�n
	1 Transmisi�n normal
	2 Transmisi�n por contingencia

CAT-005 Tipo de Contingencia
	1 No disponibilidad de sistema del MH
	2 No disponibilidad de sistema del emisor
	3 Falla en el suministro de servicio de Internet del Emisor
	4 Falla en el suministro de servicio de energ�a el�ctrica del emisor
	que impida la transmisi�n de los DTE
	5 Otro (deber� digitar un m�ximo de 500 caracteres explicando el motivo) 

CAT-006 Retenci�n IVA MH
	22 Retenci�n IVA 1%
	C4 Retenci�n IVA 13%
	C9 Otras retenciones IVA casos especiales 

CAT-007 Tipo de Generaci�n del Documento
	1 F�sico
	2 Electr�nico

CAT-009 Tipo de establecimiento
	01 Sucursal / Agencia
	02 Casa matriz
	04 Bodega
	07 Predio y/o patio
	20 Otro 

CAT-011 Tipo de �tem
	1 Bienes
	2 Servicios
	3 Ambos (Bienes y Servicios, incluye los dos inherente a los Productos o servicios)
	4 Otros tributos por �tem 

CAT-012 Departamentos (no va en nuestro caso ZonaGeografica)
CAT-013 Municipios (no va en nuestro caso ZonaGeografica)
CAT-014 Unidad de Medida => UnidadMedida
CAT-015 Tributos. 
Modificar la tabla tributo para agregar el c�digo MH, debe ser �nico

CAT-016 Condici�n de la Operaci�n
	1 Contado 
	2 A cr�dito
	3 Otro 

Los valores actuales de la enumeraci�n son:
	Contado = 0,
	Credito = 1,
	Otro = 3  -- agregado por Dte

	Evaluar si se hace un update y se cambian los valores. Parece que eso procede. Tomar en cuenta que pueden haber errores

CAT-017 Forma de Pago
	01 Billetes y monedas
	02 Tarjeta D�bito
	03 Tarjeta Cr�dito
	04 Cheque
	05 Transferencia_ Dep�sito Bancario
	06 Vales o Cupones
	08 Dinero electr�nico
	09 Monedero electr�nico
	10 Certificado o tarjeta de regalo
	11 Bitcoin
	12 Otras Criptomonedas
	13 Cuentas por pagar del receptor
	14 Giro bancario
	99 Otros (se debe indicar el medio de pago) 

NOTA: Se tiene que agregar columna para el valor de equivalencia en la aplicaci�n o se hace un update a la bd
y se implementa tabla


CAT-018 Plazo

	01 D�as
	02 Meses
	03 A�os 

Nota: No existe actualmente en el erp, evaluar si es necesaria


CAT-019 C�digo de Actividad Econ�mica
ya esta implementado y es similar


CAT-020 PA�S
Modificar el cat�logo de zonas geogr�ficas para agregar codigo de pa�s seg�n MH

CAT-021 Otros Documentos Asociados
	1 Emisor
	2 Receptor
	3 M�dico (solo aplica para contribuyentes obligados a la presentaci�n de F-958)
	4 Transporte (solo aplica para Factura de exportaci�n) 


CAT-022 Tipo de documento de identificaci�n del Receptor
	36 NIT
	13 DUI 
	37 Otro
	03 Pasaporte
	02 Carnet de Residente

CAT-023 Tipo de Documento en Contingencia
	01 Factura Electr�nico
	03 Comprobante de Cr�dito Fiscal Electr�nico
	04 Nota de Remisi�n Electr�nica
	05 Nota de Cr�dito Electr�nica
	06 Nota de D�bito Electr�nica
	11 Factura de Exportaci�n Electr�nica
	14 Factura de Sujeto Excluido Electr�nica 

CAT-024 Tipo de Invalidaci�n
1 Error en la Informaci�n del Documento Tributario Electr�nico a invalidar.
2 Rescindir de la operaci�n realizada.
3 Otro 

CAT-025 T�tulo a que se remiten los bienes
	01 Dep�sito
	02 Propiedad
	03 Consignaci�n
	04 Traslado
	05 Otros 

CAT-026 Tipo de Donaci�n
	1 Efectivo
	2 Bien
	3 Servicio 

CAT-027 Recinto fiscal
-- no lo usamos, evaluar si se agrega

CAT-028 R�gimen
-- no se utiliza, evaluar si se agrega

CAT-029 Tipo de persona
	1 Persona Natural
	2 Persona Jur�dica

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

-- 4. Modificar ZonaGeografica para agregar codigo de pais de cat�logo 020 de 
--    normas para la emisi�n de facturas electr�nicas

alter table ZonaGeografica
  add CodigoPaisMH varchar(4) null
go

-- 5. Crear tabla para los cat�logos dte
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que corresponde a los cat�logos de apoyo del sistema de transmisi�n de Dte, excepto Actividad Economica, Pais, Departamento, Municipio, Tributos, Regimen',
   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Oid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificar los diferentes cat�logos del sistema de transmisi�n' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Tipo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'C�digo del registro correspondiente al cat�logo identificado de IdCatalogo' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Codigo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concepto o etiqueta' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Concepto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor correspondiente en el ERP' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Equivalente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica cuando el registro est� activo' , 
  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Activo'
GO

-- 6. Llenar la tabla DteCatalogo con los registros que utilizamos
insert into DteCatalogo  
       (Tipo, Codigo, Concepto, Equivalente, Activo)
values (1, '00', 'Pruebas', null, 1),
	(1, '01', 'Produccion', null, 1),
	(2, '01', 'Factura', 'COVE02', 1),
	(2, '03', 'Comprobante de cr�dito fiscal', 'COVE01', 1),
	(2, '04', 'Nota de remisi�n', 'COVE08', 1),
	(2, '05', 'Nota de cr�dito', 'DACV02', 1),
	(2, '06', 'Nota de d�bito', 'DACV03', 1),
	(2, '07', 'Comprobante de retenci�n', 'COVE09', 1),
	(2, '08', 'Comprobante de liquidaci�n', 'COVE07', 1),
	(2, '09', 'Documento contable de liquidaci�n', null, 1),
	(2, '11', 'Facturas de exportaci�n', 'COVE03', 1),
	(2, '14', 'Factura de sujeto excluido', 'COVE06', 1),
	(2, '15', 'Comprobante de donaci�n', null, 1),
	(3, '1', 'Modelo Facturaci�n previo', null, 1),
	(3, '2', 'Modelo Facturaci�n diferido', null, 1),
	(4, '1', 'Transmisi�n normal', null, 1),
	(4, '2', 'Transmisi�n por contingencia', null, 1),
	(5, '1', 'No disponibilidad de sistema del MH', null, 1),
	(5, '2', 'No disponibilidad de sistema del emisor', null, 1),
	(5, '3', 'Falla en el suministro de servicio de Internet del Emisor', null, 1),
	(5, '4', 'Falla suministro de servicio de energ�a el�ctrica del emisor', null, 1),
	(5, '5', 'Otro (m�ximo de 500 caracteres explicando el motivo)', null, 1),
	(6, '22', 'Retenci�n IVA 1%', null, 1),
	(6, 'C4', 'Retenci�n IVA 13%', null, 1),
	(6, 'C9', 'Otras retenciones IVA casos especiales', null, 1),	
	(7, '1', 'F�sico', null, 1),
	(7, '2', 'Electr�nico', null, 1),	
	(9, '01', 'Sucursal / Agencia', null, 1),
	(9, '02', 'Casa matriz', null, 1),
	(9, '04', 'Bodega', null, 1),	
	(9, '07', 'Predio y/o patio', null, 1),
	(9, '20', 'Otro', null, 1),	
	(11, '1', 'Bienes', null, 1),
	(11, '2', 'Servicios', null, 1),	
	(11, '3', 'Ambos (Bienes y Servicios)', null, 1),
	(11, '4', 'Otros tributos por �tem', null, 1),	
	(16, '1', 'Contado', '0', 1),
	(16, '2', 'A cr�dito', '1', 1),	
	(16, '3', 'Otro', '2', 1),
	(17, '01', 'Billetes y monedas', '0', 1),
	(17, '02', 'Tarjeta D�bito', '2', 1),
	(17, '03', 'Tarjeta Cr�dito', '2', 1),
	(17, '04', 'Cheque', '1', 1),
	(17, '05', 'Transferencia_ Dep�sito Bancario', '3', 1),
	(17, '06', 'Vales o Cupones', '4', 1),
	(17, '08', 'Dinero electr�nico', '4', 1),
	(17, '09', 'Monedero electr�nico', '4', 1),
	(17, '10', 'Certificado o tarjeta de regalo', '4', 1),
	(17, '11', 'Bitcoin', '4', 1),
	(17, '12', 'Otras Criptomonedas', '4', 1),
	(17, '13', 'Cuentas por pagar del receptor', '4', 1),
	(17, '14', 'Giro bancario', '3', 1),
	(17, '99', 'Otros (se debe indicar el medio de pago)', '4', 1),
	(18, '01', 'D�as', null, 1),
	(18, '02', 'Meses', null, 1),
	(18, '03', 'A�os', null, 1),	
	(21, '1', 'Emisor', null, 1),
	(21, '2', 'Receptor', null, 1),	
	(21, '3', 'M�dico (Contribuyentes obligados a presentar F-958)', null, 1),
	(21, '4', 'Transporte (solo aplica para Factura de exportaci�n)', null, 1),	
	(22, '36', 'NIT', 'NIT', 1),
	(22, '13', 'DUI', 'DUI', 1),
	(22, '37', 'Otro', null, 1),	
	(22, '03', 'Pasaporte', 'PAS', 1),
	(22, '02', 'Carnet de Residente', 'RES', 1),
	(23, '01', 'Factura Electr�nico', 'COVE02', 1),
	(23, '03', 'Comprobante de Cr�dito Fiscal Electr�nico', 'COVE03', 1),	
	(23, '04', 'Nota de Remisi�n Electr�nica', 'COVE08', 1),
	(23, '05', 'Nota de Cr�dito Electr�nica', 'DACV02', 1),
	(23, '06', 'Nota de D�bito Electr�nica', 'DACV01', 1),	
	(23, '11', 'Factura de Exportaci�n Electr�nica', 'COVE03', 1),
	(23, '14', 'Factura de Sujeto Excluido Electr�nica', 'COVE06', 1),
	(24, '1', 'Error en la Informaci�n del Dte a invalidar', null, 1),	
	(24, '2', 'Rescindir de la operaci�n realizada', null, 1),
	(24, '3', 'Otro', null, 1),
	(25, '01', 'Dep�sito', null, 1),
	(25, '02', 'Propiedad', null, 1),
	(25, '03', 'Consignaci�n', null, 1),	
	(25, '04', 'Traslado', null, 1),
	(25, '05', 'Otros', null, 1),
	(26, '1', 'Efectivo', null, 1),	
	(26, '2', 'Bien', null, 1),
	(26, '3', 'Servicio', null, 1),
	(29, '1', 'Persona Natural', '1', 1),	
	(29, '2', 'Persona Jur�dica', '2', 1)
go

