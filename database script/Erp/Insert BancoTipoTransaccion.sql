
set identity_insert BancoTipoTransaccion on
go
insert into BancoTipoTransaccion
      (Oid, Nombre, Tipo, Activa)
values 
(1, 'Remesa Cobro Venta de Credito', 2, 1),
(2, 'Remesa Transferencia entre Cuentas Propias', 2, 1),
(3, 'Remesa Venta de Contado', 2, 1),
(4, 'Remesas Varias', 2, 1),
(5, 'Nota Abono Cobro Venta de Credito', 1, 1),
(6, 'Nota Abono Transferencia entre Cuentas Propias', 1, 1),
(7, 'Nota Abono Venta de Contado', 1, 1),
(8, 'Nota Abono Transferencia Terceros', 1, 1),
(9, 'Nota Abono Varias', 1, 1),
(10, 'Nota Abono Operaciones Financieras', 1, 1),
(11, 'Cheque Pago a Proveedor', 3, 1),
(12, 'Cheque Pago de Salario', 3, 1),
(13, 'Cheque Tranferencia entre Cuentas Propias', 3, 1),
(14, 'Cheque Pago Impuestos', 3, 1),
(15, 'Cheque Gastos Varios', 3, 1),
(16, 'Nota Cargo Pago a Proveedor', 4, 1),
(17, 'Nota Cargo Pago de Salario', 4, 1),
(18, 'Nota Cargo Transferencia entre Cuentas Propias', 4, 1),
(19, 'Nota Cargo Pago Impuestos', 4, 1),
(20, 'Nota Cargo Transferencia Terceros', 4, 1),
(21, 'Nota Cargo Varias', 4, 1),
(22, 'Nota Cargo Operaciones Financieras', 4, 1)
go
set identity_insert BancoTipoTransaccion off
go