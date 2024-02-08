using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors;
using System;
using System.Web.UI.WebControls;

namespace SBT.Apps.Medico.Module.Web.Editors
{
    [PropertyEditor(typeof(Int32), false)]
    public class ToothEditor : WebPropertyEditor
    {
        public ToothEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override WebControl CreateViewModeControlCore()
        {
            Label control = new Label();
            control.ID = "editor";
            return control;
        }
        protected override WebControl CreateEditModeControlCore()
        {
            // aqui estaria la creacion de la imagen
            // hacemos una sola o un conjunto de imagenes podemos hacerlo de una sola vez las 32 imagenes de los dientes 
            // (ver los controles de presentacion de varias imagenes)
            // revisar todo se agrego el codigo para que evitar los errores. 
            DropDownList control = new DropDownList();
            //control.ID = "editor";
            //control.Items.Add("0");
            //control.Items.Add("1");
            //control.Items.Add("2");
            //control.Items.Add("3");
            //control.Items.Add("4");
            //control.Items.Add("5");
            //control.SelectedIndexChanged += control_SelectedIndexChanged;

            return control;
        }

        void control_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditValueChangedHandler(sender, e);
        }
        protected override object GetControlValueCore()
        {
            int result = 0;
            if (int.TryParse(((DropDownList)Editor).SelectedValue, out result))
            {
                return result;
            }
            return 0;
        }
        protected override void ReadEditModeValueCore()
        {
            ((DropDownList)Editor).SelectedValue = ((int)PropertyValue).ToString();
        }
        protected override void ReadViewModeValueCore()
        {
            ((Label)InplaceViewModeEditor).Text = ((int)PropertyValue).ToString();
        }
    }
}
