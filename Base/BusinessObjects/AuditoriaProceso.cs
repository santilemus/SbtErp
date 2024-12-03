using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [NavigationItem(false), Persistent("SysAuditoriaProceso"), ModelDefault("Caption", "Auditoría Proceso"),
        DefaultProperty(nameof(NombreProceso))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AuditoriaProceso : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AuditoriaProceso(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            int id = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var fEmpre= Session.GetObjectByKey<Empresa>(id);
            if (fEmpre != null)
                empresa = fEmpre;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [DbType("int"), Persistent(nameof(Oid)), Key(true)]
        int oid = -1;
        [Persistent(nameof(Empresa))]
        Empresa empresa;
        [Persistent(nameof(NombreProceso)), Size(60), DbType("varchar(60)")]
        string nombreProceso = string.Empty;
        [Persistent(nameof(Bitacora)), Size(2000), DbType("varchar(2000)")]
        string bitacora = string.Empty;
        [DbType("datetime"), Persistent(nameof(FechaCrea))]
        DateTime fechaCrea = DateTime.Now;
        [Size(25), DbType("varchar(25)"), Persistent(nameof(UsuarioCrea))]
        string usuarioCrea = ((Usuario)DevExpress.ExpressApp.SecuritySystem.CurrentUser).UserName;

        [PersistentAlias(nameof(oid)), Browsable(false)]
        public int Oid
        {
            get => oid;
        }

        [PersistentAlias(nameof(empresa)), XafDisplayName("Empresa")]
        public Empresa Empresa
        {
            get => empresa;
        }
        [PersistentAlias(nameof(nombreProceso)), XafDisplayName("Nombre Proceso")]
        public string NombreProceso
        {
            get => nombreProceso;
        }
        [PersistentAlias(nameof(bitacora)), XafDisplayName("Bitácora"), ModelDefault("RowCount", "6")]
        public string Bitacora
        {
            get => bitacora;
        }

        [PersistentAlias(nameof(fechaCrea)), XafDisplayName("Fecha Creación")]
        public DateTime FechaCrea
        {
            get => fechaCrea;
        }

        [PersistentAlias(nameof(usuarioCrea)), XafDisplayName("Usuario Creó")]
        public string UsuarioCrea
        {
            get => usuarioCrea;
        }


        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para generar el registro de auditoria de la ejecucion de un proceso
        /// </summary>
        /// <param name="sProceso">Nombre del proceso a generar auditoria de la ejecucion</param>
        /// <param name="sParametros">Cadena con una linea por cada parametro y su valor</param>
        /// <param name="sLog">Log del proceso ejecutado</param>
        public void AuditarProceso(string sProceso, string sParametros, string sLog)
        {
            if (!Session.IsNewObject(this))
                return;
            nombreProceso = sProceso;
            usuarioCrea = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
            fechaCrea = DateTime.Now;
            if (!string.IsNullOrEmpty(sParametros))
                bitacora += $" ***** Parametros **** {Environment.NewLine}{sParametros}{Environment.NewLine}";
            bitacora += $" **** LOG ***{Environment.NewLine}{sLog}";
            Save();
            if (Session.InTransaction)
                Session.CommitTransaction();
        }
        #endregion
    }
}