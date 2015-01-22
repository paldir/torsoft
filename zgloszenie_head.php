<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$config = simplexml_load_file("config.xml");
$connection = new mysqli($config->host, $config->user, $config->password, $config->database);

$connection->set_charset("utf8");

$queryResult = $connection->query(mysql_escape_string("SELECT z.id, data, k.opis AS opisKategorii, z.opis, szerokosc, dlugosc FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id WHERE z.id=" . filter_input(INPUT_GET, "id")));
$row = $queryResult->fetch_assoc();
?>

<script src="https://maps.googleapis.com/maps/api/js?v=3.exp"></script>

<script>
    google.maps.event.addDomListener(window, 'load', function () {
        initializeMapWithOneMarker(<?php echo $row["szerokosc"]; ?>, <?php echo $row["dlugosc"]; ?>, "zgłoszenie");
    });
</script>