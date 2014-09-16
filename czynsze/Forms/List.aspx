<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="czynsze.Forms.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/List.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form id="form" method="post" runat="server">
        <div id="placeOfHeading" runat="server"></div>
        <div id="placeOfMainTableButtons" runat="server"></div>
        <div id="placeOfMainTable" runat="server"></div>
    </form>
    <script>
        Init('<%=placeOfMainTableButtons.FindControl("editaction").ClientID%>', '<%=placeOfMainTableButtons.FindControl("deleteaction").ClientID%>', '<%=placeOfMainTableButtons.FindControl("browseaction").ClientID%>')
    </script>
</asp:Content>
