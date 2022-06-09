<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AngularMaterialDateBox.ascx.vb" Inherits="DominionPayroll.AngularMaterialDateBox" %>

<asp:TextBox ID="txtDate" class="txtDate" runat="server"></asp:TextBox>
<ds-angular-material-datebox data="<%=data%>" enabled="<%=txtDate.Enabled%>" ctrl="<%=ctrl%>"></ds-angular-material-datebox>
