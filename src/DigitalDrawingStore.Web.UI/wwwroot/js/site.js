'use strict';

$(document).ready(() => {
    $("#menu").click(() => toggleMenuElements());
});

let toggleMenuElements = () => {
    document.getElementsByClassName('menu')[0].classList.toggle('open');
    document.getElementsByClassName('bar-container')[0].classList.toggle('change');
    document.getElementsByClassName('dropdown')[0].classList.toggle('down');
    if (document.getElementsByClassName('dropdown')[0].classList.contains('down')) {
        setTimeout(function () {
            document.getElementsByClassName('dropdown')[0].style.overflow = 'visible'
        }, 500)
    } else {
        document.getElementsByClassName('dropdown')[0].style.overflow = 'hidden'
    }
}