<%@ Page Title="Wykaz wg składnika" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WykazWgSkladnika.aspx.cs" Inherits="czynsze.Formularze.WykazWgSkladnika" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="pojemnikSkladnika" runat="server"></div>
                <div id="pojemnikReszty" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
