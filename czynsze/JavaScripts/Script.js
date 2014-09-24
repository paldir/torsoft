function ShowMenu(item) {
    item.style.display = "table";
}

function body_onclick(evt) {
    HideMenu();

    srcElement = evt.srcElement;

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
