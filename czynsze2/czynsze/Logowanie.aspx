﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logowanie.aspx.cs" Inherits="czynsze.Logowanie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logowanie</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="login" method="post" action="Formularze\WalidacjaUzytkownika.aspx">
        <div id="placeOfLogin">
            <span style="position: absolute; left: -3px; top: -20px;" runat="server">System CZYNSZE</span>
            <span id="companyName" style="position: absolute; right: -3px; top: -20px;" runat="server"></span>
            <table>
                <tr>
                    <td style="text-align: right">Użytkownik: </td>
                    <td>
                        <input name="uzytkownik" type="text" size="40" maxlength="40" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Hasło: </td>
                    <td>
                        <input name="haslo" type="password" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right">
                        <input name="Submit" type="submit" value="Zaloguj" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <img src="Images/logo.png" />
                    </td>
                    <td>
                        <b>TORSOFT Jan Konieczny</b><br />
                        Biuro: 87-100 Toruń, ul. Lubicka 23/1<br />
                        tel. 56 6591441<br />
                        email: <a href="mailto:biuro@torsoft.pl">biuro@torsoft.pl</a><br />
                        <a href="http://www.torsoft.pl/">http://www.torsoft.pl/</a>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script>
        <%
        string przyczyna = Request.Params["przyczyna"];

        if (przyczyna != null)
        {
            string message;

            switch (przyczyna)
            {
                case "NiepoprawneDaneUwierzytelniajace":
                    message = "Musisz podać prawidłową nazwę użytkownika i hasło.";

                    break;

                case "NiezalogowanyLubSesjaWygasla":
                    message = "Nie jesteś zalogowany lub Twoja sesja wygasła.";

                    break;

                default:
                    message = przyczyna;

                    break;
            }
                
                %>
        alert('<%=message%>');
        <%
            }
        %>
    </script>
</body>
</html>
