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
    $filterOptions = filter_input(INPUT_GET, "filter", FILTER_DEFAULT, FILTER_REQUIRE_ARRAY);
    $limitStatement = "";
    $perPageOptions = filter_input(INPUT_GET, "perPage");

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

    if (!isset($perPageOptions)) {
        $perPageOptions = 10;
    }

    if (is_numeric($perPageOptions)) {
        $limitStatement = "LIMIT " . $perPageOptions;
    }

    $queryResult = $connection->query(mysql_escape_string("SELECT z.id, data, k.opis, z.opis FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id " . $whereStatement . " ORDER BY data DESC " . $limitStatement));

    if ($queryResult->num_rows > 0) {
        $headers = array("Identyfikator", "Data", "Kategoria", "Opis");
        ?>

        <div class="column">
            <form method='get' action='zgloszenie.php'>
                <input type='submit' id='browseNotification' class="browseNotification" name='browseNotification' value='Przeglądaj zgłoszenie' disabled />
                <table class="tableOfReports">
                    <tr>

                        <?php
                        foreach ($headers as $header) {
                            ?>

                            <th><?php echo $header; ?></th>

                            <?php
                        }
                        ?>

                    </tr>                

                    <?php
                    $fieldCount = $queryResult->field_count;

                    for ($i = 0; $i < $queryResult->num_rows; $i++) {
                        $queryResult->data_seek($i);

                        $row = $queryResult->fetch_row();
                        ?>

                        <tr id="row<?php echo $row[0]; ?>" class="rowOfTableOfReports">
                            <td class="numericalInput">
                                <input type='radio' name='id' id='<?php echo $row[0]; ?>' value='<?php echo $row[0]; ?>' onchange='id_change(this);' /><label for='<?php echo $row[0]; ?>'><?php echo $row[0]; ?></label>
                            </td>

                            <?php
                            for ($j = 1; $j < $fieldCount; $j++) {
                                ?>

                                <td>
                                    <label for='<?php echo $row[0]; ?>'><?php echo $row[$j]; ?></label>
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
            <input type="button" value="Poprzednia" /><input type="button" value="Następna" />
        </div>
        <div class="column">
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
                        ?>

                        <input type='checkbox' id='<?php echo $id; ?>' name='filter[]' value='<?php echo $row["id"]; ?>' <?php echo $checked; ?> /><label for='<?php echo $id; ?>'><?php echo $row["opis"] ?></label><br />

                        <?php
                    }
                    ?>

                    <input type='button' id='clearFilter' Value='Wyczyść' onclick='clearFilter_click()' />
                    <input type='submit' id='filterButton' name='filterButton' value='Filtruj' />
                </fieldset>
                <fieldset>
                    <legend>Zgłoszeń na stronę</legend>
                    <input type="radio" id="perPage10" name="perPage" value="10" /><label for="perPage10">10</label><br />
                    <input type="radio" id="perPage50" name="perPage" value="50" /><label for="perPage50">50</label><br />
                    <input type="radio" id="perPage100" name="perPage" value="100" /><label for="perPage100">100</label><br />
                    <input type="radio" id="perPageAll" name="perPage" value="All" /><label for="perPageAll">wszystkie</label><br />
                    <input type="submit" name="split" value="Zatwierdź" />
                </fieldset>

                <script>
                    checkProperPerPage("<?php echo $perPageOptions ?>");
                </script>

            </form>
        </div>

        <?php
    } else {
        echo "<p>Brak zgłoszeń.</p>";
    }

    $queryResult->free();
}

$connection->close();
