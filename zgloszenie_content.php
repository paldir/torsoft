<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

if (!$connection->errno) {
    if ($queryResult->num_rows == 1) {
        ?>

        <table>
            <tr>
                <td>Identyfikator: </td>
                <td><input type="text" class="numericalInput" size="10" value="<?php echo $row["id"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Data: </td>
                <td><input type="text" size="19" value="<?php echo $row["data"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Typ: </td>
                <td><input type="text" value="<?php echo $row["opisKategorii"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Szerokość geograficzna: </td>
                <td><input type="text" class="numericalInput" size="9" value="<?php echo $row["szerokosc"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Długość geograficzna: </td>
                <td><input type="text" class="numericalInput" size="9" value="<?php echo $row["dlugosc"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Opis:</td>
                <td>
                    <textarea rows='3' cols='50' disabled><?php echo $row["opis"]; ?></textarea>
                </td>
            </tr>
        </table>
        <div class="column" id='map-canvas'></div>
        <div class="column">

            <?php
            $queryResult = $connection->query("SELECT id, dane FROM zdjecie WHERE idZgloszenia=" . $row["id"]);

            for ($i = 0; $i < $queryResult->num_rows; $i++) {
                $queryResult->data_seek($i);

                $row = $queryResult->fetch_assoc();
                $src = "image.php?id=" . $row["id"];
                ?>

                <div class="imageContainer column"><a href="<?php echo $src; ?>" target="_blank"><img class="miniature" src="<?php echo $src; ?>" /></a></div>

                <?php
                if ($i % 2 != 0) {
                    ?>

                    <br />

                    <?php
                }
            }
            ?>

        </div>

        <?php
    }

    $queryResult->free();
}