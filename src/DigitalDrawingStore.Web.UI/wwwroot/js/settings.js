var passRevealed = false;
var appConfigFormChanged = false;
var feedbackFormChanged = false;

var changesNotSavedText = "[Esetleges változtatások mentés nélkül elveszhetnek, folytatja?]";

$(document).ready(function () {

    $('#togglePassword').on("click", togglePasswordHidden);

    $(window).on("beforeunload", unsavedChangesPopup);

    requestInvoker
        .executeQuery('/GetNotSavedText', {})
        .then((response) => {
            let translation = response.responseObject;
            if (translation != null) {
                changesNotSavedText = translation;
            }
    });

    startCheckingAppConfigMod();
    startCheckingFeedbackMod();
});

function unsavedChangesPopup()
{
    if (appConfigFormChanged || feedbackFormChanged)
    {
        return confirm(changesNotSavedText);
    }
}

function togglePasswordHidden(evt)
{
    $(evt.target).toggleClass("fa-eye fa-eye-slash");

    if (passRevealed)
    {
        ($("#smtpPass")[0]).type = "password";
    }
    else
    {
        ($("#smtpPass")[0]).type = "text";
    }
    passRevealed = !passRevealed;
}

function modAppConfigForm()
{
    appConfigFormChanged = true;
    $("#appConfig *").off("input");
}

function modFeedbackForm()
{
    feedbackFormChanged = true;
    $("#feedback *").off("input");
}

function startCheckingAppConfigMod()
{
    appConfigFormChanged = false;
    $("#appConfig *").on("input", modAppConfigForm);
}

function startCheckingFeedbackMod()
{
    feedbackFormChanged = false;
    $("#feedback *").on("input", modFeedbackForm);
}

function getFormValues(formDivName)
{

    let configParamatersObject = {}
    $(`${formDivName}>.setting input, ${formDivName}>.setting textarea`).each(function () {
        let name = this.name;
        let value = this.value;

        configParamatersObject[name] = value;
    });

    $(`${formDivName}>.settingChb input`).each(function () {
        let name = this.name;
        let value = this.checked;

        configParamatersObject[name] = value;
    });

    return configParamatersObject;
}

function updateConfigSettings()
{
    let configParamatersObject = getFormValues("#appConfig");
    requestInvoker
        .executeUpdate('/Administration/UpdateApplicationConfigurationSettings', configParamatersObject);
    startCheckingAppConfigMod();
}

function updateEmailSettings()
{
    let configParamatersObject = getFormValues("#feedback");
    requestInvoker
        .executeUpdate('/Administration/UpdateFeedbackEmailSettings', configParamatersObject);
    startCheckingFeedbackMod();
}


