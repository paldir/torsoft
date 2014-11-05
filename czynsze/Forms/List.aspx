<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="czynsze.Forms.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/List.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="display: table; margin: 0 auto;">
        <form id="form" method="get" runat="server">
            <div id="placeOfHeading" runat="server"></div>
            <div id="placeOfMainTableButtons" runat="server"></div>
            <div id="placeOfMainTable" class="placeOfMainTable" runat="server"></div>
            <div id="placeUnderMainTable" class="placeUnderMainTable" runat="server"></div>
        </form>
    </div>
    <script>
        <%
            string table = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))];
        %>

        Init(table);
    </script>
</asp:Content>
