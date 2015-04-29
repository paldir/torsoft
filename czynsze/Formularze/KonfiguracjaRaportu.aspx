<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KonfiguracjaRaportu.aspx.cs" Inherits="czynsze.Formularze.KonfiguracjaRaportu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/raportConfiguration.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <form method="post" target="_blank" runat="server">
        <table>
            <tr>
                <td id="placeOfConfigurationFields" runat="server"></td>
            </tr>
        </table>            
        <asp:Button ID="generationButton" Text="Generuj" runat="server" />
    </form>
    <script>
        <%
        string key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("raport"));
        string raport = key.Replace("raport", String.Empty).Substring(key.LastIndexOf('$') + 1);

            switch (raport)
            {
                case "PlacesInEachBuilding":
                    %>
                        InitPlacesInEachBuilding();
                    <%
    
                    break;
            }
        %>
    </script>
</asp:Content>
