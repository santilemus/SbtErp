using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Clasificación de las categorías de Productos
    /// </summary>
    public enum EClasificacion
    {
        /// <summary>
        /// Producto final para la venta. Pueden ser insumos (productos intermedios) para otros procesos más complejos y de mayor valor agregado  
        /// </summary>
        [XafDisplayName("Producto Terminado")]
        ProductoTerminado = 0,
        /// <summary>
        /// No ha sufrido ninguna transformación, se encuentra en su estado natural 
        /// </summary>
        [XafDisplayName("Materia Prima")]
        MateriaPrima = 1,
        /// <summary>
        /// Materiales utilizados en el proceso de fabricación y que es atribuible de forma directa el producto final
        /// </summary>
        [XafDisplayName("Material Directo")]
        MaterialDirecto = 2,
        /// <summary>
        /// Materiales utilizados en el proceso de fabricación y que no es posible relacionarlo con los productos elaborados
        /// </summary>
        [XafDisplayName("Material Indirecto")]
        MaterialIndirecto = 3,
        /// <summary>
        /// El cliente recibe un servicio no un bien físico. Ejemplo: Alquiler de local, Servicios de Consultoría, servicios Médicos, etc.
        /// </summary>
        [XafDisplayName("Servicios")]
        Servicios = 4,
        /// <summary>
        /// Productos intangibles. Ejemplo software
        /// </summary>
        [XafDisplayName("Intangible")]
        Intangible = 5,
        /// <summary>
        /// Otros productos no clasificados
        /// </summary>
        [XafDisplayName("Otros")]
        Otros = 6
    }
}
