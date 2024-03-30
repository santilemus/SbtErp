namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Enumeración con los estados de la consulta médica, odontólogica o de Nutrición
    /// Es permitido:
    /// Espera = 0,
    /// Iniciada = 1,
    /// Finalizada = 2,
    /// Cancelada = 3,
    /// </summary>
    public enum EEstadoConsulta
    {
        Espera = 0,
        Iniciada = 1,
        Finalizada = 2,
        Cancelada = 3
    }
}
