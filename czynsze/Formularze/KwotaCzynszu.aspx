<%@ Page Title="Kwota czynszu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KwotaCzynszu.aspx.cs" Inherits="czynsze.Formularze.KwotaCzynszu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="placeOfPlaces" runat="server"></div>
                <hr />
                <div id="placeOfBuildings" runat="server"></div>
                <hr />
                <div id="placeOfCommunities" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
