<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Record.aspx.cs" Inherits="czynsze.Record" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JavaScripts/Record.js"></script>
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
            case "Places":
                Control kod_lok = column0.FindControl("kod_lok");
                string kod_lok_id = String.Empty;

                Control nr_lok = column0.FindControl("nr_lok");
                string nr_lok_id = String.Empty;

                Control adres = column0.FindControl("adres");
                string adres_id = String.Empty;

                Control adres_2 = column0.FindControl("adres_2");
                string adres_2_id = String.Empty;

                if (kod_lok != null)
                    kod_lok_id = kod_lok.ClientID;

                if (nr_lok != null)
                    nr_lok_id = nr_lok.ClientID;

                if (adres != null)
                    adres_id = adres.ClientID;

                if (adres_2 != null)
                    adres_2_id = adres_2.ClientID;
                
                %>
                InitPlaces('<%=kod_lok_id%>', '<%=nr_lok_id%>', '<%=adres_id%>', '<%=adres_2_id%>');
                <%
    
                break;
            case "RentComponents":
                Control s_zaplat = column0.FindControl("s_zaplat");
                string s_zaplat_Id = String.Empty;

                if (s_zaplat != null)
                    s_zaplat_Id = s_zaplat.ClientID;
                    
                %>
                InitRentComponent('<%=s_zaplat_Id%>');
                <%
    
                break;
        }
        %>
    </script>
</asp:Content>
