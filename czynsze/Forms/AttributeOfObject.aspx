<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttributeOfObject.aspx.cs" Inherits="czynsze.Forms.AttributeOfObject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../JavaScripts/AttributeOfObject.js"></script>
    <script src="../JavaScripts/Script.js"></script>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.css" />
</head>
<body>
    <form id="form" method="post" runat="server">
        <div id="placeOfButtons" runat="server"></div>
        <div id="placeOfTable" runat="server"></div>
        <div id="placeOfNewAttribute" runat="server"></div>
        <div id="placeOfEditingWindow" runat="server"></div>
    </form>
    <script>
        <%
        List<czynsze.DataAccess.Attribute> attributes;
        
        using (czynsze.DataAccess.Czynsze_Entities db = new czynsze.DataAccess.Czynsze_Entities())
            attributes = db.attributes.ToList();

        string ids = String.Empty;
        string types = String.Empty;
        string units = String.Empty;
        string defaults = String.Empty;

        foreach (czynsze.DataAccess.Attribute attribute in attributes)
        {
            ids += "'" + attribute.kod.ToString() + "', ";
            types += "'" + attribute.nr_str + "', ";
            units += "'" + attribute.jedn.ToString() + "', ";

            switch (attribute.nr_str)
            {
                case "N":
                    defaults += "'" + attribute.wartosc_n.ToString() + "', ";
                    break;
                case "C":
                    defaults += "'" + attribute.wartosc_s.Trim() + "', ";
                    break;
            }
        }

        if (ids.Length > 0)
        {
            ids = ids.Remove(ids.Length - 2);
            types = types.Remove(types.Length - 2);
            units = units.Remove(units.Length - 2);
            defaults = defaults.Remove(defaults.Length - 2);
        }
        %>
        var ids = [<%=ids%>];
        var types = [<%=types%>];
        var units = [<%=units%>];
        var defaults = [<%=defaults%>];

        Init(ids, types, units, defaults);
    </script>
</body>
</html>
