select * from Medico..Listas
-- grupo sanguineo Categoria = 2
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('GSA01', 'A Rh+', 2, null, 1, 'Admin', current_timestamp),
       ('GSA02', 'A Rh-', 2, null, 1, 'Admin', current_timestamp),
       ('GSA03', 'B Rh+', 2, null, 1, 'Admin', current_timestamp),
       ('GSA04', 'B Rh-', 2, null, 1, 'Admin', current_timestamp),
       ('GSA05', 'AB Rh+', 2, null, 1, 'Admin', current_timestamp),
       ('GSA06', 'AB Rh-', 2, null, 1, 'Admin', current_timestamp),
       ('GSA07', 'O Rh+', 2, null, 1, 'Admin', current_timestamp),
       ('GSA08', 'O Rh-', 2, null, 1, 'Admin', current_timestamp)
go
-- Tipos de seguro - Categoria = 4
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('SEG001', 'Medico Hospitalario Individual', 4, null, 1, 'Admin', current_timestamp),
       ('SEG002', 'Medico Hospitalario Familiar', 4, null, 1, 'Admin', current_timestamp),
       ('SEG003', 'Accidente', 4, null, 1, 'Admin', current_timestamp)
go
-- Forma de Pago - Categoria = 5
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('FPA01', 'Efectivo', 5, null, 1, 'Admin', current_timestamp),
       ('FPA02', 'Cheque', 5, null, 1, 'Admin', current_timestamp),
       ('FPA03', 'Tarjeta Crédito o Débito', 5, null, 1, 'Admin', current_timestamp),
       ('FPA04', 'Transferencia Electrónica', 5, null, 1, 'Admin', current_timestamp)
go
-- Tipo de Tarjeta - Categoria = 6
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('TTA01', 'Crédito', 6, null, 1, 'Admin', current_timestamp),
       ('TTA02', 'Débito', 6, null, 1, 'Admin', current_timestamp),
       ('TTA03', 'E-Card (Virtual)', 6, null, 1, 'Admin', current_timestamp)
go
-- Estado de Empleados - Categoria = 9
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('EMPL01', 'Activo', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL02', 'Retirado', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL03', 'Permiso sin goce de Sueldo', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL04', 'Permiso con goce de Sueldo', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL05', 'Incapacidad', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL06', 'Maternidad', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL07', 'SubContrato', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL08', 'Despedido', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL09', 'Contrato Suspendido', 9, null, 1, 'Admin', current_timestamp),
       ('EMPL10', 'Otro', 9, null, 1, 'Admin', current_timestamp)
go
-- Tipo de Documento de identidad - Categoria = 10
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('DUI', 'Documento Unico de Identidad', 10, null, 1, 'Admin', current_timestamp),
       ('NIT', 'No Identificación Tributaria', 10, null, 1, 'Admin', current_timestamp),
       ('LIC', 'Licencia de Conducir', 10, null, 1, 'Admin', current_timestamp),
       ('PAS', 'Pasaporte', 10, null, 1, 'Admin', current_timestamp),
       ('RES', 'Carne Residente', 10, null, 1, 'Admin', current_timestamp),
	   ('NUP', 'No Unico Previsional', 10, null, 1, 'Admin', current_timestamp),
	   ('NIS', 'No Afiliado Seguro Social', 10, null, 1, 'Admin', Current_timestamp),
	   ('NRC', 'No Registro Contribuyente', 10, null, 1, 'Admin', Current_timestamp)
go
-- Tipos de Parientes - Categoria = 12
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PAR01', 'Padres', 12, null, 1, 'Admin', current_timestamp),
       ('PAR02', 'Hijo(a)', 12, null, 1, 'Admin', current_timestamp),
       ('PAR03', 'Esposo(a)', 12, null, 1, 'Admin', current_timestamp),
       ('PAR04', 'Hermano(a)', 12, null, 1, 'Admin', current_timestamp),
       ('PAR05', 'Otro(a)', 12, null, 1, 'Admin', current_timestamp)
go
-- Atributos de los productos - Categoria = 13
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('ATR001', 'Color', 13, null, 1, 'Admin', current_timestamp),
       ('ATR002', 'Modelo', 13, null, 1, 'Admin', current_timestamp),
       ('ATR003', 'Olor', 13, null, 1, 'Admin', current_timestamp),
       ('ATR004', 'Dimensiones (L-W-H)', 13, null, 1, 'Admin', current_timestamp),
       ('ATR005', 'Peso', 13, null, 1, 'Admin', current_timestamp),
       ('ATR006', 'Ancho', 13, null, 1, 'Admin', current_timestamp),
       ('ATR007', 'Composición', 13, null, 1, 'Admin', current_timestamp),
       ('ATR008', 'Duración', 13, null, 1, 'Admin', current_timestamp),
       ('ATR009', 'Envase', 13, null, 1, 'Admin', current_timestamp),
       ('ATR010', 'Embalaje', 13, null, 1, 'Admin', current_timestamp),
       ('ATR011', 'Velocidad', 13, null, 1, 'Admin', current_timestamp),
       ('ATR012', 'Temperatura', 13, null, 1, 'Admin', current_timestamp)
go
-- Tipos de precios de productos - Categoria = 14
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('TPR001', 'General - Detalle', 14, null, 1, 'Admin', current_timestamp),
       ('TPR002', 'Mayoreo', 14, null, 1, 'Admin', current_timestamp),
       ('TPR003', 'Promoción', 14, null, 1, 'Admin', current_timestamp)
go
-- Tipos de Formularios de Compra y Venta - Categoria = 15
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('COVE01', 'Crédito Fiscal', 15, null, 1, 'Admin', current_timestamp),
       ('COVE02', 'Consumidor Final', 15, null, 1, 'Admin', current_timestamp),
       ('COVE03', 'Factura de Exportación', 15, null, 1, 'Admin', current_timestamp),
       ('COVE04', 'Factura Simplificada', 15, null, 1, 'Admin', current_timestamp),
       ('COVE05', 'Ticket de Venta', 15, null, 1, 'Admin', current_timestamp),
       ('COVE06', 'Sujeto Excluido', 15, null, 1, 'Admin', current_timestamp),
	   ('COVE07', 'Comprobante Liquidaciónn', 15, null, 1, 'Admin', current_timestamp),
       ('COVE08', 'Nota de Remisión', 15, null, 1, 'Admin', current_timestamp),
       ('COVE09', 'Comprobante de Retención', 15, null, 1, 'Admin', current_timestamp),
       ('COVE10', 'Recibo (Caja Chica)', 15, null, 1, 'Admin', current_timestamp),
       ('COVE11', 'Vale (Caja Chica)', 15, null, 1, 'Admin', current_timestamp)
go
-- Tipos de Formularios Aplicados a las Compras y Ventas - Categoria = 16
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('DACV01', 'Nota de Débito', 16, null, 1, 'Admin', current_timestamp),
       ('DACV02', 'Nota de Crédito', 16, null, 1, 'Admin', current_timestamp),
       ('DACV03', 'Ticket de Devolución', 16, null, 1, 'Admin', current_timestamp)
go
-- Condicion Pago - Categoria = 17
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('CPA01', 'Contado', 17, null, 1, 'Admin', current_timestamp),
       ('CPA02', 'Credito', 17, null, 1, 'Admin', current_timestamp)
go
-- Transacciones de Planilla de Ingresos Gravados - Categoria = 18
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('RHTI01', 'Otro Ingreso Gravado (Isss, AFP, Renta)', 18, null, 1, 'Admin', current_timestamp),
       ('RHTI02', 'Ingreso Gravado Renta', 18, null, 1, 'Admin', current_timestamp)
go
-- Transacciones de Planilla de Ingresos No Gravados - Categoria = 19
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('RHTE01', 'Viáticos', 19, null, 1, 'Admin', current_timestamp),
       ('RHTE02', 'Depreciación', 19, null, 1, 'Admin', current_timestamp),
	   ('RHTE03', 'Comunicación', 19, null, 1, 'Admin', current_timestamp),
       ('RHTE04', 'Otro Ingreso No Gravado', 19, null, 1, 'Admin', current_timestamp)
go
-- Transacciones de Planilla Descuentos - Categoria = 20
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('RHTD01', 'Préstamo', 20, null, 1, 'Admin', current_timestamp),
       ('RHTD02', 'Seguro', 20, null, 1, 'Admin', current_timestamp),
	   ('RHTD03', 'Embargo', 20, null, 1, 'Admin', current_timestamp),
       ('RHTD04', 'Donación', 20, null, 1, 'Admin', current_timestamp),
	   ('RHTD05', 'Otros Descuentos', 20, null, 1, 'Admin', current_timestamp)
go

-- Tipos de Jornadas - Categoria = 21
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('RHJL01', 'Diurna', 21, null, 1, 'Admin', current_timestamp),
       ('RHJL02', 'Nocturna', 21, null, 1, 'Admin', current_timestamp),
       ('RHJL03', 'Diurna Día Descanso', 21, null, 1, 'Admin', current_timestamp),
	   ('RHJL04', 'Nocturna Día Descanso', 21, null, 1, 'Admin', current_timestamp),
	   ('RHJL05', 'Diurna Asueto', 21, null, 1, 'Admin', current_timestamp),
	   ('RHJL06', 'Nocturna Asueto', 21, null, 1, 'Admin', current_timestamp),
	   ('RHJL07', 'Extra Diurna Dia Descanso', 21, null, 1, 'Admin', current_timestamp),
       ('RHJL08', 'Extra Nocturna Día Descanso', 21, null, 1, 'Admin', current_timestamp)
go
-- Impuestos adicionales al IVA - Categoria = 18  -- NO VA, por el momento
/*
insert into dbo.Listas 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('IMP01', 'DAI', 18, null, 1, 'Admin', current_timestamp),
       ('IMP02', 'ISC', 18, null, 1, 'Admin', current_timestamp),
       ('IMP03', 'FOVIAL', 18, null, 1, 'Admin', current_timestamp),
       ('IMP04', 'ISC', 18, null, 1, 'Admin', current_timestamp),  --Impuesto selectivo al consumo
       ('IMP05', 'CESC', 18, null, 1, 'Admin', current_timestamp), --Contribucion especial a la seguridad ciudadana
	   ('IMP06', 'COTRANS', 18, null, 1, 'Admin', current_timestamp), -- Contribucion al transporte
       ('IMP07', 'FEFE', 18, null, 1, 'Admin', current_timestamp) -- Fondo de estabilizacion y fomento economico
*/

--