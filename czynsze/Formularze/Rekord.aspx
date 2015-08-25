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
        string table = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))];

        switch (table)
        {
            case "Buildings":
                %>
                    InitBuilding();
                <%
    
                break;

            case "Tenants":
                %>
                    InitTenant();
                <%
    
                break;

            case "Places":
                %>
                    InitPlace();
                <%
    
                break;

            case "Communities":
                %>
                    InitCommunity();
                <%
    
                break;

            case "RentComponents":
                %>
                    InitRentComponent();
                <%
    
                break;

            case "Attributes":
                %>
                    InitAttribute();
                <%
    
                break;
  
                case "Users":
                %>
                    InitUser();
                <%
    
                break;
        }
        %>
    </script>
</asp:Content>
