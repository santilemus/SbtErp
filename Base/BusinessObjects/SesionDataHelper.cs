using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Guardar Informacion de los parámetros de sesion: Empresa, Agencia, Periodo, etc. por usuario
    /// La información se obtiene del login del usuario y debe guardar despues de logeado, para eso se utiliza el evento
    /// Instance_LoggedOn del global.asax
    /// </summary>
    public class SesionDataHelper
    {
        private static IValueManager<Dictionary<string, object>> fStore;


        /// <summary>
        /// Inicializar el almacenamiento fStore, para los parámetros de sesión, ingresados en el diálogo de login
        /// </summary>
        /// <param name="IDSesion"></param>
        public static void Inicializar(string IDSesion)
        {
            if (fStore == null)
            {
                fStore = ValueManager.GetValueManager<Dictionary<string, object>>(IDSesion);
                fStore.Value = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Retornar el valor del parámetro de sesion, cuyo nombre (key) se recibe en el parámetro
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static object ObtenerValor(string AKey)
        {
            object valor;
            if ((fStore != null) && (fStore.Value.TryGetValue(AKey, out valor)))
                return valor;
            else
                return null;
        }

        /// <summary>
        /// Agregar un parámetro de inicio de sesion al almacenamiento
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        public static void Agregar(string AKey, object AValue)
        {
            if (fStore != null)
            {
                if (fStore.Value == null)
                    fStore.Value = new Dictionary<string, object>();
                if (!fStore.Value.ContainsKey(AKey))
                    fStore.Value.Add(AKey, AValue);
                else
                    fStore.Value[AKey] = AValue;
            }
        }
    }

}