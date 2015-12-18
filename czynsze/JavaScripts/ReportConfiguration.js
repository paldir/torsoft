function InitPlacesInEachBuilding() {
    var kod_1_start = document.getElementById("kod_1_start");
    var kod_1_start_dropdown = document.getElementById("kod_1_start_dropdown");
    var kod_1_end = document.getElementById("kod_1_end");
    var kod_1_end_dropdown = document.getElementById("kod_1_end_dropdown");

    if (kod_1_start != null && kod_1_start_dropdown != null) {
        kod_1_start.onchange = function () { kod_1_onchange(this.value, kod_1_start_dropdown); }
        kod_1_start_dropdown.onchange = function () { kod_1_dropdown_onchange(this.value, kod_1_start); }
    }

    if (kod_1_end != null && kod_1_end_dropdown != null) {
        kod_1_end.onchange = function () { kod_1_onchange(this.value, kod_1_end_dropdown); }
        kod_1_end_dropdown.onchange = function () { kod_1_dropdown_onchange(this.value, kod_1_end); }
    }
}

function kod_1_onchange(value, kod_1_dropdown) {
    value = Number(value);

    for (var i = kod_1_dropdown.options.length - 1; i >= 0; i--)
        if (Number(kod_1_dropdown.options.item(i).value) <= value) {
            kod_1_dropdown.selectedIndex = i;

            break;
        }
}

function kod_1_dropdown_onchange(value, kod_1) {
    kod_1.value = value;
}