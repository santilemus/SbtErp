using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DomainComponent, Serializable]
    [System.ComponentModel.DisplayName("Conexión")]   // "Log On"
    public class CustomLogonParameters : INotifyPropertyChanged, ISerializable
    {
        int oidSucursal;
        private Empresa empresa;
        [DataSourceProperty("EmpresasDisponible"), ImmediatePostData(true)]
        public Empresa Empresa
        {
            get { return empresa; }
            set
            {
                if (empresa == value) return;
                empresa = value;
                //RefreshAvailableUsers();
                RefrehsAvailableSuc();
            }
        }

        private int oidEmpresa;
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
                if (EmpresasDisponible != null)
                {
                    var obj = EmpresasDisponible.Lookup(OidEmpresa);
                    if (obj != null)
                        Empresa = obj;
                }
            }
        }

        private EmpresaUnidad agencia;
        //[DataSourceCriteria("Empresa = '@This.Empresa'"), ImmediatePostData(true)]
        [DataSourceProperty("SucursalesDisponibles"), ImmediatePostData(true)]
        public EmpresaUnidad Agencia
        {
            get { return agencia; }
            set
            {
                if (agencia == value) return;
                agencia = value;
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
                if (SucursalesDisponibles != null)
                {
                    var obj = SucursalesDisponibles.Lookup(value);
                    if (obj != null)
                        Agencia = obj;
                }
            }
        }

        //private Usuario usuario;
        //[DataSourceProperty("UsuariosDisponible"), ImmediatePostData(true)]
        //public Usuario Usuario
        //{
        //    get { return usuario; }
        //    set
        //    {
        //        if (usuario == value || value == null) return;
        //        usuario = value;
        //        Empresa = usuario.Empresa; ;
        //        UserName = usuario.UserName;
        //    }
        //}

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
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;



        //private void RefreshAvailableUsers()
        //{
        //    if (usuariosDisponible == null) return;
        //    if (Empresa == null)
        //    {
        //        usuariosDisponible.Criteria = null;
        //    }
        //    else
        //    {
        //        if (Agencia != null)
        //            usuariosDisponible.Criteria = CriteriaOperator.Parse("Empresa = ? And Agencia = ?", Empresa, Agencia);
        //        else
        //            usuariosDisponible.Criteria = new BinaryOperator("Empresa", Empresa);
        //    }
        //    if (usuario != null)
        //    {
        //        if ((usuariosDisponible.IndexOf(usuario) == -1) || (usuario.Empresa != Empresa))
        //        {
        //            Usuario = null;
        //        }
        //    }
        //}


        // AGREGAR AQUI, metodo similar al anterior pero para refrescar las usuarios disponibles por agencia y analizar si aplica, sino quitar la relacion
        // entre usuarios y agencia

        private void RefrehsAvailableSuc()
        {
            if (sucursalesDisponibles == null) return;
            if (Agencia == null)
                sucursalesDisponibles.Criteria = null;
            else
                sucursalesDisponibles.Criteria = CriteriaOperator.Parse("Empresa = ? And Role = ? And Activa = ?", Empresa, 2, true);
        }

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
        private XPCollection<Empresa> empresasDisponible;
        private XPCollection<EmpresaUnidad> sucursalesDisponibles;
        //private XPCollection<Usuario> usuariosDisponible;

        [Browsable(false)]
        public IObjectSpace ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }

        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false)]
        public XPCollection<Empresa> EmpresasDisponible
        {
            get
            {
                if (empresasDisponible == null)
                {
                    empresasDisponible = ObjectSpace.GetObjects<SBT.Apps.Base.Module.BusinessObjects.Empresa>() as XPCollection<SBT.Apps.Base.Module.BusinessObjects.Empresa>;
                }
                return empresasDisponible;
            }
        }

        /// <summary>
        /// Retornar coleccion de sucursales, por eso se filtra por tipo de role (2 es agencia) y que esten activas
        /// </summary>
        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public XPCollection<EmpresaUnidad> SucursalesDisponibles
        {
            get
            {
                if (sucursalesDisponibles == null)
                {
                    CriteriaOperator criteria = CriteriaOperator.Parse("Empresa = ? And Role = ?", Empresa, 2); // And Activa = ?", Empresa, 2, true);
                    sucursalesDisponibles = ObjectSpace.GetObjects<SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad>(criteria) as XPCollection<EmpresaUnidad>;
                }
                return sucursalesDisponibles;
            }
        }

        //[Browsable(false)]
        //[CollectionOperationSet(AllowAdd = false)]
        //public XPCollection<Usuario> UsuariosDisponible
        //{
        //    get
        //    {
        //        if (usuariosDisponible == null)
        //        {
        //            usuariosDisponible = ObjectSpace.GetObjects<Usuario>(CriteriaOperator.Parse("Empresa = ?", Empresa)) as XPCollection<Usuario>;
        //            RefreshAvailableUsers();
        //        }
        //        return usuariosDisponible;
        //    }
        //}
    }
}
