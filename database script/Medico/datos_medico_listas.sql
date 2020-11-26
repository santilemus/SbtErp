-- insertar las categorias o grupos de terminologia anatomica
insert into MedLista
        (Codigo, Nombre, Categoria, Activo, UsuarioCrea, FechaCrea)
values  ('A010', 'Cuerpo Humano', 1, 1, 'Admin', current_timestamp),
        ('A011', 'Partes del Cuerpo Humano', 1, 1, 'Admin', current_timestamp),
        ('A012', 'Planos Líneas y Regiones Anatómicas', 1, 1, 'Admin', current_timestamp),
        ('A020', 'Huesos; Sistema Esquelético', 1, 1, 'Admin', current_timestamp), 
		('A030', 'Articulaciones; Sistema Articular', 1, 1, 'Admin', current_timestamp), 
		('A040', 'Músculos; Sistema Muscular', 1, 1, 'Admin', current_timestamp),
		('A050', 'Sistema Digestivo', 1, 1, 'Admin', current_timestamp),
        ('A060', 'Sistema Respiratorio', 1, 1, 'Admin', current_timestamp),
		('A070', 'Cavidad Torácica', 1, 1, 'Admin', current_timestamp),
		('A080', 'Sistema Urinario', 1, 1, 'Admin', current_timestamp),
		('A090', 'Sistema Genitales', 1, 1, 'Admin', current_timestamp),
		('A091', 'Sistemas Genital Femenino', 1, 1, 'Admin', current_timestamp),
		('A092', 'Sistemas Genital Masculino', 1, 1, 'Admin', current_timestamp),
		('A100', 'Cavidad Abdominal y de la Pelvis', 1, 1, 'Admin', current_timestamp),
		('A110', 'Glándulas Endocrinas', 1, 1, 'Admin', current_timestamp),
		('A120', 'Sistema Cardiovascular', 1, 1, 'Admin', current_timestamp),
		('A121', 'Corazón', 1, 1, 'Admin', current_timestamp),
		('A122', 'Arterias', 1, 1, 'Admin', current_timestamp),
		('A123', 'Ventas', 1, 1, 'Admin', current_timestamp),
		('A130', 'Sistema Linfático', 1, 1, 'Admin', current_timestamp),
		('A140', 'Sistema Nervioso', 1, 1, 'Admin', current_timestamp),
		('A141', 'Sistema Nervioso Central', 1, 1, 'Admin', current_timestamp),
		('A142', 'Sistema nervioso Periférico', 1, 1, 'Admin', current_timestamp),
		('A143', 'División Autónoma. Porción Autonómica del Sistema Nervioso Periférico', 1, 1, 'Admin', current_timestamp),
		('A150', 'Órganos de los Sentidos', 1, 1, 'Admin', current_timestamp),
		('A160', 'Integumento Común', 1, 1, 'Admin', current_timestamp)
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
values  ('EM001', 'Alergología', 3, 1, 'Admin', current_timestamp),
        ('EM002', 'Anestesiología y Reanimación', 3, 1, 'Admin', current_timestamp),
        ('EM003', 'Angiología', 3, 1, 'Admin', current_timestamp),
        ('EM004', 'Cardiología', 3, 1, 'Admin', current_timestamp),
        ('EM005', 'Endocrinología', 3, 1, 'Admin', current_timestamp),
        ('EM006', 'Epidemiología', 3, 1, 'Admin', current_timestamp),
        ('EM007', 'Gastroenterología', 3, 1, 'Admin', current_timestamp),
        ('EM008', 'Geriatría', 3, 1, 'Admin', current_timestamp),
        ('EM009', 'Hematología y Hemoterapia', 3, 1, 'Admin', current_timestamp),
        ('EM010', 'Hepatología', 3, 1, 'Admin', current_timestamp),
        ('EM011', 'Infectología', 3, 1, 'Admin', current_timestamp),
        ('EM012', 'Medicina Aeroespacial', 3, 1, 'Admin', current_timestamp),
        ('EM013', 'Medicina del Deporte', 3, 1, 'Admin', current_timestamp),
        ('EM014', 'Medicina de Emergencia', 3, 1, 'Admin', current_timestamp),
        ('EM015', 'Medicina Familiar y Comunitaria', 3, 1, 'Admin', current_timestamp),
        ('EM016', 'Medicina Física y Rehabilitación o Fisiatría', 3, 1, 'Admin', current_timestamp),
        ('EM017', 'Medicina Forense', 3, 1, 'Admin', current_timestamp),
        ('EM018', 'Medicina Intensiva', 3, 1, 'Admin', current_timestamp),
        ('EM019', 'Medicina Interna', 3, 1, 'Admin', current_timestamp),
        ('EM020', 'Medicina Preventiva y Salud Pública', 3, 1, 'Admin', current_timestamp),
        ('EM021', 'Medicina del Trabajo', 3, 1, 'Admin', current_timestamp),
        ('EM022', 'Nefrología', 3, 1, 'Admin', current_timestamp),
        ('EM023', 'Neumología', 3, 1, 'Admin', current_timestamp),
        ('EM024', 'Neurología', 3, 1, 'Admin', current_timestamp),
        ('EM025', 'Nutriología', 3, 1, 'Admin', current_timestamp),
        ('EM026', 'Oncología Médica', 3, 1, 'Admin', current_timestamp),
        ('EM027', 'Oncología Radioterápica', 3, 1, 'Admin', current_timestamp),
        ('EM028', 'Pediatría', 3, 1, 'Admin', current_timestamp),
        ('EM029', 'Psiquiatría', 3, 1, 'Admin', current_timestamp),
        ('EM030', 'Reumatología', 3, 1, 'Admin', current_timestamp),
        ('EM031', 'Toxicología', 3, 1, 'Admin', current_timestamp),
        ('EM032', 'Cirugía Cardiovascular', 3, 1, 'Admin', current_timestamp),
        ('EM033', 'Cirugía General y del Aparato Digestivo', 3, 1, 'Admin', current_timestamp),
        ('EM034', 'Cirugía Oral y Maxilofacial', 3, 1, 'Admin', current_timestamp),
        ('EM035', 'Cirugía Ortopédica y Traumatología', 3, 1, 'Admin', current_timestamp),
        ('EM036', 'Cirugía Pediátrica', 3, 1, 'Admin', current_timestamp),
        ('EM037', 'Cirugía Plástica', 3, 1, 'Admin', current_timestamp),
        ('EM038', 'Cirugía Torácica', 3, 1, 'Admin', current_timestamp),
        ('EM039', 'Psiquiatría', 3, 1, 'Admin', current_timestamp),
        ('EM040', 'Angiología y Cirugía Vascular', 3, 1, 'Admin', current_timestamp),
        ('EM041', 'Neurocirugía', 3, 1, 'Admin', current_timestamp),
        ('EM042', 'Dermatología', 3, 1, 'Admin', current_timestamp),
        ('EM043', 'Ginecología y Obstetricia o Tocología', 3, 1, 'Admin', current_timestamp),
        ('EM044', 'Oftalmología', 3, 1, 'Admin', current_timestamp),
        ('EM045', 'Traumatología', 3, 1, 'Admin', current_timestamp),
        ('EM046', 'Urología', 3, 1, 'Admin', current_timestamp),
        ('EM047', 'Análisis clínico', 3, 1, 'Admin', current_timestamp),
        ('EM048', 'Anatomía Patológica', 3, 1, 'Admin', current_timestamp),
        ('EM049', 'Bioquímica Clínica', 3, 1, 'Admin', current_timestamp),
        ('EM050', 'Farmacología Clínica', 3, 1, 'Admin', current_timestamp),
        ('EM051', 'Fisioterapia', 3, 1, 'Admin', current_timestamp),
        ('EM052', 'Genética médica', 3, 1, 'Admin', current_timestamp),
        ('EM053', 'Inmunología', 3, 1, 'Admin', current_timestamp),
        ('EM054', 'Medicina Nuclear', 3, 1, 'Admin', current_timestamp),
        ('EM055', 'Microbiología y Parasitología', 3, 1, 'Admin', current_timestamp),
        ('EM056', 'Neurofisiología Clínica', 3, 1, 'Admin', current_timestamp),
        ('EM057', 'Radiología', 3, 1, 'Admin', current_timestamp),
        ('EM058', 'Medicina Veterinaria', 3, 1, 'Admin', current_timestamp)
go

-- Vía de Administración (Categoria = 6)
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('VA001', 'Oral', 6, null, 1, 'Admin', current_timestamp),
       ('VA002', 'Sublingual', 6, null, 1, 'Admin', current_timestamp),
       ('VA003', 'Gastroentérica', 6, null, 1, 'Admin', current_timestamp),
       ('VA004', 'Tópica', 6, null, 1, 'Admin', current_timestamp),
       ('VA005', 'Transdérmica', 6, null, 1, 'Admin', current_timestamp),
       ('VA006', 'Oftalmológica', 6, null, 1, 'Admin', current_timestamp),
       ('VA007', 'Inhalatoria', 6, null, 1, 'Admin', current_timestamp),
       ('VA008', 'Nasal', 6, null, 1, 'Admin', current_timestamp),
       ('VA009', 'Nebulizaciones', 6, null, 1, 'Admin', current_timestamp),
       ('VA010', 'Rectal', 6, null, 1, 'Admin', current_timestamp),
       ('VA011', 'Vaginal', 6, null, 1, 'Admin', current_timestamp),
       ('VA012', 'Intravenoso - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA013', 'Intramuscular - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA014', 'Subcutánea - Parental', 6, null, 1, 'Admin', current_timestamp),
       ('VA015', 'Intratecal (Parental, alrededor de médula espinal)', 6, null, 1, 'Admin', current_timestamp),
       ('VA016', 'Ocular', 6, null, 1, 'Admin', current_timestamp),
       ('VA017', 'Ótica', 6, null, 1, 'Admin', current_timestamp),
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
values ('EX001', 'Físico', 8, null, 1, 'Admin', current_timestamp),
       ('EX002', 'Prueba de Laboratorio', 8, null, 1, 'Admin', current_timestamp),
       ('EX003', 'Prueba Médica', 8, null, 1, 'Admin', current_timestamp)
go

-- insertar la clasificacion de las actividades de estilo de vida
insert into MedLista 
       (Codigo, Nombre, Categoria, Comentario, Activo, UsuarioCrea, FechaCrea)
values ('EV01', 'Dieta', 9, null, 1, 'Admin', current_timestamp),
       ('EV02', 'Higiene Personal', 9, null, 1, 'Admin', current_timestamp),
	   ('EV03', 'Estrés', 9, null, 1, 'Admin', current_timestamp),
	   ('EV04' ,'Tabaco', 9, null, 1, 'Admin', current_timestamp),
	   ('EV05', 'Alcohol', 9, null, 1, 'Admin', current_timestamp),
	   ('EV06', 'Alimentación', 9, null, 1, 'Admin', current_timestamp),
	   ('EV07', 'Drogas no recetadas', 9, null, 1, 'Admin', current_timestamp),
	   ('EV08', 'Patrones de Sueño', 9, null, 1, 'Admin', current_timestamp),
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
	   ('PM03', 'Medicación', 10, null, 1, 'Admin', current_timestamp), 
       ('PM04', 'Lesion', 10, null, 1, 'Admin', current_timestamp),
       ('PM05', 'Cirujía', 10, null, 1, 'Admin', current_timestamp), 
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
   set via = 'Tópica'
  where via in ('Tópica', 'Tópico')
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
 where via like '%Tópica%'
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
