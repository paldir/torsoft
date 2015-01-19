/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function wyczyscFiltr_click()
{
    var filters = document.getElementsByName("filtr[]");

    for (var i = 0; i < filters.length; i++)
        filters[i].checked = false;
}