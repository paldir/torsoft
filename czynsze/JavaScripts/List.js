function Init() {
    var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");

    if (editingButton != null)
        editingButton.disabled = true;

    if (deletingButton != null)
        deletingButton.disabled = true;

    if (browsingButton != null)
        browsingButton.disabled = true;

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

function ChangeRow(rowId, inactive) {
    var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");

    if (!inactive) {
        if (editingButton != null)
            editingButton.disabled = false;

        if (deletingButton != null)
            deletingButton.disabled = false;
    }

    if (browsingButton != null)
        browsingButton.disabled = false;

    var selectedRow = document.getElementsByClassName("selectedRow")[0];

    if (selectedRow != null)
        selectedRow.className = "tableRow";

    var row = document.getElementById(rowId + "_row");

    if (row != null)
        row.className = "selectedRow";
}