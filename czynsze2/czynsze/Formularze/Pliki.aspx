<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pliki.aspx.cs" Inherits="czynsze.Formularze.Pliki" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../JavaScripts/Script.js"></script>
    <script src="../JavaScripts/List.js"></script>
        <script src="../JavaScripts/Pliki.js"></script>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.css" />
    <title></title>
</head>
<body>
    <form id="form" method="post" enctype="multipart/form-data" runat="server">
        <div id="miejscePrzycisków" runat="server"></div>
        <div id="miejsceTabeli" runat="server"></div>
        <div id="miejsceOknaDodawania" runat="server"></div>
    </form>

    <script>
        Init();
        Init_Pliki();
    </script>
</body>
</html>
