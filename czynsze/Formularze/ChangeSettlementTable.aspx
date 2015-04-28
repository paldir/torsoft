<%@ Page Title="Zmiana tabeli rozliczeniowej" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeSettlementTable.aspx.cs" Inherits="czynsze.Forms.ChangeSettlementTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form runat="server">
        <div style="display: table; margin: 0 auto;">
            <h2>Zmiana tabeli rozliczeniowej</h2>
            <div class="placeOfMainTable">
                <div id="placeOfRadioButtons" runat="server"></div>
                <div id="placeOfButton" runat="server"></div>
            </div>
        </div>
    </form>
</asp:Content>
