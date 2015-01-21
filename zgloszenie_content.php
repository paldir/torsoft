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
        ?>

        <table>

            <?php
            for ($i = 0; $i < 3; $i++) {
                ?>

                <tr>

                    <?php
                    echo "<td>" . $labels[$i] . "</td>";
                    echo "<td><input type='text' value='" . $row[$i] . "' disabled></td>";
                    ?>

                </tr>

                <?php
            }
            ?>

            <tr>
                <td>Opis:</td>
                <td>

                    <?php
                    echo "<textarea rows='4' cols='50' disabled>" . $row[3] . "</textarea>";
                    ?>

                </td>
            </tr>
        </table>

        <div id='map-canvas'></div>

        <?php
    }

    $queryResult->free();
}

$connection->close();
