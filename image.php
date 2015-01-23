<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$id = filter_input(INPUT_GET, "id");
$config = simplexml_load_file("config.xml");
$connection = new mysqli($config->host, $config->user, $config->password, $config->database);
$queryResult = $connection->query("SELECT dane FROM zdjecie WHERE id=" . $id);
$row = $queryResult->fetch_assoc();
$image = $row["dane"];

header('Content-Type: image/jpg');
echo $image;
