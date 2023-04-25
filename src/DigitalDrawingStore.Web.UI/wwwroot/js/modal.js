let coll = document.querySelector(".editor-menu-toggle");

var openEditorText = "";
var closeEditorText = "";

requestInvoker
    .executeQuery('/GetEditorTexts', {})
    .then((response) => {
        let translationDict = response.responseObject;
        if (translationDict["open"] != null) {
            openEditorText = translationDict["open"];
        }
        else {
            openEditorText = "[Szerkesztő megnyitása.]";
        }
        if (translationDict["close"] != null) {
            closeEditorText = translationDict["close"];
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