﻿<%@ Page Title="Zmiana miesiąca" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZmianaDaty.aspx.cs" Inherits="czynsze.Formularze.ZmianaDaty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form runat="server">
        <div style="display: table; margin: 0 auto;">
            <h2>Zmiana miesiąca</h2>
            <div class="placeOfMainTable">
                Zbiory danych za rok <span id="placeOfYear" runat="server"></span> miesiąc <span id="placeOfMonth" runat="server"></span>
                <div id="placeOfButton" runat="server"></div>
            </div>
        </div>
    </form>
</asp:Content>
