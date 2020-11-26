using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [NavigationItem(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("user_group")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Parientes")]
    public class Pariente : XPObjectBaseBO
    {
        string comentario;
        Enfermedad diagnostico;
        decimal estatura;
        DateTime fechaNacimiento;
        string apellidos;
        string nombres;
        private System.Boolean _responsable;
        private Paciente _paciente;
        private Listas _parentesco;
        private Paciente _idPariente;
        public Pariente(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Propiedad que corresponde al ID del BO paciente
        /// </summary>
        [DevExpress.Xpo.AssociationAttribute("Parientes-Paciente"), Persistent("Paciente"), XafDisplayName("Paciente")]
        public Paciente Paciente
        {
            get => _paciente;
            set => SetPropertyValue(nameof(Paciente), ref _paciente, value);
        }

        /// <summary>
        ///  Esta propiedad tiene valor y es valida cuando el pariente es ademas paciente 
        ///  Cuando el pariente no es paciente  sera nula
        /// </summary>
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, VisibleInLookupListView(true),
            Persistent("IdPariente"), XafDisplayName("Id Pariente")]
        public Paciente IdPariente
        {
            get => _idPariente;
            set => SetPropertyValue(nameof(IdPariente), ref _idPariente, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public Listas Parentesco
        {
            get
            {
                return _parentesco;
            }
            set
            {
                SetPropertyValue("Parentesco", ref _parentesco, value);
            }
        }


        [Size(50), DbType("varchar(50)"), Persistent("Nombres"), XafDisplayName("Nombres")]
        public string Nombres
        {
            get => nombres;
            set => SetPropertyValue(nameof(Nombres), ref nombres, value);
        }


        [Size(50), DbType("varchar(50)"), Persistent("Apellidos"), XafDisplayName("Apellidos")]
        public string Apellidos
        {
            get => apellidos;
            set => SetPropertyValue(nameof(Apellidos), ref apellidos, value);
        }

        [DbType("datetime"), Persistent("FechaNacimiento"), XafDisplayName("Fecha Nacimiento")]
        public DateTime FechaNacimiento
        {
            get => fechaNacimiento;
            set => SetPropertyValue(nameof(FechaNacimiento), ref fechaNacimiento, value);
        }

        [DbType("numeric(5,2)"), Persistent("Estatura"), XafDisplayName("Estatura (cm)"),
           RuleRange("Pariente.Estatura_RangoValido", DefaultContexts.Save, 10, 250, ResultType = ValidationResultType.Warning,
            SkipNullOrEmptyValues = true)]
        public decimal Estatura
        {
            get => estatura;
            set => SetPropertyValue(nameof(Estatura), ref estatura, value);
        }


        [Persistent("Diagnostico"), XafDisplayName("Diagnostico"), ToolTip("Referencia del diagnostico del pariente, para la historia familiar")]
        public Enfermedad Diagnostico
        {
            get => diagnostico;
            set => SetPropertyValue(nameof(Diagnostico), ref diagnostico, value);
        }

        /// <summary>
        /// Complementar la informacion del diagnostico del pariente. En algunos puede que no exista un diagnostico
        /// puntual para el pariente, asi que se tiene que detallar la informacion proporcionada en este campo
        /// </summary>
        [Size(200), DbType("varchar(200)"), Persistent("Comentario"), XafDisplayName("Comentario"),
            ToolTip("Cuando el diagnostico del pariente no es claro o es necesario detallar, hacerlo aquí")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        /// <summary>
        /// Cuando el paciente es menor de edad, valor true indica que el pariente es el responsable
        ///
        /// </summary>
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public System.Boolean Responsable
        {
            get
            {
                return _responsable;
            }
            set
            {
                SetPropertyValue("Responsable", ref _responsable, value);
            }
        }

    }
}
