var idWiersza=0;

function isInteger(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isFloat(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}

function isDate(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var klawisz = String.fromCharCode(charCode);
    var cyfra = Number(klawisz);
    var pole = evt.currentTarget;
    var wartosc = pole.value;
    var poprzedniaCyfra = -1;
    var dlugosc = wartosc.length;

    //dlugosc = pole.selectionStart; // DU�Y POTENCJA�

    if (dlugosc > 0)
        poprzedniaCyfra = Number(wartosc[dlugosc - 1]);

    if (dlugosc >= 7) {
        if (cyfra >= 0 && cyfra <= 9)
            return true;
        else
            return false;
    }
    else
        switch (dlugosc) {
            case 0:
            case 10:
                if (cyfra >= 0 && cyfra <= 3)
                    return true;
                else
                    return false;

            case 1:
                var dodawa� = false;

                switch (poprzedniaCyfra) {
                    case 0:
                    case 1:
                    case 2:
                        dodawa� = cyfra >= 0 && cyfra <= 9;

                        break;

                    case 3:
                        switch (cyfra) {
                            case 0:
                            case 1:
                                dodawa� = true;

                                break;
                        }

                        break;
                }

                if (dodawa�)
                    pole.value += klawisz + "-";

                return false;

            case 3:
                switch (cyfra) {
                    case 0:
                    case 1:
                        return true;

                    default:
                        return false;
                }

            case 4:
                var dodawa� = false;

                switch (poprzedniaCyfra) {
                    case 0:
                        dodawa� = cyfra >= 0 && cyfra <= 9;

                        break;

                    case 1:
                        dodawa� = cyfra >= 0 && cyfra <= 2;

                        break;
                }

                if (dodawa�)
                    pole.value += klawisz + "-";

                return false;

            case 6:
                switch (cyfra) {
                    case 1:
                    case 2:
                        return true;

                    default:
                        return false;
                }

            default:
                return false;
        }

    return false;
}

function ClearSelectionOfRow() {
    var radios = document.getElementsByTagName('input');

    for (var i = 0; i < radios.length; i++)
        if (radios[i].type == 'radio' && radios[i].className == 'rowRadio')
            radios[i].checked = false;
}

function ChangeRow(rowId) {
	idWiersza=rowId;
    var buttons = Array(4)
    buttons[1] = document.getElementById("editingButton");
    buttons[2] = document.getElementById("deletingButton");
    buttons[3] = document.getElementById("browsingButton");
	buttons[4] = document.getElementById("przyciskOpisu");

    for (var i = 0; i < buttons.length; i++)
        if (buttons[i] != null)
            buttons[i].disabled = false;

    var selectedRow = document.getElementsByClassName("selectedRow")[0]

    if (selectedRow != null)
        selectedRow.className = "row";

    var row = document.getElementById(rowId + "_row");

    if (row != null)
        row.className = "selectedRow";

    var editingButtonSpecial = document.getElementById("editingButtonSpecial");

    if (editingButtonSpecial != null && rentComponentsWithAmount != null) {
        editingButtonSpecial.disabled = true;

        for (var i = 0; i < rentComponentsWithAmount.length; i++) {
            if (rentComponentsWithAmount[i] == rowId) {
                editingButtonSpecial.disabled = false;

                break;
            }
        }
    }
}

function Init() {
    var browsingButton = document.getElementById("browsingButton");

    if (browsingButton != null) {
        browsingButton.onclick = function () {
            var form = document.getElementById("formOfDocumentRemoval");
            form.action = "Plik.cxp";
            form.target = "_blank";
        };
    }

    var funkcja = function () {
        var form = document.getElementById("formOfDocumentRemoval");
        form.action = "Pliki.cxp";
        form.target = "_self";
    };

    var deletingButton = document.getElementById("deletingButton");

    if (deletingButton != null)
        deletingButton.onclick = funkcja;

    var editingButton = document.getElementById("editingButton");

    if (editingButton != null)
        editingButton.onclick = funkcja;

    var addingButton = document.getElementById("addingButton");

    if (addingButton != null)
        addingButton.onclick = funkcja;
}

function opis(tabela)
{
	$.ajax({
		url: "Opis.cxp",
		data:{
			id: idWiersza,
			tabela: tabela
		},
		success: function(dane){
			dane=dane.replace(/\\n/g, "\n");

			dane=dane.replace(/&#185;/, "�"); //a
			dane=dane.replace(/&#230;/, "�"); //c
			dane=dane.replace(/&#234;/, "�"); //e
			dane=dane.replace(/&#179;/, "�"); //l
			dane=dane.replace(/&#241;/, "�"); //n
			dane=dane.replace(/&#243;/, "�"); //o
			dane=dane.replace(/&#156;/, "�"); //s
			dane=dane.replace(/&#159;/, "�"); //x
			dane=dane.replace(/&#191;/, "�"); //z
				
			alert(dane);
		}
	});
}