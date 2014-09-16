<%@ Page Title="Konfiguracja wydruku" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="czynsze.Forms.ReportConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form target="_blank" runat="server">
        <table>
            <tr>
                <td id="placeOfConfigurationFields" runat="server"></td>
            </tr>
        </table>            
        <asp:Button ID="generationButton" Text="Generuj" runat="server" />
    </form>
</asp:Content>
