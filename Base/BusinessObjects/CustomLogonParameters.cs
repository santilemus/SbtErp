using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Parametros personalizados para el Log On
    /// En este caso se incluyen la seleccion de Empresas y Agencias
    /// </summary>
    /// <remarks> 
    /// mas info en: https://docs.devexpress.com/eXpressAppFramework/112982/task-based-help/security/how-to-use-custom-logon-parameters-and-authentication
    /// </remarks>
    [DomainComponent, Serializable]
    [System.ComponentModel.DisplayName("Conectarse")]   // "Log On"
    public class CustomLogonParameters : INotifyPropertyChanged, ISerializable, ICustomObjectSerialize, ISupportClearPassword, IAuthenticationStandardLogonParameters
    {
        //int oidSucursal;
        //private int oidEmpresa;
        private Empresa empresa;
        private EmpresaUnidad agencia;
        private string password;

        [DataSourceCriteria("Activa == True")]
        [JsonIgnore]
        public Empresa Empresa
        {
            get { return empresa; }
            set
            {
                if (empresa == value) return;
                empresa = value;
                Agencia = null;
                OnPropertyChanged(nameof(Empresa));
                if (Empresa != null) 
                    Empresa.Agencias.Reload();
            }
        }

        [DataSourceProperty("Empresa.Agencias")]
        [JsonIgnore]
        public EmpresaUnidad Agencia
        {
            get { return agencia; }
            set
            {
                if (agencia == value) return;
                agencia = value;
                Empresa = agencia?.Empresa;
                OnPropertyChanged(nameof(Agencia));
            }
        }


        public CustomLogonParameters() { }
        // ISerializable 
        public CustomLogonParameters(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount > 0)
            {
                UserName = info.GetString("UserName");
                password = info.GetString("Password");
            }
        }

        [System.Security.SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserName", UserName);
            info.AddValue("Password", password);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //[Browsable(false)]
        [System.ComponentModel.DisplayName("Usuario")]
        [RuleRequiredField("CustomLogonParameters.UserName_requerido", DefaultContexts.Save, "El nombre de usuario es obligatorio")]
        public String UserName { get; set; }
       
        [PasswordPropertyText(true)]
        [System.ComponentModel.DisplayName("Contraseña")]
        public string Password
        {
            get { return password; }
            set
            {
                if (string.Compare(password, value, StringComparison.Ordinal) == 0) return;
                password = value;
            }
        }

        public void RefrescarAgencias(IObjectSpace objectSpace)
        {
            Agencia = (Empresa == null) ? null : objectSpace.FirstOrDefault<EmpresaUnidad>(e => e.Empresa == Empresa && e.Activa == true); // && e.Role == ETipoRoleUnidad.Agencia);
        }

        public void ReadPropertyValues(SettingsStorage storage)
        {
            UserName = storage.LoadOption("", "UserName"); 
        }

        public void WritePropertyValues(SettingsStorage storage)
        {
            storage.SaveOption("", "UserName", UserName); 
        }

        public void ClearPassword()
        {
            Password = string.Empty;
        }
    }
}
