<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecordValidation.aspx.cs" Inherits="czynsze.Formularze.RecordValidation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" method="post" runat="server">
                <div id="placeOfMessage" runat="server"></div>
                <div id="placeOfButtons" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
