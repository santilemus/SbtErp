using System;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;

public partial class Default : BaseXafPage
{
    protected override ContextActionsMenu CreateContextActionsMenu()
    {
        return new ContextActionsMenu(this, "Edit", "RecordEdit", "ObjectsCreation", "ListView", "Reports");
    }
    public override Control InnerContentPlaceHolder
    {
        get
        {
            return Content;
        }
    }

    //protected void Page_Init()
    //{
    //    CustomizeTemplateContent += (s, e) => {
    //        IHeaderImageControlContainer content = TemplateContent as IHeaderImageControlContainer;
    //        if (content == null) return;
    //        content.HeaderImageControl.DefaultThemeImageLocation = "Images";
    //        content.HeaderImageControl.ImageName = "Rep.png";
    //        //content.HeaderImageControl.Width = System.Web.UI.WebControls.Unit.Pixel(30);
    //        //content.HeaderImageControl.Height = System.Web.UI.WebControls.Unit.Pixel(30);
    //    };
    //}
}
