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

    var tableRows = document.getElementsByClassName('tableRow');

    if (tableRows != null) {
        var widthsOfColumns = new Array(tableRows[0].cells.length)

        for (var i = 0; i < widthsOfColumns.length; i++)
            widthsOfColumns[i] = 0;

        for (var i = 0; i < tableRows.length; i++)
            for (var j = 0; j < tableRows[i].cells.length; j++)
                if (tableRows[i].cells[j].clientWidth > widthsOfColumns[j])
                    widthsOfColumns[j] = tableRows[i].cells[j].clientWidth;
        
        var headers = document.getElementsByClassName("tableHeaderCell");

        if (headers != null) {
            for (var i = 0; i < headers.length; i++)
                if (headers[i].clientWidth < widthsOfColumns[i])
                    headers[i].clientWidth = widthsOfColumns[i];
        }
    }
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