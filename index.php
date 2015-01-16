<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <meta charset="UTF-8">
        <title></title>
    </head>
    <body>
        <?php
        $connection = new mysqli("localhost", "angler", "angler", "fish");

        $connection->set_charset("utf8");

        if ($connection->errno) {
            echo 'Nie można połączyć się z serwerem bazy danych.';
        } else {
            $queryResult = $connection->query("SELECT * FROM zgloszenie");

            echo "<table>";

            if ($queryResult->num_rows > 0) {
                $fieldCount = $queryResult->field_count;

                for ($i = 0; $i < $queryResult->num_rows; $i++) {
                    $queryResult->data_seek($i);

                    $row = $queryResult->fetch_row();

                    echo "<tr>";

                    for ($j = 0; $j < $fieldCount; $j++) {
                        echo "<td>";
                        echo $row[$j];
                        echo "</td>";
                    }

                    echo "</tr>";
                }
            } else {
                echo "Brak zgłoszeń.";
            }

            echo "</table>";

            $queryResult->free();
        }

        $connection->close();
        ?>
    </body>
</html>
