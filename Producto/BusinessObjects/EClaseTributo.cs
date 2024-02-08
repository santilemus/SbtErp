﻿using DevExpress.ExpressApp.DC;


namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con las clases de tributos
    /// </summary>
    public enum EClaseTributo
    {
        Impuesto = 0,
        Tasa = 1,
        [XafDisplayName("Contribución")]
        Contribucion = 2
    }
}