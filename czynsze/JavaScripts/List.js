function Init(table) {
    var inactive = false;
    var buttons =
    [
        document.getElementById("editaction"),
        document.getElementById("deleteaction"),
        document.getElementById("browseaction"),
        document.getElementById("moveaction"),
        document.getElementById("saldo"),
        document.getElementById("MonthlySumOfComponentreport"),
        document.getElementById("SumOfTurnoversOnreport"),
        document.getElementById("turnoversEditing")
    ];

    switch (table) {
        case "InactivePlaces":
        case "InactiveTenants":
            inactive = true;
            buttons[buttons.length] = getElementById("addaction");

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
}

/*function InitInactive() {
    var addingButton = document.getElementById("addaction");

    if (addingButton != null)
        addingButton.disabled = true;

    var idRadio = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio')
            idRadio[i].onchange = function () { ChangeRow(this.id, true); }
}*/

/*function InitReceivablesAndTurnoversOfTenant() {
    var footer = document.getElementByClassName("tableFooterRow");

    if (footer.length > 0) {
        alert('mam dziada');
    }
}*/

function Redirect(href) {
    var selectedRow = document.getElementsByClassName("selectedRow");

    if (selectedRow.length > 0) {
        var id = selectedRow[0].id.replace("_row", "");

        window.location.href = href + "&id=" + id;
    }
}