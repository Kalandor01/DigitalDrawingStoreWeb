'use strict';

$(document).ready(() => {
    $("#menu").click(() => toggleMenuElements("#xpericad-dropdown-menu"));
    $("#langMenu").click(() => toggleMenuElements("#language-dropdown-menu"));

    $(".changeLang").on("click", changeLang);
});

let toggleMenuElements = (parrent) => {
    document.querySelectorAll(parrent)[0].classList.toggle('open');
    document.querySelectorAll(parrent + ' .bar-container')[0].classList.toggle('change');
    document.querySelectorAll(parrent + ' .dropdown')[0].classList.toggle('down');
    if (document.querySelectorAll(parrent + ' .dropdown')[0].classList.contains('down'))
    {
        setTimeout(function ()
        {
            document.querySelectorAll(parrent + ' .dropdown')[0].style.overflow = 'visible';
        }, 500)
    }
    else
    {
        document.querySelectorAll(parrent + ' .dropdown')[0].style.overflow = 'hidden';
    }
}

function changeLang(evt) {
    evt.preventDefault();

    let languageCode = (evt.target).getAttribute("value");

    requestInvoker
        .executeUpdate('/ChangeLanguage', { languageString: languageCode })
        .then((response) => {
            location.reload(); 
        });
}