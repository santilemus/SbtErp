using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

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
    [System.ComponentModel.DisplayName("Conexión")]   // "Log On"
    public class CustomLogonParameters : INotifyPropertyChanged, ISerializable
    {
        int oidSucursal;
        private int oidEmpresa;
        private Empresa empresa;
        private EmpresaUnidad agencia;

        //[DataSourceProperty("EmpresasDisponible"), ImmediatePostData(true)]
        [DataSourceCriteria("Activa == True"), ImmediatePostData(true)]
        public Empresa Empresa
        {
            get { return empresa; }
            set
            {
                if (empresa == value) return;
                empresa = value;
                Agencia = null;
                OnPropertyChanged(nameof(Empresa));
                RefrescarAgencias();
                //RefreshAvailableUsers();
            }
        }

        /// <summary>
        /// Propiedad para recuperar la empresa a partir del codigo. La propiedad es utilizada desde el evento
        /// LastLogonParametersReading del Global.asax, con el valor del codigo de la empresa guardado en la
        /// sesion anterior
        /// </summary>
        [VisibleInDetailView(false), VisibleInListView(false)]
        public int OidEmpresa
        {
            get { return (empresa != null) ? empresa.Oid : 1; }
            set
            {
                oidEmpresa = value;
                Empresa emp = ObjectSpace.GetObjectByKey<Empresa>(oidEmpresa);
                if (emp != null)
                    Empresa = emp;
            }
        }

        [DataSourceProperty("Empresa.Unidades"), ImmediatePostData(true)]
        public EmpresaUnidad Agencia
        {
            get { return agencia; }
            set
            {
                if (agencia == value) return;
                agencia = value;
                OnPropertyChanged(nameof(Agencia));
                //RefreshAvailableUsers();
            }
        }

        /// <summary>
        /// Propiedad para recuperar la agencia a partir del codigo. La propiedad es utilizada desde el evento
        /// LastLogonParametersReading del Global.asax, con el valor del codigo de la agencia guardada en la
        /// sesion anterior
        /// </summary>
        [VisibleInDetailView(false), VisibleInListView(false), Browsable(false)]
        public int OidSucursal
        {
            get { return (agencia != null) ? agencia.Oid : 1; }
            set
            {
                oidSucursal = value;
                if (AgenciasDisponibles != null)
                {
                    var obj = AgenciasDisponibles.Lookup(value);
                    if (obj != null)
                        Agencia = obj;
                }
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
        public String UserName { get; set; }
        private string password;
        [PasswordPropertyText(true)]
        public string Password
        {
            get { return password; }
            set
            {
                if (string.Compare(password, value, StringComparison.Ordinal) == 0) return;
                password = value;
            }
        }

        private IObjectSpace objectSpace;
        private XPCollection<EmpresaUnidad> agenciasDisponibles;
        //private XPCollection<Usuario> usuariosDisponible;

        [Browsable(false)]
        public IObjectSpace ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }

        /// <summary>
        /// Retornar coleccion de sucursales, por eso se filtra por tipo de role (2 es agencia) y que esten activas
        /// </summary>
        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public XPCollection<EmpresaUnidad> AgenciasDisponibles
        {
            get
            {
                if (agenciasDisponibles == null)
                    RefrescarAgencias();
                return agenciasDisponibles;
            }
        }

        private void RefrescarAgencias()
        {
            agenciasDisponibles = Empresa.Unidades;
            // Se produce error al logearse cuando agenciasDisponibles es nulo, por eso se comentario el filtro
            // porque pueden haber empresas en las cuales ninguna de sus unidades es agencia

            // agenciasDisponibles.Filter = CriteriaOperator.Parse("[Role] == 2 && [Activa] == True");
        }

    }
}
