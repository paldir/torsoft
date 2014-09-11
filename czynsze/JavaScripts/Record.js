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

function InitPlaces(kod_lok_id, nr_lok_id, adres_id, adres_2_id) {
    var kod_lok = document.getElementById(kod_lok_id);
    var nr_lok = document.getElementById(nr_lok_id);
    var adres = document.getElementById(adres_id);
    var adres_2 = document.getElementById(adres_2_id);

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

function InitRentComponent(s_zaplat_name) {
    var fields = document.getElementsByTagName("input");
    var s_zaplat = document.getElementById(s_zaplat_name);

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