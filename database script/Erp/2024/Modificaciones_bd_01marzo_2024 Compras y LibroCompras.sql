alter table TerceroGiro
  add Sector smallint default 0 null
go
alter table CompraFactura
  add TipoOperacion smallint default 0 null,
      ClasificacionRenta smallint default 0 null,
      TipoCostoGasto smallint default 0 null
go
alter table CompraFactura
  add ProveedorGiro int default 0 null
go

alter table LibroCompra
  add TipoOperacion smallint default 0 null,
      ClasificacionRenta smallint default 0 null,
	  Sector smallint default 0 null,
      TipoCostoGasto smallint default 0 null