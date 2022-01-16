﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Controls;
using SBT.Apps.Banco.Module.Controllers;

namespace SBT.Apps.Erp.Module.Win.Controllers
{
    /// <summary>
    /// View controller que corresponde al BO BancoTransaccion para la vista de detalle
    /// </summary>
    /// <remarks>
    /// OJO: Probar con datos
    /// </remarks>
    public class vcBancoTransaccionDetailWin: vcBancoTransaccionDetail
    {
        public vcBancoTransaccionDetailWin(): base()
        {

        }
        protected override void CustomizeDisabledEditorsAppearance(ApplyAppearanceEventArgs e)
        {
            base.CustomizeDisabledEditorsAppearance(e);
            DXPropertyEditor dxEditor = e.Item as DXPropertyEditor;
            if (dxEditor != null && dxEditor.Control != null)
            {
                dxEditor.Caption = "Tarjeta" + dxEditor.Caption; 
                dxEditor.Control.Properties.BorderStyle = BorderStyles.Simple;
                dxEditor.Control.Properties.Appearance.BackColor = Color.RosyBrown;
            }
        }
    }
}
