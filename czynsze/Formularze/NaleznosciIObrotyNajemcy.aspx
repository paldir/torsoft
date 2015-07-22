<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NaleznosciIObrotyNajemcy.aspx.cs" Inherits="czynsze.Formularze.NaleznosciIObrotyNajemcy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/List.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="display: table; margin: 0 auto;">
        <form id="form" method="post" runat="server">
            <div id="miejscaNagłówka" runat="server"></div>
            <div id="miejscePrzycisków" runat="server"></div>
            <div id="miejsceTabeli" class="placeOfMainTable" runat="server"></div>
            <div id="miejscePodTabelą" class="placeUnderMainTable" runat="server"></div>
        </form>
    </div>
    <script>
        <%
        string table = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))];
        %>

        Init("<%=table%>");
    </script>
</asp:Content>
