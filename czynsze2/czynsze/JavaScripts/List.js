﻿function Init(table) {
    var inactive = false;
    var buttons =
    [
        document.getElementById("edit"),
        document.getElementById("delete"),
        document.getElementById("browse"),
        document.getElementById("move"),
        document.getElementById("saldo"),
        document.getElementById("SumyMiesięczneSkładnika"),
        document.getElementById("SumOfTurnoversOnraport"),
        document.getElementById("turnoversEditing")
    ];

    switch (table) {
        case "NieaktywneLokale":
        case "NieaktywniNajemcy":
            inactive = true;
            //buttons[buttons.length] = getElementById("addaction");

            break;
    }

    for (var i = 0; i < buttons.length; i++)
        if (buttons[i] != null)
            buttons[i].disabled = true;

    var subMenu = document.getElementsByClassName("superMenu");

    for (var i = 0; i < subMenu.length; i++)
        subMenu[i].className = "superMenu superMenuDisabled";

    var idRadio = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio') {
            idRadio[i].checked = false;
            idRadio[i].onchange = function () { ChangeRow(this.id, inactive, buttons, subMenu); }
        }

    var naglowki = document.getElementsByClassName("sortLink");

    for (var i = 0; i < naglowki.length; i++) {
        var naglowek = naglowki[i];
        naglowek.id = i;

        naglowek.addEventListener("click", function () { naglowek_onclick(naglowki, this) });
    }
}

function Redirect(href) {
    var selectedRow = document.getElementsByClassName("selectedRow");

    if (selectedRow.length > 0) {
        var id = selectedRow[0].id.replace("_row", "");

        window.location.href = href + "&id=" + id;
    }
}

function naglowek_onclick(wszystkieNaglowki, zrodlo) {
    var tablica = document.getElementById("tablica");

    if (tablica != null) {
        var tbody = tablica.tBodies[0];
        var wiersze = Array.prototype.slice.call(tbody.getElementsByTagName("tr"), 0);
        var indeks = zrodlo.id;
        var mnoznik;
        var sortowacRosnaco = zrodlo.className.indexOf("asc") == -1;

        for (var i = 0; i < wszystkieNaglowki.length; i++)
            wszystkieNaglowki[i].className = "sortLink";

        if (sortowacRosnaco) {
            mnoznik = 1;
            zrodlo.className += " asc";
        }
        else {

            mnoznik = -1;
            zrodlo.className += " desc";
        }

        if (wiersze.length > 0) {
            var funkcjaSortujaca;
            var tekstPierwszegoWiersza = wiersze[0].cells[indeks].textContent.replace(",", ".");

            if (isNaN(tekstPierwszegoWiersza) || tekstPierwszegoWiersza.length == 0)
                funkcjaSortujaca = function (a, b) { return mnoznik * a.cells[indeks].textContent.localeCompare(b.cells[indeks].textContent) };
            else
                funkcjaSortujaca = function (a, b) { return mnoznik * (Number(a.cells[indeks].textContent.replace(",", ".")) - Number(b.cells[indeks].textContent.replace(",", "."))) };

            wiersze = wiersze.sort(funkcjaSortujaca);

            for (var i = 0; i < wiersze.length; i++)
                tbody.appendChild(wiersze[i]);
        }
    }
}