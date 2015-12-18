<%@ Page Title="Proszę czekać..." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ladowanie.aspx.cs" Inherits="czynsze.Formularze.Ladowanie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <img src="../Images/loading.gif" id="loading" />
    <script>
        FinishLoading();
    </script>
</asp:Content>
