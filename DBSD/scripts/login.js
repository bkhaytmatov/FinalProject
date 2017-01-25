/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

function validation() {

    var errormessage = "";

    if (document.getElementById('username').value == "") {
        errormessage += "please enter the username \n";
        document.getElementById("username").style.borderColor = "red";

    }
    if (document.getElementById('password').value == "") {
        errormessage += "Enter the password \n";
        document.getElementById("password").style.borderColor = "red";

    }
    if (errormessage != "") {
        alert(errormessage);
        return false;

    }

}
function search() {
    var txt = document.getElementById("srch").value;

    if (txt != null) {
        window.location.href = 'search.jsp?p=' + txt;
    }
}

$(document).ready(function () {
    $('#logo').addClass('animated fadeInDown');
    $("input:text:visible:first").focus();
});
$('#username').focus(function () {
    $('label[for="username"]').addClass('selected');
});
$('#username').blur(function () {
    $('label[for="username"]').removeClass('selected');
});
$('#password').focus(function () {
    $('label[for="password"]').addClass('selected');
});
$('#password').blur(function () {
    $('label[for="password"]').removeClass('selected');
});



