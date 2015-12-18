<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

if ($connection->errno) {
    echo 'Nie można połączyć się z serwerem bazy danych.';
} else {
    $whereStatement = "";
    $filterOptions = filter_input(INPUT_GET, "filter", FILTER_DEFAULT, FILTER_REQUIRE_ARRAY);
    $limitStatement = "";
    $perPageOptions = filter_input(INPUT_GET, "perPage");
    $page = filter_input(INPUT_GET, "page");

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

    $resultOfCountOfRows = $connection->query(mysqli_real_escape_string($connection, "SELECT COUNT(*) FROM zgloszenie " . $whereStatement));
    $rowOfCountOfRows = $resultOfCountOfRows->fetch_row();
    $countOfRows = $rowOfCountOfRows[0];

    if (!isset($perPageOptions)) {
        $perPageOptions = 10;
    }

    if (!isset($page)) {
        $page = 1;
    }

    if (is_numeric($perPageOptions)) {
        $countOfPages = ceil($countOfRows / $perPageOptions);
    } else {
        $countOfPages = 1;
    }

    if ($countOfPages > 0 && $page > $countOfPages) {
        $page = $countOfPages;
    }

    if (is_numeric($perPageOptions)) {
        $limitStatement = "LIMIT " . ($perPageOptions * ($page - 1)) . ", " . $perPageOptions;
    }

    $queryResult = $connection->query("SELECT z.id, data, k.opis, CASE WHEN LENGTH(z.opis)>50 THEN CONCAT(LEFT(z.opis, 50), '...') ELSE z.opis END FROM zgloszenie z JOIN kategoria k ON z.idKategorii=k.id " . mysqli_real_escape_string($connection, $whereStatement) . " ORDER BY data DESC " . mysqli_real_escape_string($connection, $limitStatement));
    ?>

    <div class="column">

        <?php
        if ($queryResult->num_rows > 0) {
            $headers = array("Identyfikator", "Data", "Kategoria", "Opis");
            ?>

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

            <?php
            $previous = "";
            $next = "";

            if ($page == 1) {
                $previous = "disabled";
            }

            if (!is_numeric($perPageOptions) || $queryResult->num_rows < $perPageOptions || $page * $perPageOptions == $countOfRows) {
                $next = "disabled";
            }
            ?>

            <input type="hidden" id="countOfPages" value="<?php echo $countOfPages; ?>" />
            <div class="pageSelectors"><input type="button" value="<<<" <?php echo $previous; ?> onclick="changePage_click(-2);" /><input type="button" value="<" <?php echo $previous; ?> onclick="changePage_click(-1);" /><span>Strona <?php echo $page; ?> z <?php echo $countOfPages; ?></span><input type="button" value=">" <?php echo $next; ?> onclick="changePage_click(1);" /><input type="button" value=">>>" <?php echo $next; ?> onclick="changePage_click(2);" /></div>

            <?php
        } else {
            ?>

            <p>Brak zgłoszeń.</p>

            <?php
        }
        ?>

    </div>
    <div class="column">
        <form method='get' action='zgloszenia.php'>
            <fieldset>
                <legend>Filtr</legend>

                <?php
                $categoryResult = $connection->query("SELECT id, opis FROM kategoria");

                for ($i = 0; $i < $categoryResult->num_rows; $i++) {
                    $categoryResult->data_seek($i);

                    $row = $categoryResult->fetch_assoc();
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
                checkProperPerPage("<?php echo $perPageOptions; ?>");
            </script>

            <input type="hidden" id="page" name="page" value="<?php echo $page; ?>" />
        </form>
    </div>

    <?php
    $queryResult->free();
}