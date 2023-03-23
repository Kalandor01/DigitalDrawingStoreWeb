var passRevealed = false;
var appConfigFormChanged = false;
var feedbackFormChanged = false;

$(document).ready(function () {

    const togglePassword = $('#togglePassword');

    togglePassword.on("click", (e) => {
        togglePasswordHidden(e)
        togglePassword.toggleClass("fa-eye fa-eye-slash");
    });


    $("#passReveal").on("click", togglePasswordHidden);
    $("form").on("submit", clickFormSubmit);

     $(window).on("beforeunload", unsavedChangesPopup);

    startCheckingAppConfigMod();
    startCheckingFeedbackMod();
});

function unsavedChangesPopup()
{
    if (appConfigFormChanged || feedbackFormChanged)
    {
        return confirm('Esetleges változtatások mentés nélkül elveszhetnek, folytatja?');
    }
}

function togglePasswordHidden(evt)
{
    evt.preventDefault();

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

function clickFormSubmit(evt)
{
    evt.preventDefault();

    let eventForm = $(evt.target);
    if (eventForm.children("#appConfig").length == 1)
    {
        let values = getFormValues("#appConfig");
        updateConfigSettings(values);
    }
    else
    {
        let values = getFormValues("#feedback");
        updateEmailSettings(values);
    }
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

function updateConfigSettings(configParamatersObject)
{
    requestInvoker
        .executeUpdate('/Administration/UpdateApplicationConfigurationSettings', configParamatersObject);
    startCheckingAppConfigMod();
}

function updateEmailSettings(configParamatersObject)
{
    requestInvoker
        .executeUpdate('/Administration/UpdateFeedbackEmailSettings', configParamatersObject);
    startCheckingFeedbackMod();
}


