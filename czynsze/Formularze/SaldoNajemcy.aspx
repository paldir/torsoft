<%@ Page Title="" Language="C#" MasterPageFile="~/NiebieskieOkno.master" AutoEventWireup="true" CodeBehind="SaldoNajemcy.aspx.cs" Inherits="czynsze.Formularze.SaldoNajemcy" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ZawartośćNiebieskiegoOkna" runat="server">
    <form runat="server">
        <table>
            <tr><td>Saldo</td><td id="saldo" class="numericTableCell" runat="server"></td></tr>
            <tr><td>Saldo na dzień <span id="dzień" runat="server"></span></td><td id="saldoNaDzień" class="numericTableCell" runat="server"></td></tr>
            <tr><td>W tym noty odsetkowe</td><td id="noty" class="numericTableCell" runat="server"></td></tr>
        </table>
    </form>
</asp:Content>
