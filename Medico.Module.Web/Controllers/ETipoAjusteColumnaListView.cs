namespace SBT.Apps.Medico.Module.Web.Controllers
{
    public enum ETipoAjusteColumnaListView
    {
        /// <summary>
        /// Ancho de columnas por defecto. En aplicaciones asp.net es calculado dinamicamente con base al contenido de la columna
        /// </summary>
        Default,
        /// <summary>
        /// Ancho de la columna de acuerdo al ancho definido para la columna en el correspondiente ListView del Model
        /// </summary>
        Model,
        /// <summary>
        /// Solamente se muestran las columnas que caben de acuerdo al ancho de la pantalla. Se ajusta dinamicamente para ser responsive
        /// </summary>
        BestFit
    }
}
