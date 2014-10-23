function Init() {
    var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");
    var movingButton = document.getElementById("moveaction");
    var saldoButton = document.getElementById("saldo");

    if (editingButton != null)
        editingButton.disabled = true;

    if (deletingButton != null)
        deletingButton.disabled = true;

    if (browsingButton != null)
        browsingButton.disabled = true;

    if (movingButton != null)
        movingButton.disabled = true;

    if (saldoButton != null) {
        saldoButton.disabled = true;

        var subMenu = document.getElementsByClassName("superMenu");

        if (subMenu.length > 0)
            subMenu[0].className = "superMenu superMenuDisabled";
    }

    var idRadio = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio')
            idRadio[i].onchange = function () { ChangeRow(this.id, false); }
}

function InitInactive() {
    var addingButton = document.getElementById("addaction");

    if (addingButton != null)
        addingButton.disabled = true;

    var idRadio = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio')
            idRadio[i].onchange = function () { ChangeRow(this.id, true); }
}

function Redirect(href) {
    var selectedRow = document.getElementsByClassName("selectedRow");

    if (selectedRow.length > 0) {
        var id = selectedRow[0].id.replace("_row", "");

        window.location.href = href + "&id=" + id;
    }
}