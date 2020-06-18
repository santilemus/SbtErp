using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    public enum CategoriaListaMedico
    {
        [XafDisplayName("Terminología Anatómica")]
        PartesCuerpo = 1,
        [XafDisplayName("Intensidades")]
        Intensidad = 2,
        [XafDisplayName("Especialidades")]
        Especialidad = 3,
        [XafDisplayName("UnidadMedica")]
        UnidadMedica = 4,
        [XafDisplayName("Tipo de Vacuna")]
        TipoVacuna = 5,
        [XafDisplayName("Vías de Administración")]
        ViaAdministracion = 6,
        [XafDisplayName("Tipo de Admisión")]
        TipoAdmision = 7,
        [XafDisplayName("Categoría Examen")]
        CategoriaExamen = 8,
        [XafDisplayName("Estilo de Vida")]
        EstiloVida = 9,
        [XafDisplayName("Tipo Problema Medico")]
        TipoProblemaMedico = 10,
        [XafDisplayName("Gravedad PM")]
        GravedadPM = 11,
        [XafDisplayName("Reacción PM")]
        ReaccionPM = 12,
        [XafDisplayName("Resultado PM")]
        ResultadoPM = 13,
        [XafDisplayName("Frecuencia PM")]
        FrecuenciaPM = 14
    }
}

