<%@ Page Title="" Language="C#" MasterPageFile="~/NiebieskieOkno.master" AutoEventWireup="true" CodeBehind="Analiza.aspx.cs" Inherits="czynsze.Formularze.Analiza" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ZawartośćNiebieskiegoOkna" runat="server">
    <form id="form" runat="server">
        <div id="placeOfPlaces" runat="server"></div>
        <hr />
        <div id="placeOfBuildings" runat="server"></div>
        <hr />
        <div id="placeOfCommunities" runat="server"></div>
        <hr />
        <div id="placeOfOthers" class="special" runat="server"></div>
    </form>
</asp:Content>
