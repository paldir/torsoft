function Init() {
    var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");
    var movingButton = document.getElementById("moveaction");

    if (editingButton != null)
        editingButton.disabled = true;

    if (deletingButton != null)
        deletingButton.disabled = true;

    if (browsingButton != null)
        browsingButton.disabled = true;

    if (movingButton != null)
        movingButton.disabled = true;

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