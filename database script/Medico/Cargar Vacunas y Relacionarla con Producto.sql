insert Vacuna
       (Medicamento, Nombre, CodigoPT, DescripcionCPT, CodigoCVX, Comentario, UsuarioCrea, FechaCrea)
select case 
         when CodigoPT in (/*90389, */90714, 90715, 90718) then 180
		 when CodigoPT in (90696, 90697, 90698, 90700, 90701) then 181
		 when CodigoPT = 90732 then 182
		 when CodigoPT = 90703 then 184
		 when CodigoPT = 90702 then 185
		 when CodigoPT in (90585, 90728) then 186
		 when CodigoPT in (90654, 90655, 90656, 90657, 90658, 90659, 90660, 90661, 90662, 90663, 90664,
		                   90666, 90668, 90672, 90673, 90674, 90682, 90685, 90686, 90687, 90688, 90724,
						   90756) then 187
		 when CodigoPT = 90712 then 188
		 when CodigoPT = 90713 then 189
		 when CodigoPT in (90375, 90376, 90726) then 190
		 when CodigoPT in (90680, 90681) then 191
		 when CodigoPT in (90704, 90705, 90706, 90710, 90708, 90707) then 192
		 when CodigoPT in (90371, 90731, 90743, 90744, 90745, 90746, 90747, 90748) then 193
		 else null end, Nombre, CodigoPT, DescripcionCPT, CodigoCVX, Comentario, current_user, current_timestamp
  from TMP_CVX_VACUNA

  
