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

function InitBuilding() {
    var kod_1 = document.getElementById("id");
    var adres = document.getElementById("adres");
    var adres_2 = document.getElementById("adres_2");

    SetPreview([kod_1, adres, adres_2]);
}

function InitTenant() {
    var nr_kontr = document.getElementById("id");
    var nazwisko = document.getElementById("nazwisko");
    var imie = document.getElementById("imie");
    var adres_1 = document.getElementById("adres_1");
    var adres_2 = document.getElementById("adres_2");

    SetPreview([nr_kontr, nazwisko, imie, adres_1, adres_2]);
}

function InitPlace() {
    var kod_lok = document.getElementById("kod_lok");
    var nr_lok = document.getElementById("nr_lok");
    var adres = document.getElementById("adres");
    var adres_2 = document.getElementById("adres_2");

    SetPreview([kod_lok, nr_lok, adres, adres_2]);
}

function InitCommunity() {
    var kod = document.getElementById("id");
    var nazwa_skr = document.getElementById("nazwa_skr");
    var il_bud = document.getElementById("il_bud");
    var il_miesz = document.getElementById("il_miesz");

    SetPreview([kod, nazwa_skr, il_bud, il_miesz]);
}

function SetPreview(inputs) {
    for (var i = 0; i < inputs.length; i++)
        if (inputs[i] != null)
            inputs[i].onchange = function () { preview_onchange(this.id, this.value); };
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

function InitUser() {
    var nazwisko = document.getElementById("nazwisko");
    var imie = document.getElementById("imie");

    if (nazwisko != null && imie != null)
        nazwisko.onchange = imie.onchange = function () { nazwiskoImie_onchange(); }

    nazwiskoImie_onchange();
}

function nazwiskoImie_onchange() {
    var nazwisko = document.getElementById("nazwisko");
    var imie = document.getElementById("imie");

    if (nazwisko != null && imie != null) {
        var uzytkownik = document.getElementById("uzytkownik");

        if (uzytkownik != null) {
            if (nazwisko.value.length > 0)
                uzytkownik.value = nazwisko.value;

            if (imie.value.length > 0)
                uzytkownik.value += " " + imie.value;
        }
    }
}