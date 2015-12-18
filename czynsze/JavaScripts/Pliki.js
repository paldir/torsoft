function Init_Pliki() {
    var przyciskiSelf = ["delete", "otwarcieOknaDodawania"];
    var przyciskiBlank = ["browse"];
    var formularz = document.getElementById("form");

    for (var i = 0; i < przyciskiSelf.length; i++) {
        var przycisk = document.getElementById(przyciskiSelf[i]);

        if (przycisk != null)
            przycisk.onclick = function () {
                formularz.target = "_self";
                formularz.action = "Pliki.aspx";
            };
    }

    for (var i = 0; i < przyciskiBlank.length; i++) {
        var przycisk = document.getElementById(przyciskiBlank[i]);

        if (przycisk != null)
            przycisk.onclick = function () {
                formularz.target = "_blank";
                formularz.action = "Plik.aspx";
            };
    }
}