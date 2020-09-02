select * from Enfermedad
 where Categoria = 2

select *, LEN(ltrim(rtrim(CodCie))) as largo, substring(CodCie, 1, 4) as cod2 from TMP_CIE_10
 where LEN(ltrim(rtrim(CodCie))) > 4

update TMP_CIE_10
    set CodCie = rtrim(CodCie)
go

select * from TMP_CVX_VACUNA

select * from Enfermedad
 where Nombre like '%Asma%'