function MainInit() {
    var buttonOfChangingDate = document.getElementById("buttonOfChangingDate");

    if (buttonOfChangingDate != null) {
        buttonOfChangingDate.onclick = function () { buttonOfChangingDate_click(); }
    }

    var placeOfMinutes = document.getElementById("placeOfMinutes");
    var placeOfSeconds = document.getElementById("placeOfSeconds");

    if (placeOfMinutes != null && placeOfSeconds != null) {
        placeOfMinutes.innerHTML = "20";
        placeOfSeconds.innerHTML = "00";

        setInterval(function () { UpdateCounter(placeOfMinutes, placeOfSeconds); }, 1001);
    }
}

function buttonOfChangingDate_click() {
    if (confirm("Czy chcesz opuścić obecną stronę, aby zmienić miesiąc? Niezapisane dane nie zostaną zachowane."))
        window.location.href = "ChangeDate.aspx";
}

function UpdateCounter(placeOfMinutes, placeOfSeconds) {
    var seconds = parseInt(placeOfSeconds.innerHTML);
    var minutes = parseInt(placeOfMinutes.innerHTML);

    if (seconds == 0) {
        minutes--;

        if (minutes < 10)
            placeOfMinutes.innerHTML = "0" + minutes;
        else
            placeOfMinutes.innerHTML = minutes;

        placeOfSeconds.innerHTML = "59";
    }
    else {
        seconds--;

        if (seconds < 10)
            placeOfSeconds.innerHTML = "0" + seconds;
        else
            placeOfSeconds.innerHTML = seconds;
    }

    if (minutes <= 0 && seconds <= 0)
        window.location.href = "../Login.aspx?reason=NotLoggedInOrSessionExpired";
}

function ShowMenu(item) {
    item.style.display = "table";
}

function body_onclick(evt) {
    HideMenu();

    var srcElement;

    srcElement = evt.target;

    if (srcElement.className.indexOf("mainSuperMenuItem") != -1)
        ShowMenu(srcElement.children[0]);
    else
        if (srcElement.className.indexOf("mainSubMenuItem") != -1) {
            ShowMenu(srcElement.parentElement);
            ShowMenu(srcElement.children[0]);
        }
}

function HideMenu() {
    var subMenus = document.querySelectorAll(".mainSubMenu");

    for (var i = 0; i < subMenus.length; i++)
        subMenus[i].style.display = "none";

    var subSubMenus = document.querySelectorAll(".mainSubSubMenu");

    for (var i = 0; i < subSubMenus.length; i++)
        subSubMenus[i].style.display = "none";
}

function isInteger(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isFloat(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode != 44 && (charCode < 48 || charCode > 57)))
        return false;

    return true;
}

function isDate(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode != 45 && (charCode < 48 || charCode > 57)))
        return false;

    return true;
}

function ChangeRow(rowId, inactive) {
    var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");
    var movingButton = document.getElementById("moveaction");
    var editingTabButton = document.getElementById("showEditingWindow");
    var deletingTabButton = document.getElementById("deletechildAction");
    var browsingTabButton = document.getElementById("browsechildAction");

    if (!inactive) {
        if (editingButton != null)
            editingButton.disabled = false;

        if (deletingButton != null)
            deletingButton.disabled = false;

        if (editingTabButton != null)
            editingTabButton.disabled = false;

        if (deletingTabButton != null)
            deletingTabButton.disabled = false;
    }

    if (browsingButton != null)
        browsingButton.disabled = false;

    if (browsingTabButton != null)
        browsingTabButton.disabled = false;

    if (movingButton != null)
        movingButton.disabled = false;

    var selectedRow = document.getElementsByClassName("selectedRow")[0];

    if (selectedRow != null)
        selectedRow.className = "tableRow";

    var row = document.getElementById(rowId + "_row");

    if (row != null)
        row.className = "selectedRow";
}