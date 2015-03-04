function Init(componentsWithAmount) {
    var buttons =
        [
            document.getElementById("removeChildAction"),
            document.getElementById("editShowWindow")
        ];

    for (var i = 0; i < buttons.length; i++)
        if (buttons[i] != null)
            buttons[i].disabled = true;

    var radios = document.querySelectorAll(".mainTable input");

    for (var i = 0; i < radios.length; i++)
        if (radios[i].type == 'radio') {
            radios[i].checked = false;
            radios[i].onchange = function () { ChangeRow(this.id, false, buttons); }
        }

    var nr_skl = document.getElementById("nr_skl");

    if (nr_skl != null)
        nr_skl.onchange = function () { nr_skl_onchange(this.value, componentsWithAmount); }

    nr_skl_onchange(-1, componentsWithAmount);
}

function nr_skl_onchange(id, componentsWithAmount)
{
    var placeOfAmount = document.getElementById("placeOfAmount");

    if (placeOfAmount != null) {
        for (var i = 0; i < componentsWithAmount.length; i++)
            if (componentsWithAmount[i] == id) {
                placeOfAmount.hidden = false;

                return;
            }

        placeOfAmount.hidden = true;
    }
}