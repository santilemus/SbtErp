<%@ Control Language="C#" CodeBehind="DialogTemplateContentExt.ascx.cs" ClassName="DialogTemplateContentExt" Inherits="SBT.Apps.Erp.Web.DialogTemplateContentExt" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="xaf" %>

<style type="text/css">
    /*====================================================================================================================*/
</style>
<xaf:XafUpdatePanel ID="UPPopupWindowControl" runat="server">
    <xaf:XafPopupWindowControl runat="server" ID="PopupWindowControl" />
</xaf:XafUpdatePanel>
<div class="dialogContent newStylePopupContent">
    <div>
        <table id="headerTable" class="dialog headerTable gray borderBottom width100" style="position: initial; padding-left: 0px; padding-right: 0px">
            <tbody>
                <tr>
                    <td>
                        <table class="viewCaption">
                            <tbody>
                                <tr>
                                    <td>
                                        <xaf:XafUpdatePanel ID="UPVIC" runat="server">
                                            <xaf:ViewImageControl ID="VIC" runat="server" CssClass="ViewImage" />
                                        </xaf:XafUpdatePanel>
                                    </td>
                                    <td>
                                        <xaf:XafUpdatePanel ID="UPVH" runat="server" ForeColor="#4a4a4a" Font-Size="X-Large" Style="white-space: normal;">
                                            <xaf:ViewCaptionControl ID="VCC" runat="server" />
                                        </xaf:XafUpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td style="width: 50%">
                        <div style="float: right">
                            <xaf:XafUpdatePanel ID="UPSAC" runat="server">
                                <xaf:ActionContainerHolder runat="server" ID="SAC" ContainerStyle="Buttons" Orientation="Horizontal">
                                    <actioncontainers>
                                        <xaf:WebActionContainer ContainerId="Search" />
                                        <xaf:WebActionContainer ContainerId="FullTextSearch" />
                                        <xaf:WebActionContainer ContainerId="ObjectsCreation" DefaultActionID="New" AutoChangeDefaultAction="true" />
                                        <xaf:WebActionContainer ContainerId="Diagnostic" />
                                        <xaf:WebActionContainer ContainerId="PopupActions" />
                                        <xaf:WebActionContainer ContainerId="Save" DefaultActionID="Save" IsDropDown="true" AutoChangeDefaultAction="true" />
                                    </actioncontainers>
                                </xaf:ActionContainerHolder>
                            </xaf:XafUpdatePanel>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="viewSite" class="white viewSite" style="margin-top: 33px">
        <xaf:XafUpdatePanel ID="UPEI" runat="server" UpdatePanelForASPxGridListCallback="True">
            <xaf:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server" />
        </xaf:XafUpdatePanel>
        <xaf:XafUpdatePanel ID="UPVSC" runat="server">
            <xaf:ViewSiteControl ID="VSC" runat="server" />
        </xaf:XafUpdatePanel>
    </div>
</div>

<script type="text/javascript">
    (function() {
        var mainWindow = xaf.Utils.GetMainWindow();
        mainWindow.pageLoaded = false;
        window.NewStyle = true;

        $(window).on("load", function() {
            var mainWindow = xaf.Utils.GetMainWindow();
            mainWindow.pageLoaded = true;
            AdjustHeaderHeight();
            PageLoaded();
        });
    })();
</script>
