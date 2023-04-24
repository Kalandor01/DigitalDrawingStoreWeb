let coll = document.querySelector(".editor-menu-toggle");

var openEditorText = "";
var closeEditorText = "";

requestInvoker
    .executeQuery('/GetOpenEditorText', {})
    .then((response) => {
        if (response.responseObject != null) {
            openEditorText = response.responseObject;
        }
        else {
            openEditorText = "[Szerkesztő megnyitása.]";
        }
    });

requestInvoker
    .executeQuery('/GetCloseEditorText', {})
    .then((response) => {
        if (response.responseObject != null) {
            closeEditorText = response.responseObject;
        }
        else {
            closeEditorText = "[Szerkesztő bezárása.]";
        }
    });

$('button').removeClass("ui-button ui-corner-all ui-widget");

coll.addEventListener("click", function () {
    this.classList.toggle("active");
    let content = this.nextElementSibling;
    if (content.style.maxHeight) {
        content.style.maxHeight = null;
        coll.innerText = openEditorText;
    } else {
        content.style.maxHeight = content.scrollHeight + "px";
        coll.innerText = closeEditorText;
    }
});