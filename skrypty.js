/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function initializeMapWithOneMarker(lat, lng, title) {
    var myLatlng = new google.maps.LatLng(lat, lng);
    var mapOptions = {
        zoom: 10,
        center: myLatlng
    };
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: title
    });
}

function initializeMapWithFewMarkers(positions, shortDescriptions, fullHtmlDescriptions) {
    var bounds = new google.maps.LatLngBounds();

    for (var i = 0; i < positions.length; i++)
        bounds.extend(positions[i]);

    var mapOptions = {
        zoom: 10,
        center: positions[0]
    };
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    var markers = [];
    var infoWindows = [];

    map.fitBounds(bounds);

    for (var i = 0; i < positions.length; i++)
    {
        var infoWindow = new google.maps.InfoWindow({content: "dummy"});
        var marker = new google.maps.Marker({
            position: positions[i],
            map: map,
            title: shortDescriptions[i],
            description: fullHtmlDescriptions[i]
        });

        google.maps.event.addListener(marker, 'click', function () {
            infoWindow.setContent(this.description);
            infoWindow.open(map, this);
        });

        markers.push(marker);
        infoWindows.push(infoWindow);
    }

    var markerCluster = new MarkerClusterer(map, markers);
}

function id_change(row)
{
    var button = document.getElementById("browseNotification");

    if (button !== null)
        button.disabled = false;

    var selectedRows = document.getElementsByClassName("selectedRow");

    for (var i = 0; i < selectedRows.length; i++)
        selectedRows[i].className = "rowOfTableOfReports";

    var newSelectedRow = document.getElementById("row" + row.id);

    if (newSelectedRow !== null)
        newSelectedRow.className = "selectedRow";
}

function clearFilter_click()
{
    var filters = document.getElementsByName("filter[]");

    for (var i = 0; i < filters.length; i++)
        filters[i].checked = false;
}

function checkProperLimit(id)
{
    var limit = document.getElementById(id);

    if (limit === null)
    {
        limit = document.getElementById("other");
        var other = document.getElementById("otherLimit");

        if (other !== null)
            other.value = id;
    }

    if (limit !== null)
        limit.checked = true;
}

function checkProperPerPage(id)
{
    var perPage = document.getElementById("perPage"+id);

    if (perPage !== null)
        perPage.checked = true;
}