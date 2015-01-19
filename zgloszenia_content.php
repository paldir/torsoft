<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$config = simplexml_load_file("config.xml");
$connection = new mysqli($config->host, $config->user, $config->password, $config->database);

$connection->set_charset("utf8");

if ($connection->errno) {
    echo 'Nie można połączyć się z serwerem bazy danych.';
} else {
    $whereStatement = "";
    $filterOptions = filter_input(INPUT_GET, "filtr", FILTER_DEFAULT, FILTER_REQUIRE_ARRAY);

    if (isset($filterOptions)) {
        $whereStatement = "WHERE idKategorii IN(";
        $count = count($filterOptions);

        for ($i = 0; $i < $count; $i++) {
            $whereStatement.=$filterOptions[$i];

            if ($i != $count - 1) {
                $whereStatement.=", ";
            }
        }

        $whereStatement.=")";
    }

    $queryResult = $connection->query("SELECT z.id, data, k.opis, z.opis FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id " . $whereStatement . " ORDER BY data DESC");

    if ($queryResult->num_rows > 0) {
        $headers = array("Identyfikator", "Data", "Kategoria", "Opis");

        echo "<form method='get' action='zgloszenie.php'>";
        echo "<input type='submit' id='przegladajZgloszenie' name='przegladajZgloszenie' value='Przeglądaj zgłoszenie' />";
        echo "<table>";
        echo "<tr>";

        foreach ($headers as $header) {
            echo "<th>" . $header . "</th>";
        }

        echo "</tr>";

        $fieldCount = $queryResult->field_count;

        for ($i = 0; $i < $queryResult->num_rows; $i++) {
            $queryResult->data_seek($i);

            $row = $queryResult->fetch_row();

            echo "<tr>";

            echo "<td>";
            echo "<input type='radio' name='id' id='" . $row[0] . "' value='" . $row[0] . "' /><label for='" . $row[0] . "'>" . $row[0] . "</label>";
            echo "</td>";

            for ($j = 1; $j < $fieldCount; $j++) {
                echo "<td><label for='" . $row[0] . "'>" . $row[$j] . "</label></td>";
            }

            echo "</tr>";
        }

        echo "</table>";
        echo "</form>";

        echo "<form method='get' action='zgloszenia.php'>";
        echo "<fieldset>";
        echo "<legend>Filtr</legend>";

        $queryResult = $connection->query("SELECT id, opis FROM kategoria");

        for ($i = 0; $i < $queryResult->num_rows; $i++) {
            $queryResult->data_seek($i);

            $row = $queryResult->fetch_assoc();
            $id = "filter" . $row["id"];
            $checked = "";

            if (isset($filterOptions)) {
                if (in_array($row["id"], $filterOptions)) {
                    $checked = "checked";
                }
            } else {
                $checked = "checked";
            }

            echo "<input type='checkbox' id='" . $id . "' name='filtr[]' value='" . $row["id"] . "' " . $checked . " /><label for='" . $id . "'>" . $row["opis"] . "</label><br />";
        }

        echo "<input type='button' id='wyczyscFiltr' Value='Wyczyść' onclick='wyczyscFiltr_click()' />";
        echo "<input type='submit' id='filtruj' name='filtruj' value='Filtruj' />";
        echo "</fieldset>";
        echo "</form>";
    } else {
        echo "<p>Brak zgłoszeń.</p>";
    }

    $queryResult->free();
}

$connection->close();
