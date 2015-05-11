<%@ Page Title="Składniki czynszu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SkladnikiCzynszu.aspx.cs" Inherits="czynsze.Formularze.SkladnikiCzynszu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="pojemnikRadio" runat="server"></div>
                <div id="pojemnikReszty" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
