<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudynkiWspolnoty.aspx.cs" Inherits="czynsze.Formularze.BudynkiWspolnoty" %>

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
                <div id="placeOfNewBuilding" runat="server"></div>
                <div id="placeOfComments" runat="server"></div>
                <div id="placeOfButtonsOfWindow" runat="server"></div>
            </div>
        </div>
    </form>
    <script>
        <%
        int id = (int)ViewState["id"];
        %>
        Init(<%=id%>);
    </script>
</body>
</html>
