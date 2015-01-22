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
                <td><input type="text" value="<?php echo $row["szerokosc"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Długość geograficzna: </td>
                <td><input type="text" value="<?php echo $row["dlugosc"]; ?>" disabled /></td>
            </tr>
            <tr>
                <td>Opis:</td>
                <td>
                    <textarea rows='4' cols='50' disabled><?php echo $row["opis"]; ?></textarea>;
                </td>
            </tr>
        </table>

        <div id='map-canvas'></div>

        <?php
    }

    $queryResult->free();
}

$connection->close();
