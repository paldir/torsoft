<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rekord.aspx.cs" Inherits="czynsze.Formularze.Rekord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/Record.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="placeOfHeading" runat="server"></div>
    <div id="placeOfPreview" runat="server"></div>
    <div id="placeOfTabButtons" runat="server"></div>
    <form id="form" method="post" runat="server">
        <div class="tabBorder">
            <div class="tabSeparator"></div>
            <table id="dane_tab" class="tab">
                <tr id="formRow" runat="server">
                    <td id="column0" style="vertical-align: top;" runat="server"></td>
                    <td id="column1" style="vertical-align: top;" runat="server"></td>
                    <td id="column2" style="vertical-align: top;" runat="server"></td>
                    <td id="column3" style="vertical-align: top;" runat="server"></td>
                </tr>
            </table>
            <div id="placeOfTabs" runat="server"></div>
        </div>
        <div id="placeOfButtons" runat="server"></div>
    </form>
    <script>
        Init();

        <%
        czynsze.Enumeratory.Tabela table = PobierzWartośćParametru<czynsze.Enumeratory.Tabela>("table");

        switch (table)
        {
            case czynsze.Enumeratory.Tabela.Budynki:
                %>
                    InitBuilding();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.AktywniNajemcy:
                %>
                    InitTenant();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.AktywneLokale:
                %>
                    InitPlace();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.Wspolnoty:
                %>
                    InitCommunity();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.SkladnikiCzynszu:
                %>
                    InitRentComponent();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.Atrybuty:
                %>
                    InitAttribute();
                <%
    
                break;

            case czynsze.Enumeratory.Tabela.Uzytkownicy:
                %>
                    InitUser();
                <%
    
                break;
        }
        %>
    </script>
</asp:Content>
