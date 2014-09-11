function Init(editingButtonId, deletingButtonId, browsingButtonId) {
    var editingButton = document.getElementById(editingButtonId);
    var deletingButton = document.getElementById(deletingButtonId);
    var browsingButton = document.getElementById(browsingButtonId);

    editingButton.disabled = true;
    deletingButton.disabled = true;
    browsingButton.disabled = true;

    var idRadio = document.getElementsByTagName('input');

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio')
            idRadio[i].onchange = function () { ChangeRow(this.id, editingButton.id, deletingButton.id, browsingButton.id); }
}

function ChangeRow(rowId, editingButtonId, deletingButtonId, browsingButtonId) {
    var editingButton = document.getElementById(editingButtonId);
    var deletingButton = document.getElementById(deletingButtonId);
    var browsingButton = document.getElementById(browsingButtonId);

    editingButton.disabled = false;
    deletingButton.disabled = false;
    browsingButton.disabled = false;

    var selectedRow = document.getElementsByClassName("selectedRow")[0];

    if (selectedRow != null)
        selectedRow.className = "tableRow";

    var row = document.getElementById(rowId + "_row");

    if (row != null)
        row.className = "selectedRow";
}