<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record.aspx.cs" Inherits="czynsze.Record" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form id="form" method="post" runat="server">
        <div id="placeOfHeading" runat="server"></div>
        <table>
            <tr id="formRow" runat="server">
                <td id="column0" style="vertical-align: top;"></td>
                <td id="column1" style="vertical-align: top;"></td>
                <td id="column2" style="vertical-align: top;"></td>
                <td id="column3" style="vertical-align: top;"></td>
            </tr>
        </table>
        <div id="placeOfButtons" runat="server"></div>
    </form>
</asp:Content>
