function Init(ids, types, units, defaults) {
    var deletingButton = document.getElementById("deletechildAction");

    if (deletingButton != null)
        deletingButton.disabled = true;

    var editingButton = document.getElementById("showEditingWindow");

    if (editingButton != null)
        editingButton.disabled = true;

    var idRadio = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < idRadio.length; i++)
        if (idRadio[i].type == 'radio') {
            idRadio[i].checked = false;
            idRadio[i].onchange = function () { ChangeRow(this.id, false); }
        }

    var kod = document.getElementById("kod");

    if (kod != null) {
        kod.onchange = function () { kod_onchange(this.value, ids, types, units, defaults); }

        kod_onchange(kod.value, ids, types, units, defaults);
    }
}

function kod_onchange(value, ids, types, units, defaults) {
    var wartosc = document.getElementById("wartosc");
    var unit = document.getElementById("unit");

    if (wartosc != null && unit != null) {
        var i = ids.indexOf(value);

        if (i != -1) {
            wartosc.value = defaults[i];

            switch (types[i]) {
                case "C":
                    wartosc.maxLength = wartosc.size = 25;
                    unit.innerHTML = "";
                    break;
                case "N":
                    wartosc.maxLength = wartosc.size = 16;
                    unit.innerHTML = units[ids.indexOf(value)];
                    break;
            }
        }
    }
}