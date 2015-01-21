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
        ?>

        <form method='get' action='zgloszenie.php'>
            <input type='submit' id='przegladajZgloszenie' name='przegladajZgloszenie' value='Przeglądaj zgłoszenie' />
            <table>
                <tr>

                    <?php
                    foreach ($headers as $header) {
                        echo "<th>" . $header . "</th>";
                    }
                    ?>

                </tr>                

                <?php
                $fieldCount = $queryResult->field_count;

                for ($i = 0; $i < $queryResult->num_rows; $i++) {
                    $queryResult->data_seek($i);

                    $row = $queryResult->fetch_row();
                    ?>

                    <tr>
                        <td>

                            <?php
                            echo "<input type='radio' name='id' id='" . $row[0] . "' value='" . $row[0] . "' /><label for='" . $row[0] . "'>" . $row[0] . "</label>";
                            ?>

                        </td>

                        <?php
                        for ($j = 1; $j < $fieldCount; $j++) {
                            ?>
                            <td>

                                <?php
                                echo "<label for='" . $row[0] . "'>" . $row[$j] . "</label>";
                                ?>

                            </td>
                            <?php
                        }
                        ?>

                    </tr>

                    <?php
                }
                ?>

            </table>
        </form>

        <form method='get' action='zgloszenia.php'>
            <fieldset>
                <legend>Filtr</legend>

                <?php
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
                ?>

                <input type='button' id='wyczyscFiltr' Value='Wyczyść' onclick='wyczyscFiltr_click()' />
                <input type='submit' id='filtruj' name='filtruj' value='Filtruj' />
            </fieldset>
        </form>

        <?php
    } else {
        echo "<p>Brak zgłoszeń.</p>";
    }

    $queryResult->free();
}

$connection->close();
