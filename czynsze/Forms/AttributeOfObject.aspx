<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttributeOfObject.aspx.cs" Inherits="czynsze.Forms.AttributeOfObject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../JavaScripts/AttributeOfObject.js"></script>
</head>
<body>
    <form id="form" action="AttributeOfObject.aspx" runat="server">
        <div id="placeOfTable" runat="server"></div>
        Nowa cecha: <span id="placeOfDropDown" runat="server"></span>Wartość: <span id="placeOfValue" runat="server"></span>
    </form>
    <script>
        <%
        List<int> numericAttributes;
        
        using (czynsze.DataAccess.Czynsze_Entities db = new czynsze.DataAccess.Czynsze_Entities())
            numericAttributes = db.attributes.Where(a => a.nr_str == "N").Select(a => a.kod).ToList();

        string jSArray = String.Empty;

        foreach (int numericAttribute in numericAttributes)
            jSArray += numericAttribute.ToString() + ", ";

        if (numericAttributes.Count > 0)
            jSArray = jSArray.Remove(jSArray.Length - 2);
        %>
        var numericAttributes = [<%=jSArray%>]

        Init(numericAttributes);
    </script>
</body>
</html>
