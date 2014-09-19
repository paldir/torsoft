function Init(editingButtonId, deletingButtonId, browsingButtonId) {
    var editingButton = document.getElementById(editingButtonId);
    var deletingButton = document.getElementById(deletingButtonId);
    var browsingButton = document.getElementById(browsingButtonId);

    if (editingButton != null)
        editingButton.disabled = true;

    if (deletingButton != null)
        deletingButton.disabled = true;

    if (browsingButton != null)
        browsingButton.disabled = true;

    var idRadio = document.querySelectorAll(".mainTable input");

    if (idRadio != null)
        for (var i = 0; i < idRadio.length; i++)
            if (idRadio[i].type == 'radio')
                idRadio[i].onchange = function () { ChangeRow(this.id, editingButton.id, deletingButton.id, browsingButton.id); }

    /*var tableRows = document.getElementsByClassName('tableRow');

    if (tableRows != null) {
        var widthsOfColumns = new Array(tableRows[0].cells.length)

        for (var i = 0; i < widthsOfColumns.length; i++)
            widthsOfColumns[i] = 0;

        for (var i = 0; i < tableRows[0].cells.length; i++)
            widthsOfColumns[i] = tableRows[0].cells[i].clientWidth;

        var headers = document.getElementsByClassName("tableHeaderCell");

        if (headers != null) {
            for (var i = 0; i < headers.length; i++)
                if (headers[i].clientWidth < widthsOfColumns[i])
                    headers[i].width = widthsOfColumns[i];
                else
                    widthsOfColumns[i] = headers[i].clientWidth;

            for (var i = 0; i < tableRows[0].cells.length; i++)
                tableRows[0].cells[i].width = widthsOfColumns[i];
        }
    }*/

    /*var header = document.querySelector(".mainTable thead");
    var headerRow = document.querySelector(".mainTable .tableHeaderRow");

    if (headerRow != null)
    {
        var widthsOfColumns = new Array(headerRow.cells.length);

        for (var i = 0; i < headerRow.cells.length; i++)
            widthsOfColumns[i] = headerRow.cells[i].clientWidth;

        if (header != null) {
            header.style.display = headerRow.style.display = "block";
            var body = document.querySelector(".mainTable tbody");

            if (body != null) {
                body.style.display = "inline-block";

                var rows = document.getElementsByClassName("tableRow");

                if (rows.length > 0) {
                    for (var i = 0; i < headerRow.cells.length; i++) {
                        headerRow.cells[i].width = widthsOfColumns[i] * 0.99;
                        rows[0].cells[i].width = widthsOfColumns[i];
                    }
                }
            }
        }
    }*/

    /*var table = document.querySelector(".mainTable");

    if (table != null)
    {
        var clone = table.cloneNode(true);
        clone.className = "mainTable mainTableClone";

        table.parentNode.appendChild(clone);
    }*/
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