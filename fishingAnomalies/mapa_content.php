<?php
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
?>
<div id="map-canvas" class="column"></div>
<div class="column">
    <form method="get" action="mapa.php">
        <fieldset>
            <legend>Liczba ostatnich zgłoszeń wyświetlanych na mapie</legend>
            <input type="radio" name="limit" id="1" value="1" /><label for="1">1</label><br />
            <input type="radio" name="limit" id="10" value="10" /><label for="10">10</label><br />
            <input type="radio" name="limit" id="100" value="100" /><label for="100">100</label><br />
            <input type="radio" name="limit" id="1000" value="1000" /><label for="1000">1000</label><br />
            <input type="radio" name="limit" id="other" value="other" /><label for="other">inne: </label><input type="text" name="otherLimit" id="otherLimit" size="5" /><br />
            <input type="submit" id="showXNotifications" name="showXNotifications" value="Wyświetl" />
        </fieldset>
    </form>
</div>

<script>
    checkProperLimit(<?php echo $limit; ?>);
</script>