<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$config = simplexml_load_file("config.xml");
$connection = new mysqli($config->host, $config->user, $config->password, $config->database);

$connection->set_charset("utf8");

if (!$connection->errno) {
    $queryResult = $connection->query("SELECT z.id, data, k.opis, z.opis FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id WHERE z.id=" . filter_input(INPUT_GET, "id"));

    if ($queryResult->num_rows == 1) {
        $row = $queryResult->fetch_row();
        $labels = array("Identyfikator: ", "Data: ", "Kategoria: ");

        echo "<table>";

        for ($i = 0; $i < 3; $i++) {
            echo "<tr>";
            echo "<td>" . $labels[$i] . "</td>";
            echo "<td><input type='text' value='" . $row[$i] . "' disabled></td>";
            echo "</tr>";
        }

        echo "<tr>";
        echo "<td>Opis:</td>";
        echo "<td><textarea rows='4' cols='50' disabled>" . $row[3] . "</textarea></td>";
        echo "</tr>";
        echo "</table>";
    }
    
    $queryResult->free();
}

$connection->close();