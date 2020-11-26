-- insertar las categorias o grupos de terminologia anatomica
insert into MedLista
        (Codigo, Nombre, Categoria, Activo, UsuarioCrea, FechaCrea)
values  ('A010', 'Cuerpo Humano', 1, 1, 'Admin', current_timestamp),
        ('A011', 'Partes del Cuerpo Humano', 1, 1, 'Admin', current_timestamp),
        ('A012', 'Planos L�neas y Regiones Anat�micas', 1, 1, 'Admin', current_timestamp),
        ('A020', 'Huesos; Sistema Esquel�tico', 1, 1, 'Admin', current_timestamp), 
		('A030', 'Articulaciones; Sistema Articular', 1, 1, 'Admin', current_timestamp), 
		('A040', 'M�sculos; Sistema Muscular', 1, 1, 'Admin', current_timestamp),
		('A050', 'Sistema Digestivo', 1, 1, 'Admin', current_timestamp),
        ('A060', 'Sistema Respiratorio', 1, 1, 'Admin', current_timestamp),
		('A070', 'Cavidad Tor�cica', 1, 1, 'Admin', current_timestamp),
		('A080', 'Sistema Urinario', 1, 1, 'Admin', current_timestamp),
		('A090', 'Sistema Genitales', 1, 1, 'Admin', current_timestamp),
		('A091', 'Sistemas Genital Femenino', 1, 1, 'Admin', current_timestamp),
		('A092', 'Sistemas Genital Masculino', 1, 1, 'Admin', current_timestamp),
		('A100', 'Cavidad Abdominal y de la Pelvis', 1, 1, 'Admin', current_timestamp),
		('A110', 'Gl�ndulas Endocrinas', 1, 1, 'Admin', current_timestamp),
		('A120', 'Sistema Cardiovascular', 1, 1, 'Admin', current_timestamp),
		('A121', 'Coraz�n', 1, 1, 'Admin', current_timestamp),
		('A122', 'Arterias', 1, 1, 'Admin', current_timestamp),
		('A123', 'Ventas', 1, 1, 'Admin', current_timestamp),
		('A130', 'Sistema Linf�tico', 1, 1, 'Admin', current_timestamp),
		('A140', 'Sistema Nervioso', 1, 1, 'Admin', current_timestamp),
		('A141', 'Sistema Nervioso Central', 1, 1, 'Admin', current_timestamp),
		('A142', 'Sistema nervioso Perif�rico', 1, 1, 'Admin', current_timestamp),
		('A143', 'Divisi�n Aut�noma. Porci�n Auton�mica del Sistema Nervioso Perif�rico', 1, 1, 'Admin', current_timestamp),
		('A150', '�rganos de los Sentidos', 1, 1, 'Admin', current_timestamp),
		('A160', 'Integumento Com�n', 1, 1, 'Admin', current_timestamp)
go
-- Intensidades ** Categoria = 2
insert into MedLista
        (Codigo, Nombre, Categoria, Activo, UsuarioCrea, FechaCrea)
values  ('IS001', 'Ninguno', 2, 1, 'Admin', current_timestamp),
        ('IS002', 'Leve', 2, 1, 'Admin', current_timestamp),
        ('IS003', 'Moderado', 2, 1, 'Admin', current_timestamp),
        ('IS004', 'Severo', 2, 1, 'Admin', current_timestamp)
go

-- Especialidades Medicas = 3
insert into MedLista
        (Codigo, Nombre, Categoria, Activo, UsuarioCrea, FechaCrea)
values  ('EM001', 'Alergolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM002', 'Anestesiolog�a y Reanimaci�n', 3, 1, 'Admin', current_timestamp),
        ('EM003', 'Angiolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM004', 'Cardiolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM005', 'Endocrinolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM006', 'Epidemiolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM007', 'Gastroenterolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM008', 'Geriatr�a', 3, 1, 'Admin', current_timestamp),
        ('EM009', 'Hematolog�a y Hemoterapia', 3, 1, 'Admin', current_timestamp),
        ('EM010', 'Hepatolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM011', 'Infectolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM012', 'Medicina Aeroespacial', 3, 1, 'Admin', current_timestamp),
        ('EM013', 'Medicina del Deporte', 3, 1, 'Admin', current_timestamp),
        ('EM014', 'Medicina de Emergencia', 3, 1, 'Admin', current_timestamp),
        ('EM015', 'Medicina Familiar y Comunitaria', 3, 1, 'Admin', current_timestamp),
        ('EM016', 'Medicina F�sica y Rehabilitaci�n o Fisiatr�a', 3, 1, 'Admin', current_timestamp),
        ('EM017', 'Medicina Forense', 3, 1, 'Admin', current_timestamp),
        ('EM018', 'Medicina Intensiva', 3, 1, 'Admin', current_timestamp),
        ('EM019', 'Medicina Interna', 3, 1, 'Admin', current_timestamp),
        ('EM020', 'Medicina Preventiva y Salud P�blica', 3, 1, 'Admin', current_timestamp),
        ('EM021', 'Medicina del Trabajo', 3, 1, 'Admin', current_timestamp),
        ('EM022', 'Nefrolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM023', 'Neumolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM024', 'Neurolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM025', 'Nutriolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM026', 'Oncolog�a M�dica', 3, 1, 'Admin', current_timestamp),
        ('EM027', 'Oncolog�a Radioter�pica', 3, 1, 'Admin', current_timestamp),
        ('EM028', 'Pediatr�a', 3, 1, 'Admin', current_timestamp),
        ('EM029', 'Psiquiatr�a', 3, 1, 'Admin', current_timestamp),
        ('EM030', 'Reumatolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM031', 'Toxicolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM032', 'Cirug�a Cardiovascular', 3, 1, 'Admin', current_timestamp),
        ('EM033', 'Cirug�a General y del Aparato Digestivo', 3, 1, 'Admin', current_timestamp),
        ('EM034', 'Cirug�a Oral y Maxilofacial', 3, 1, 'Admin', current_timestamp),
        ('EM035', 'Cirug�a Ortop�dica y Traumatolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM036', 'Cirug�a Pedi�trica', 3, 1, 'Admin', current_timestamp),
        ('EM037', 'Cirug�a Pl�stica', 3, 1, 'Admin', current_timestamp),
        ('EM038', 'Cirug�a Tor�cica', 3, 1, 'Admin', current_timestamp),
        ('EM039', 'Psiquiatr�a', 3, 1, 'Admin', current_timestamp),
        ('EM040', 'Angiolog�a y Cirug�a Vascular', 3, 1, 'Admin', current_timestamp),
        ('EM041', 'Neurocirug�a', 3, 1, 'Admin', current_timestamp),
        ('EM042', 'Dermatolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM043', 'Ginecolog�a y Obstetricia o Tocolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM044', 'Oftalmolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM045', 'Traumatolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM046', 'Urolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM047', 'An�lisis cl�nico', 3, 1, 'Admin', current_timestamp),
        ('EM048', 'Anatom�a Patol�gica', 3, 1, 'Admin', current_timestamp),
        ('EM049', 'Bioqu�mica Cl�nica', 3, 1, 'Admin', current_timestamp),
        ('EM050', 'Farmacolog�a Cl�nica', 3, 1, 'Admin', current_timestamp),
        ('EM051', 'Fisioterapia', 3, 1, 'Admin', current_timestamp),
        ('EM052', 'Gen�tica m�dica', 3, 1, 'Admin', current_timestamp),
        ('EM053', 'Inmunolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM054', 'Medicina Nuclear', 3, 1, 'Admin', current_timestamp),
        ('EM055', 'Microbiolog�a y Parasitolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM056', 'Neurofisiolog�a Cl�nica', 3, 1, 'Admin', current_timestamp),
        ('EM057', 'Radiolog�a', 3, 1, 'Admin', current_timestamp),
        ('EM058', 'Medicina Veterinaria', 3, 1, 'Admin', current_timestamp)
go

-- V�a de Administraci�n (Categoria = 6)
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('VA001', 'Oral', 6, null, 1, 'Admin', current_timestamp),
       ('VA002', 'Sublingual', 6, null, 1, 'Admin', current_timestamp),
       ('VA003', 'Gastroent�rica', 6, null, 1, 'Admin', current_timestamp),
       ('VA004', 'T�pica', 6, null, 1, 'Admin', current_timestamp),
       ('VA005', 'Transd�rmica', 6, null, 1, 'Admin', current_timestamp),
       ('VA006', 'Oftalmol�gica', 6, null, 1, 'Admin', current_timestamp),
       ('VA007', 'Inhalatoria', 6, null, 1, 'Admin', current_timestamp),
       ('VA008', 'Nasal', 6, null, 1, 'Admin', current_timestamp),
       ('VA009', 'Nebulizaciones', 6, null, 1, 'Admin', current_timestamp),
       ('VA010', 'Rectal', 6, null, 1, 'Admin', current_timestamp),
       ('VA011', 'Vaginal', 6, null, 1, 'Admin', current_timestamp),
       ('VA012', 'Intravenoso - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA013', 'Intramuscular - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA014', 'Subcut�nea - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA015', 'Intratecal (Parental, alrededor de m�dula espinal)', 6, null, 1, 'Admin', current_timestamp),
       ('VA016', 'Ocular', 6, null, 1, 'Admin', current_timestamp),
       ('VA017', '�tica', 6, null, 1, 'Admin', current_timestamp),
       ('VA018', 'IntraLesional', 6, null, 1, 'Admin', current_timestamp),
       ('VA019', 'Intradermica', 6, null, 1, 'Admin', current_timestamp),
       ('VA020', 'Intrapreural', 6, null, 1, 'Admin', current_timestamp),
       ('VA021', 'Intratraqueal', 6, null, 1, 'Admin', current_timestamp),
       ('VA022', 'Intracraneal', 6, null, 1, 'Admin', current_timestamp),
	   ('VA023', 'Intracraneal', 6, null, 1, 'Admin', current_timestamp),
	   ('VA024', 'Intraarticular', 6, null, 1, 'Admin', current_timestamp),
	   ('VA025', 'Peritoneal', 6, null, 1, 'Admin', current_timestamp)
go

-- tipo de examen = 8
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('EX001', 'F�sico', 8, null, 1, 'Admin', current_timestamp),
       ('EX002', 'Prueba de Laboratorio', 8, null, 1, 'Admin', current_timestamp),
       ('EX003', 'Prueba M�dica', 8, null, 1, 'Admin', current_timestamp)
go

-- insertar la clasificacion de las actividades de estilo de vida
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('EV01', 'Dieta', 9, null, 1, 'Admin', current_timestamp),
       ('EV02', 'Higiene Personal', 9, null, 1, 'Admin', current_timestamp),
	   ('EV03', 'Estr�s', 9, null, 1, 'Admin', current_timestamp),
	   ('EV04' ,'Tabaco', 9, null, 1, 'Admin', current_timestamp),
	   ('EV05', 'Alcohol', 9, null, 1, 'Admin', current_timestamp),
	   ('EV06', 'Alimentaci�n', 9, null, 1, 'Admin', current_timestamp),
	   ('EV07', 'Drogas no recetadas', 9, null, 1, 'Admin', current_timestamp),
	   ('EV08', 'Patrones de Sue�o', 9, null, 1, 'Admin', current_timestamp),
	   ('EV09', 'Trabajo de Alto Riesgo', 9, null, 1, 'Admin', current_timestamp),
	   ('EV10', 'Deporte', 9, null, 1, 'Admin', current_timestamp),
	   ('EV11', 'Actividades Extremas', 9, null, 1, 'Admin', current_timestamp),
	   ('EV12', 'Actividades de Ocio y Afines', 9, null, 1, 'Admin', current_timestamp),
	   ('EV13', 'Relaciones Interpersonales', 9, null, 1, 'Admin', current_timestamp),
	   ('EV14', 'Medio Ambiente', 9, null, 1, 'Admin', current_timestamp),
	   ('EV15', 'Comportamiento Sexual', 9, null, 1, 'Admin', current_timestamp),
	   ('EV16', 'Otros', 9, null, 1, 'Admin', current_timestamp)

-- insertar los tipos de problemas medicos
insert into MedLista
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PM01', 'Enfermedad', 10, null, 1, 'Admin', current_timestamp),
       ('PM02', 'Alergia', 10, null, 1, 'Admin', current_timestamp), 
	   ('PM03', 'Medicaci�n', 10, null, 1, 'Admin', current_timestamp), 
       ('PM04', 'Lesion', 10, null, 1, 'Admin', current_timestamp),
       ('PM05', 'Ciruj�a', 10, null, 1, 'Admin', current_timestamp), 
	   ('PM06', 'Dental', 10, null, 1, 'Admin', current_timestamp), 
	   ('PM07', 'Otro', 10, null, 1, 'Admin', current_timestamp)

go

-- insertar gravedad de problema medicos
insert into MedLista
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PMG01', 'Leve', 11, null, 1, 'Admin', current_timestamp),
       ('PMG02', 'Leve a Moderado', 11, null, 1, 'Admin', current_timestamp), 
	   ('PMG03', 'Moderado', 11, null, 1, 'Admin', current_timestamp), 
	   ('PMG04', 'Moderado a Severo', 11, null, 1, 'Admin', current_timestamp),
	   ('PMG05', 'Severo ', 11, null, 1, 'Admin', current_timestamp),
	   ('PMG06', 'Evento con Amenaza de Vida', 11, null, 1, 'Admin', current_timestamp),
	   ('PMG07', 'Fatal', 11, null, 1, 'Admin', current_timestamp)
go

-- insertar reaccion a problema medico (Alergia)
insert into MedLista
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PMR01', 'Urticaria', 12, null, 1, 'Admin', current_timestamp),
       ('PMR02', 'Nausea', 12, null, 1, 'Admin', current_timestamp), 
	   ('PMR03', 'Falta Aire Respirar', 12, null, 1, 'Admin', current_timestamp)
go

-- insertar resultado problema medico 
insert into MedLista
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PRE01', 'Resuelto', 13, null, 1, 'Admin', current_timestamp),
       ('PRE02', 'Mejorado', 13, null, 1, 'Admin', current_timestamp), 
	   ('PRE03', 'Estable', 13, null, 1, 'Admin', current_timestamp), 
	   ('PRE04', 'Peor', 13, null, 1, 'Admin', current_timestamp),
	   ('PRE05', 'Seguimiento Pendiente ', 13, null, 1, 'Admin', current_timestamp)
go

-- insertar resultado problema medico 
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('PMF01', 'Primera Vez', 14, null, 1, 'Admin', current_timestamp),
       ('PMF02', 'Menos de 2 Veces al Mes', 14, null, 1, 'Admin', current_timestamp), 
	   ('PMF03', 'Entre 2 y 12 Veces al Mes', 14, null, 1, 'Admin', current_timestamp), 
	   ('PMF04', 'Mas de 12 Veces al Mes', 14, null, 1, 'Admin', current_timestamp),
	   ('PMF05', 'Cronico', 14, null, 1, 'Admin', current_timestamp),
	   ('PMF06', 'Agudo', 14, null, 1, 'Admin', current_timestamp),
	   ('PMF07', 'Desconocido o N/A', 14, null, 1, 'Admin', current_timestamp)
go

select * from MedLista where Categoria = 6
--delete from MedLista where Categoria = 6

-- para actualizar los datos de via en medicamentos y poner todo de forma uniforme
update Medicamento
   set via = 'T�pica'
  where via in ('T�pica', 'T�pico')
go
update Medicamento
   set via = 'IM, IV'
  where via = 'IM/IV'
go
update Medicamento
   set via = 'SC,IM,IV'
  where via in ('SC/IM/IV', 'SC,IM, IV')
go
update Medicamento
   set via = null
  where via = ''
go
update Medicamento
   set via = 'SC,IV'
  where via = 'SC, IV'
go
update Medicamento
   set via = 'IM,IV'
  where via = 'IM, IV'
go
update Medicamento
   set via = 'IL,IM'
  where via = 'IL, IM'
go
update Medicamento
   set via = 'IM,IA,IL'
  where via = 'IM, IA, IL'
go


-- insertar las vias de administracion de los medicamentos

insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
values (186, 'VA019', 'Admin', current_timestamp)
go

insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA018', 'Admin', current_timestamp
  from Medicamento
 where via like '%IL%'
 go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA013', 'Admin', current_timestamp
  from Medicamento
 where via like '%IM%'

go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA024', 'Admin', current_timestamp
  from Medicamento
 where via like '%IA%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA012', 'Admin', current_timestamp
  from Medicamento
 where via like '%IV%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA007', 'Admin', current_timestamp
  from Medicamento
 where via like '%INH%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA008', 'Admin', current_timestamp
  from Medicamento
 where via like '%Nasal%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA001', 'Admin', current_timestamp
  from Medicamento
 where via like '%Oral%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA025', 'Admin', current_timestamp
  from Medicamento
 where via like '%Peritoneal%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA010', 'Admin', current_timestamp
  from Medicamento
 where via like '%R%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA014', 'Admin', current_timestamp
  from Medicamento
 where via like '%SC%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA004', 'Admin', current_timestamp
  from Medicamento
 where via like '%T�pica%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA011', 'Admin', current_timestamp
  from Medicamento
 where via like '%Vaginal%'
go
insert into MedicamentoVia 
       (Medicamento, Via, UsuarioCrea, FechaCrea)
select Oid, 'VA001', 'Admin', current_timestamp
  from Medicamento
 where via like '%VO%'
go
