<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="czynsze.Forms.ReportConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JavaScripts/ReportConfiguration.js"></script>
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
        string key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("report"));
        string report = key.Replace("report", String.Empty).Substring(key.LastIndexOf('$') + 1);

            switch (report)
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
