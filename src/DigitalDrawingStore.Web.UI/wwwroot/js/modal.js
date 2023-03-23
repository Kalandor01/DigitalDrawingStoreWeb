let coll = document.querySelector(".editor-menu-toggle");

$('button').removeClass("ui-button ui-corner-all ui-widget");

coll.addEventListener("click", function () {
    this.classList.toggle("active");
    let content = this.nextElementSibling;
    if (content.style.maxHeight) {
        content.style.maxHeight = null;
        coll.innerText = "Szerkesztő megnyitása"
    } else {
        content.style.maxHeight = content.scrollHeight + "px";
        coll.innerText = "Szerkesztő bezárása"
    }
});