<%@ Page Title="Kwota czynszu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RentAmount.aspx.cs" Inherits="czynsze.Forms.RentAmount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="placeOfPlaces" runat="server"></div>
                <hr />
                <div id="placeOfBuildings" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
