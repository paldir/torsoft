<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->
<html>
    <head>
        <meta charset="UTF-8">
        <title>Galeria</title>
        <link href="style.css" rel="stylesheet" type="text/css"/>
    </head>
    <body>
        <table class="gallery">
            <tr>
                <?php
                include 'config.php';

                $id = filter_input(INPUT_GET, "id");
                $parentId = filter_input(INPUT_GET, "parentId");
                $connection = new mysqli($host, $user, $password, $database);
                $previousResult = $connection->query("SELECT id FROM zdjecie WHERE idZgloszenia=" . $parentId . " AND id<" . $id . " ORDER BY id DESC LIMIT 1");
                $previous = $previousResult->fetch_assoc();
                $nextResult = $connection->query("SELECT id FROM zdjecie WHERE idZgloszenia=" . $parentId . " AND id>" . $id . " ORDER BY id LIMIT 1");
                $next = $nextResult->fetch_assoc();

                if (isset($previous)) {
                    ?>

                    <td>            
                        <form method="get" action="gallery.php">
                            <input type="hidden" name="id" value="<?php echo $previous["id"]; ?>" />
                            <input type="hidden" name="parentId" value="<?php echo $parentId; ?>" />
                            <input type="submit" name="previousImage" value="<<<" />
                        </form>
                    </td>

                    <?php
                }
                ?>

                <td><img src="image.php?id=<?php echo $id; ?>" /></td>

                <?php
                if (isset($next)) {
                    ?>

                    <td>            
                        <form method="get" action="gallery.php">
                            <input type="hidden" name="id" value="<?php echo $next["id"]; ?>" />
                            <input type="hidden" name="parentId" value="<?php echo $parentId; ?>" />
                            <input type="submit" name="nextImage" value=">>>" />
                        </form>
                    </td>

                    <?php
                }
                $connection->close();
                ?>

            </tr>
        </table>
    </body>
</html>
