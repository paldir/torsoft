<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$limit = filter_input(INPUT_GET, "limit");

if (isset($limit)) {
    if (!is_numeric($limit)) {
        $otherLimit = filter_input(INPUT_GET, "otherLimit");

        if (isset($otherLimit) && is_numeric($otherLimit)) {
            $limit = $otherLimit;
        } else {
            $limit = 10;
        }
    }
} else {
    $limit = 10;
}

$queryResult = $connection->query(mysqli_real_escape_string($connection, "SELECT z.id, data, k.opis AS opisKategorii, z.opis, szerokosc, dlugosc FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id ORDER BY data DESC LIMIT " . $limit));
?>

<script src="https://maps.googleapis.com/maps/api/js?v=3.exp"></script>
<script src="markerclusterer_compiled.js"></script>
<title>Mapa</title>

<script>
    var positions = [];
    var descriptions = [];
    var fullHtmlDescriptions = [];
<?php
for ($i = 0; $i < $queryResult->num_rows; $i++) {
    $queryResult->data_seek($i);

    $row = $queryResult->fetch_assoc();
    ?>

        positions[positions.length] = new google.maps.LatLng(<?php echo $row["szerokosc"]; ?>, <?php echo $row["dlugosc"]; ?>);
        descriptions[descriptions.length] = "<?php echo $row["data"] . " " . $row["opisKategorii"] ?>";
        fullHtmlDescriptions[fullHtmlDescriptions.length] = "<table><tr><td>Identyfikator: </td><td><?php echo $row["id"]; ?></td></tr><tr><td>Data: </td><td><?php echo $row["data"]; ?></td></tr><tr><td>Typ: </td><td><?php echo $row["opisKategorii"]; ?></td></tr><tr><td>Szerokość geograficzna: </td><td><?php echo $row["szerokosc"]; ?></td></tr><tr><td>Długość geograficzna: </td><td><?php echo $row["dlugosc"]; ?></td></tr><tr><td>Opis: </td><td><?php echo $row["opis"]; ?></td></tr></table><a href='zgloszenie.php?id=<?php echo $row["id"] ?>'>Strona zgłoszenia</a>";

    <?php
}
?>

    google.maps.event.addDomListener(window, 'load', function () {
        initializeMapWithFewMarkers(positions, descriptions, fullHtmlDescriptions);
    });
</script>

<?php
$queryResult->free();
