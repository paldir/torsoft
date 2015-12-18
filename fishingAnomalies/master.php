<!DOCTYPE html>
<!--
To change this license header, choose License Headers in Project Properties.
To change this template file, choose Tools | Templates
and open the template in the editor.
-->

<?php
error_reporting(E_ALL);
include "config.php";

$connection = new mysqli($host, $user, $password, $database);

$connection->set_charset("utf8");
?>

<html>
    <head>
        <meta charset="UTF-8">
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
            <a href="zgloszenia.php">ZGŁOSZENIA</a>
            <a href="mapa.php">MAPA</a>
            <a href="pobierz.php">POBIERZ</a>
        </div>
        <div class="content">

            <?php
            include $content;
            ?>

        </div>
    </body>
</html>

<?php
$connection->close();