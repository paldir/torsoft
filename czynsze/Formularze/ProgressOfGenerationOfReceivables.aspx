<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgressOfGenerationOfReceivables.aspx.cs" Inherits="czynsze.Formularze.ProgressOfGenerationOfReceivables" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trwa generacja należności...</title>
    <link type="text/css" rel="stylesheet" href="../StyleSheet.css" />
    <link rel="icon" type="image/png" href="../Images/icon.ico" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="placeOfResultWindow">
            <div class="resultWindow">
                <asp:ScriptManager runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="updatePanel" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                    <ContentTemplate>
                        <div>Postęp generacji należności:</div>
                        <br />
                        <div id="progress" runat="server"></div>
                        <br />
                        <div id="info" runat="server">Proszę cierpliwie czekać.</div>
                        <asp:Timer ID="timer" Interval="2500" OnTick="timer_Tick" runat="server"></asp:Timer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
