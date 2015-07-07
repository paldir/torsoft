<%@ Page Title="Treści opisów" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TresciOpisow.aspx.cs" Inherits="czynsze.Formularze.TresciOpisow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="placeOfHeading" runat="server"></div>
                <div id="placeOfFields" runat="server"></div>
                <div id="placeOfButtons" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
