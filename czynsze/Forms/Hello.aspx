<%@ Page Title="Strona startowa" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hello.aspx.cs" Inherits="czynsze.Forms.Hello" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="helloScreen">
        <div>Witamy w systemie <span>CZYNSZE</span>!</div>
        <br />
        <br />
        Baza danych należy do: <span id="company" runat="server"></span><br />
        <br />
        Jesteś zalogowany jako: <span id="user" runat="server"></span><br />
        <br />
        Przeglądasz zbiory danych dla miesiąca: <span id="month" runat="server"></span>
    </div>
</asp:Content>
