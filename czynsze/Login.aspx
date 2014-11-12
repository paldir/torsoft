<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="czynsze.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logowanie</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <form id="login" method="post" action="/czynsze1/UserValidation.cxp">
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
                    <td colspan="2" style="text-align: center">
                        <img src="Images/logo.png" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script>
        <%
            string reason = Request.Params["reason"];
        
            if (reason != null)
            {
                string message;

                switch (reason)
                {
                    case "IncorrectCredentials":
                        message = "Musisz podać prawidłową nazwę użytkownika i hasło.";
                        
                        break;

                    case "NotLoggedInOrSessionExpired":
                        message = "Nie jesteś zalogowany lub Twoja sesja wygasła.";
                        
                        break;

                    default:
                        message = reason;
                        
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
