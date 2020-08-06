select * from syntepro_siaf3..PLA_OPERAC
 where cod_operac = 3
select * from syntepro_siaf3..PLA_PLANI_OPERA po
 inner join syntepro_siaf3..PLA_TIPO_PLANI t
    on po.tipo_plani = t.tipo_plani
 where po.cod_operac = 3

