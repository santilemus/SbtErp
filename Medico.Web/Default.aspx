<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<%@ Register assembly="DevExpress.Web.v20.2, Version=20.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
    <link runat="server" rel="shortcut icon" href="sbtrec.ico" type="image/x-icon"/>
    <link runat="server" rel="icon" href="sbtrec.ico" type="image/ico"/>
</head>
<body class="VerticalTemplate">
    <form id="form2" runat="server">
    <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
    <div runat="server" id="Content" />
    </form>
</body>
</html>
