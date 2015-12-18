<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SkladnikiCzynszuLokalu.aspx.cs" Inherits="czynsze.Formularze.SkladnikiCzynszuLokalu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../JavaScripts/RentComponentsOfPlace.js"></script>
    <script src="../JavaScripts/Script.js"></script>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.css" />
</head>
<body>
    <form id="form" runat="server">
    <div>
        <div id="placeOfButtons" runat="server"></div>
        <div id="placeOfTable" runat="server"></div>
        <div id="placeOfWindow">
            <div id="placeOfNewComponent" runat="server"></div>
            <div id="placeOfAmount" runat="server"></div>
            <div id="placeOfDate" runat="server"></div>
            <div id="placeOfButtonsOfWindow" runat="server"></div>
        </div>
    </div>
    </form>

    <script>
        <%
        List<int> componentsWithAmount = (List<int>)ViewState["componentsWithAmount"];
        int id = (int)ViewState["id"];
        string arrayOfComponentsWithAmount = String.Empty;

        foreach (int componentWithAmount in componentsWithAmount)
            arrayOfComponentsWithAmount += componentWithAmount.ToString() + ", ";
        %>
        Init(<%=id%>,[<%=arrayOfComponentsWithAmount%>]);
    </script>
</body>
</html>
