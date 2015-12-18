function Init(id, componentsWithAmount) {
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
}