<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$config = simplexml_load_file("config.xml");
$connection = new mysqli($config->host, $config->user, $config->password, $config->database);
$limit = filter_input(INPUT_GET, "limit");

if (isset($limit)) {
    if (!is_numeric($limit)) {
        $limit = filter_input(INPUT_GET, "otherLimit");
    }
} else {
    $limit = 10;
}

$connection->set_charset("utf8");

$queryResult = $connection->query(mysql_escape_string("SELECT z.id, data, k.opis, szerokosc, dlugosc FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id ORDER BY data DESC LIMIT " . $limit));
?>

<script src="https://maps.googleapis.com/maps/api/js?v=3.exp"></script>

<script>
    var positions = [];
    var descriptions = [];
<?php
for ($i = 0; $i < $queryResult->num_rows; $i++) {
    $queryResult->data_seek($i);

    $row = $queryResult->fetch_assoc();
    ?>
        positions[positions.length] = new google.maps.LatLng(<?php echo $row["szerokosc"]; ?>, <?php echo $row["dlugosc"]; ?>);
        descriptions[descriptions.length] = "<?php echo $row["data"] . " " . $row["opis"] ?>";
    <?php
}
?>

    google.maps.event.addDomListener(window, 'load', function () {
        initializeMapWithFewMarkers(positions, descriptions);
    });
</script>

<?php
$queryResult->free();
$connection->close();
