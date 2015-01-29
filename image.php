<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

include "config.php";

$connection = new mysqli($host, $user, $password, $database);
$id = filter_input(INPUT_GET, "id");
$queryResult = $connection->query("SELECT dane FROM zdjecie WHERE id=" . $id);
$row = $queryResult->fetch_assoc();
$image = $row["dane"];

header('Content-Type: image/jpg');
echo $image;
$queryResult->free();
$connection->close();
