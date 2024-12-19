using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipo de Unidad de Medida
    /// <br>Ninguna = 0</br>
    /// <br>Longitud = 1</br>
    /// <br>Masa = 2</br>
    /// <br>Tiempo = 3</br>
    /// <br>Volumen = 4</br>
    /// <br>Temperaatura = 5</br>
    /// <br>Area = 6</br>
    /// <br>Potencia = 7</br>
    /// </summary>
    public enum TipoUnidadMedida
    {
        [XafDisplayName("Otro")]
        Ninguna = 0,
        [XafDisplayName("Longitud")]
        Longitud = 1,
        [XafDisplayName("Masa")]
        Masa = 2,
        [XafDisplayName("Tiempo")]
        Tiempo = 3,
        [XafDisplayName("Volumen")]
        Volumen = 4,
        /// <summary>
        /// convertir kelvin a celcius  0K − 273.15 = -273.1°C -> Ejemplo: 4K − 273.15 = -269.1°C
        /// convertir celcius a kelvin 0°C + 273.15 = 273.15K
        /// convertir celcius a fahrenhit (0°C × 9/5) + 32 = 32°F -> Ejemplo: (4°C × 9/5) + 32 = 39.2°F
        /// convertir fahreinhit a celcius (32°F − 32) × 5/9 = 0°C -> Ejemplo: (45°F − 32) × 5/9 = 7.222°C
        /// Implementar metodos en el BO UnidadMedida para hacer las conversiones de cualquier unidad de medida
        /// </summary>
        [XafDisplayName("Temperatura")]
        Temperatura = 5,
        [XafDisplayName("Área")]
        Area = 6,
        Potencia = 7
        //'L', 'M', 'T', 'I', 'A', 'V', 'F', 'P', 'K'
    }
}
