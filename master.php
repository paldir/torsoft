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
        <link href="style.css" rel="stylesheet" type="text/css"/>
        <script src="skrypty.js"></script>

        <?php
        if (isset($head)) {
            include $head;
        }
        ?>

    </head>
    <body>
        <div class="menu">
            <a href="zgloszenia.php">Zgłoszenia</a>
            <a href="mapa.php">Mapa</a>
            <a href="pobierz.php">Pobierz</a>
        </div>
        <div class="content">

            <?php
            include $content;
            ?>

        </div>
    </body>
</html>
