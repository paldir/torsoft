function MainInit() {
    var buttonOfChangingDate = document.getElementById("buttonOfChangingDate");

    if (buttonOfChangingDate != null) {
        buttonOfChangingDate.onclick = function () { buttonOfPanelItem_click("ChangeDate.aspx"); }
    }

    var placeOfMinutes = document.getElementById("placeOfMinutes");
    var placeOfSeconds = document.getElementById("placeOfSeconds");

    if (placeOfMinutes != null && placeOfSeconds != null) {
        placeOfMinutes.innerHTML = "20";
        placeOfSeconds.innerHTML = "00";

        setInterval(function () { UpdateCounter(placeOfMinutes, placeOfSeconds); }, 1001);
    }

    var buttonOfSetChanging = document.getElementById("buttonOfSetChanging");

    if (buttonOfSetChanging != null)
        buttonOfSetChanging.onclick = function () { buttonOfPanelItem_click("ChangeSettlementTable.aspx"); }
}

function buttonOfPanelItem_click(href) {
    if (confirm("Czy chcesz opuścić obecną stronę? Niezapisane dane nie zostaną zachowane."))
        window.location.href = href;
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

function ChangeRow(rowId, inactive, buttons, subMenu) {
    /*var editingButton = document.getElementById("editaction");
    var deletingButton = document.getElementById("deleteaction");
    var browsingButton = document.getElementById("browseaction");
    var movingButton = document.getElementById("moveaction");
    var editingTabButton = document.getElementById("showEditingWindow");
    var deletingTabButton = document.getElementById("deletechildAction");
    var browsingTabButton = document.getElementById("browsechildAction");
    var saldoButton = document.getElementById("saldo");*/

    /*if (!inactive) {
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
        movingButton.disabled = false;*/

    for (var i = 0; i < buttons.length; i++)
        if (buttons[i] != null)
            buttons[i].disabled = false;

    for (var i = 0; i < subMenu.length; i++)
        subMenu[i].className = "superMenu";

    var selectedRow = document.getElementsByClassName("selectedRow")[0];

    if (selectedRow != null)
        selectedRow.className = "tableRow";

    var row = document.getElementById(rowId + "_row");

    if (row != null)
        row.className = "selectedRow";
}

function Load(href) {
    window.location.href = "Loading.aspx?href=*" + href + "*";
}

function FinishLoading() {
    var waterMark = document.getElementById("waterMark");

    if (waterMark != null) {
        var animationSupported = false;
        var prefixesOfBrowsers = 'Webkit Moz O ms Khtml'.split(' ');

        if (waterMark.style.animationName != undefined)
            animationSupported = true;

        if (!animationSupported)
            for (var i = 0; i < prefixesOfBrowsers.length; i++)
                if (waterMark.style[prefixesOfBrowsers[i] + 'AnimationName'] != undefined) {
                    animationSupported = true;

                    break;
                }

        animationSupported = false;

        if (animationSupported)
            waterMark.className = "animatedWaterMark";
        else {
            var loading = document.getElementById("loading");

            if (loading != null)
                loading.style.display = "block";
        }
    }

    var href = window.location.search;

    if (href.length > 7)
        window.location.href = href.substring(7, href.length - 1);
}