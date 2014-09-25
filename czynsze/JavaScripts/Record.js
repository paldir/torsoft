function Init() {
    var tabRadios = document.getElementsByClassName('tabRadio');

    if (tabRadios != null)
        for (var i = 0; i < tabRadios.length; i++)
            tabRadios[i].onchange = function () { tabRadio_onchange(this.value); }
}

function tabRadio_onchange(value) {
    var tabs = document.getElementsByClassName('tab');

    if (tabs != null)
        for (var i = 0; i < tabs.length; i++)
            if (tabs[i].id.indexOf(value) != -1)
                tabs[i].hidden = false;
            else
                tabs[i].hidden = true;
}

function InitPlace() {
    var kod_lok = document.getElementById("kod_lok");
    var nr_lok = document.getElementById("nr_lok");
    var adres = document.getElementById("adres");
    var adres_2 = document.getElementById("adres_2");

    if (kod_lok != null)
        kod_lok.onchange = function () { preview_onchange(this.id, this.value); };

    if (nr_lok != null)
        nr_lok.onchange = function () { preview_onchange(this.id, this.value); };

    if (adres != null)
        adres.onchange = function () { preview_onchange(this.id, this.value); };

    if (adres_2 != null)
        adres_2.onchange = function () { preview_onchange(this.id, this.value); };
}

function preview_onchange(id, value) {
    var preview = document.getElementById(id + "_preview");

    if (preview != null)
        preview.innerText = value;
}

function InitRentComponent() {
    var fields = document.getElementsByTagName("input");
    var s_zaplat = document.getElementById("s_zaplat");

    if (s_zaplat.value != "6")
        for (var i = 0; i < fields.length; i++)
            if (fields[i].id.indexOf('stawka_0') != -1)
                fields[i].disabled = true;

    s_zaplat.onchange = function () { s_zaplat_onchange(this.value) };
}

function s_zaplat_onchange(value) {
    var fields = document.getElementsByTagName("input");

    if (value == '6') {
        for (var i = 0; i < fields.length; i++)
            if (fields[i].id.indexOf('stawka_0') != -1)
                fields[i].disabled = false;
    }
    else
        for (var i = 0; i < fields.length; i++)
            if (fields[i].id.indexOf('stawka_0') != -1)
                fields[i].disabled = true;
}

function InitAttribute() {
    var radios = document.querySelectorAll("input[type='radio']");

    for (var i = 0; i < radios.length; i++) {
        radios[i].onchange = function () { nr_str_onchange(this.value, true) };

        if (radios[i].checked)
            nr_str_onchange(radios[i].value, false);
    }
}

function nr_str_onchange(value, radioAsSender) {
    var jedn = document.getElementById("jedn");
    var wartosc = document.getElementById("wartosc");

    if (jedn != null && wartosc != null) {
        switch (value) {
            case "N":
                if (!wartosc.disabled)
                    jedn.disabled = false;

                wartosc.maxLength = wartosc.size = 16;
                break;
            case "C":
                jedn.disabled = true;
                wartosc.maxLength = wartosc.size = 25;
                break;
        }

        if (radioAsSender)
            wartosc.value = "";
    }
}