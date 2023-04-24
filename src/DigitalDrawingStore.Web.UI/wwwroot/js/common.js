'use strict';

var emptyCategoryText = "";

var topLeftWatermarkText = null;
var topRightWatermarkText = null;
var borromLeftWatermarkText = null;
var bottomRightWatermarkText = null;

$(document).ready(() => {
    requestInvoker
        .executeQuery('/GetEmptyCategoryText', {})
        .then((response) => {
            if (response.responseObject != null)
            {
                emptyCategoryText = response.responseObject;
            }
            else
            {
                emptyCategoryText = "[Üres kategória.]";
            }
        });

    requestInvoker
        .executeQuery('/GetTopLeftWatermarkText', {})
        .then((response) => {
            if (response.responseObject != null) {
                topLeftWatermarkText = response.responseObject;
            }
            else {
                topLeftWatermarkText = "[Bal felső sarok]";
            }
            tryMakeWatermarkPosComboBox();
        });

    requestInvoker
        .executeQuery('/GetTopRightWatermarkText', {})
        .then((response) => {
            if (response.responseObject != null) {
                topRightWatermarkText = response.responseObject;
            }
            else {
                topRightWatermarkText = "[Jobb felső sarok]";
            }
            tryMakeWatermarkPosComboBox();
        });

    requestInvoker
        .executeQuery('/GetBottomLeftWatermarkText', {})
        .then((response) => {
            if (response.responseObject != null) {
                borromLeftWatermarkText = response.responseObject;
            }
            else {
                borromLeftWatermarkText = "[Bal alsó sarok]";
            }
            tryMakeWatermarkPosComboBox();
        });

    requestInvoker
        .executeQuery('/GetBottomRightWatermarkText', {})
        .then((response) => {
            if (response.responseObject != null) {
                bottomRightWatermarkText = response.responseObject;
            }
            else {
                bottomRightWatermarkText = "[Jobb alsó sarok]";
            }
            tryMakeWatermarkPosComboBox();
        });

    var handle = $("#custom-handle"), handleWidth = handle.width();
    $("#watermarkOpacitySlider").slider({
        min: 35, max: 99, value: 80,
        slide: (e, ui) => {
            $("#sliderInput").val(ui.value);
            $("#sliderInputSpan").text(ui.value);
        }
    });

    requestInvoker
        .executeQuery('/Documents/TargetOfDocumentUsages', {})
        .then((response) => {
            let tartgetOfUsageCollection = [];

            $.each(response.responseObject, (key, value) => {
                tartgetOfUsageCollection.push({ key: key, value: value })
            });

            createCombobox($("#targetOfDocumentUsageSelectMenu"), tartgetOfUsageCollection);
        });
});

function tryMakeWatermarkPosComboBox() {
    if (
        topLeftWatermarkText != null &&
        topRightWatermarkText != null &&
        borromLeftWatermarkText != null &&
        bottomRightWatermarkText != null
    )
    {
        createCombobox($("#sidedWatermarkPositionSelectMenu"), [
            { key: 'upperLeftCorner', value: topLeftWatermarkText },
            { key: 'upperRightCorner', value: topRightWatermarkText },
            { key: 'bottomLeftCorner', value: borromLeftWatermarkText },
            { key: 'bottomRightCorner', value: bottomRightWatermarkText }]);
        $("#updateWatermarkButton").button();
    }
}

const feedbackChannel = {
    showInformation: (title, message) => { b5toast.show('info', title, message); },
    showWarning: (title, message) => { b5toast.show('warning', title, message); },
    showError: (title, message) => { b5toast.show('error', title, message); }
};

const requestInvoker = {
    executeQuery: (url, args) => { return requestInvoker.sendRequest(url, 'GET', args); },
    executeCommand: (url, args) => { return requestInvoker.sendRequest(url, 'POST', args); },
    executeUpdate: (url, args) => { return requestInvoker.sendRequest(url, 'PUT', args); },
    sendRequest: (url, requestType, args) => {
        let callback = (response) => { };
        let result = {
            then: (responseHandler) => { callback = responseHandler; }
        };
        $.ajax({
            url: baseUrl.slice(0,-1) + url,
            type: requestType,
            data: args,
            success: (response) => {
                console.log('response: ', response);
                if (response && response.feedbackMessages && response.feedbackMessages.length > 0) {
                    let displayedFeedback = response.feedbackMessages[0];
                    for (const feedbackMessage of response.feedbackMessages) {
                        if (feedbackMessage.severity !== 3) {
                            displayedFeedback = feedbackMessage;
                            break;
                        }
                    };
                    if (!displayedFeedback) {
                        feedbackChannel.showError('Ismeretlen hiba', 'Ismeretlen hiba történt. Kérjük jelezze kapcsolattartóink felé.');
                    } else if (displayedFeedback.severity < 1) {
                        feedbackChannel.showError('Hiba', displayedFeedback.message);
                    }
                    else if (displayedFeedback.severity === 2) {
                        feedbackChannel.showWarning('Figyelem', displayedFeedback.message);
                    }
                    else if (displayedFeedback.severity === 3) {
                        feedbackChannel.showInformation('Információ', displayedFeedback.message);
                    }
                }

                if (response && response.isOkay) {
                    callback(response);
                }
            },
            error: (xhr, status, error) => {
                console.error('xhr: ', xhr);
                console.error('status: ', status);
                console.error('error: ', error);
                feedbackChannel.showError('Ismeretlen hiba', 'Ismeretlen hiba történt. Kérjük jelezze kapcsolattartóink felé.');
            },
        });

        return result;
    }
};

const maxToastCount = 5;

const b5toast = {
    show: function (color, title, message) {
        title = title ? title : "";
        const html =
            `<div class="notify-container">
                <div class="rectangle ${color}">
                    <div class="notification-text">
                    <span><b>${title}:</b></span>
                    <span>&nbsp;&nbsp;${message}</span>
                    </div>
                </div>
            </div>`;
        const toastElement = b5toast.htmlToElement(html);
        const toastContainer = document.getElementById("toast-container");
        toastContainer.appendChild(toastElement);
        setTimeout(() => toastElement.remove(), b5toast.delayInMilliseconds);
        if (toastContainer.childElementCount > maxToastCount) {
            toastContainer.firstChild.remove();
        }
    },
    delayInMilliseconds: 5000,
    htmlToElement: (html) => {
        const template = document.createElement("template");
        html = html.trim();
        template.innerHTML = html;
        return template.content.firstChild;
    }
};

const documentTableBuilder = {
    createTable: (title, columns, records, actions, page, sortState) => {
        let documentTableElement = $(document.createElement('div'));
        documentTableElement.addClass('table-wrapper')
        let titleElement = documentTableBuilder.addTitleToTable(title);
        documentTableElement.append(titleElement);

        let attributeWrapperElement = $(document.createElement('table'));

        actions.onHeaderClick = (key) => {
            let newSortState = {
                sortBy: key,
                ascending: ((sortState.sortBy === key) ? !sortState.ascending : true)
            }
            actions.refresh(documentTableBuilder.createTable(title, columns, documentTableBuilder.sortElements(records, newSortState), actions, 1, newSortState), newSortState);
        };

        let subtitles = documentTableBuilder.generateRow(columns, actions, null, true, sortState);
        attributeWrapperElement.append(subtitles);

        if (records.length === 0) {
            attributeWrapperElement.addClass('empty');

            let emptyContentRow = $(document.createElement('tr'));

            let emptyContentElement = $(document.createElement('td'));
            emptyContentElement.addClass('empty');
            
            emptyContentElement.text(emptyCategoryText);

            emptyContentRow.append(emptyContentElement);
            attributeWrapperElement.append(emptyContentRow);

            emptyContentElement.attr('colspan', columns.size);
        }
        else {
            $.each(records.slice((page - 1) * 25, (page - 1) * 25 + 25), (index, record) => {
                let rowElement = documentTableBuilder.generateRow(record, actions, columns, false, null);
                attributeWrapperElement.append(rowElement);
            });
        }

        documentTableElement.append(attributeWrapperElement);

        if (records.length > 25) {
            let paginationControlsContainer = $(document.createElement('div'));
 
            if (page !== 1) {
                let firstPageButton = buttonBuilder.createButton('◀◀');
                firstPageButton.onclick = () => {
                    actions.refresh(documentTableBuilder.createTable(title, columns, records, actions, 1, sortState));
                };
                paginationControlsContainer.append(firstPageButton);

                let previousPageButton = buttonBuilder.createButton('◀');
                previousPageButton.onclick = () => {
                    actions.refresh(documentTableBuilder.createTable(title, columns, records, actions, page - 1, sortState));
                };
                paginationControlsContainer.append(previousPageButton);
            }

            let pageDisplay = document.createTextNode(`page ${page}/${Math.ceil(records.length / 25)}`);
            paginationControlsContainer.append(pageDisplay);

            if (page !== Math.ceil(records.length / 25)) {
                let nextPageButton = buttonBuilder.createButton('▶');
                nextPageButton.onclick = () => {
                    actions.refresh(documentTableBuilder.createTable(title, columns, records, actions, page + 1, sortState));
                };
                paginationControlsContainer.append(nextPageButton);

                let lastPageButton = buttonBuilder.createButton('▶▶');
                lastPageButton.onclick = () => {
                    actions.refresh(documentTableBuilder.createTable(title, columns, records, actions, Math.ceil(records.length / 25), sortState));
                };
                paginationControlsContainer.append(lastPageButton);
            }

            documentTableElement.append(paginationControlsContainer);
        }

        return documentTableElement;
    },

    createTableWithMovableProperties: (title, columns, actions, sortState) => {
        let documentTableElement = $(document.createElement('div'));
        documentTableElement.addClass('table-wrapper')
        let titleElement = documentTableBuilder.addTitleToTable(title);
        documentTableElement.append(titleElement);

        let attributeWrapperElement = $(document.createElement('div'));
        attributeWrapperElement.addClass('attribute-wrapper');

        let isEmpty = true;
        columns.every((column) => {
            if (column.content.size !== 0) {
                isEmpty = false;
                return false;
            }
            return true;
        });

        if (isEmpty) {
            let attributesElement = $(document.createElement('div'));
            attributesElement.addClass('attribute-wrapper');

            columns.forEach((column) => {
                let columnElement = documentTableBuilder.generateColumn(column.content, column.title, actions);
                attributesElement.append(columnElement);
            });

            attributeWrapperElement.append(attributesElement);
            attributeWrapperElement.addClass('empty');

            let emptyContentElement = $(document.createElement('p'));

            emptyContentElement.addClass('empty');
            emptyContentElement.addClass('attributes');
            emptyContentElement.text('Nem találhatóak a kért attribútumok.');

            attributeWrapperElement.append(emptyContentElement);
        }
        else {
            let max = columns.length - 1;

            columns.forEach((column, index) => {
                let type = {
                    pos: null,
                    col: index
                };

                switch (index) {
                    case 0: 
                        type.pos = 'left';
                        break;
                    case max:
                        type.pos = 'right';
                        break;
                    default:
                        type.pos = 'middle';
                        break;
                }

                actions.moveElement = (iFrom, iTo, iIndex) => documentTableBuilder.moveElement(columns, iFrom, iTo, iIndex, actions, sortState);
                actions.updateSort = (iSortState) => actions.refreshTable(columns, iSortState);

                let columnElement = documentTableBuilder.generateColumn(column.content, column.title, type, actions, sortState);
                attributeWrapperElement.append(columnElement);
            });
        }

        documentTableElement.append(attributeWrapperElement);

        return documentTableElement;
    },

    addTitleToTable: (documentName) => {
        let documentTitleElement = $(document.createElement('p'));
        documentTitleElement.addClass('category-title');

        if (documentName) {
            documentTitleElement.text(documentName);
        }
        else {
            documentTitleElement.text('Érvénytelen táblanév');
        }

        return documentTitleElement;
    },

    generateRow: (record, actions, columns, isSubtitle, sortState) => {
        let rowElement = $(document.createElement('tr'));

        if (record.size !== 0) {
            record.forEach((recordValue, recordKey) => {
                let attributeValueElement;

                if (isSubtitle) {
                    attributeValueElement = $(document.createElement('th'));
                     
                    attributeValueElement.append(`${(recordKey === sortState.sortBy) ? (sortState.ascending ? '⯅ ' : '⯆ ') : ''}${recordValue ?? '-'}`);
                    if (recordKey !== 'actions') {
                        attributeValueElement.on('click', () => actions.onHeaderClick(recordKey));
                    }
                }
                else if (columns.get(recordKey)) {
                    attributeValueElement = $(document.createElement('td'));

                    if (recordKey === 'nameWithExtension') {
                        let link = $(document.createElement('a'));
                        link.text(record.get('nameWithExtension'))

                        link.click((e) => {
                            e.stopPropagation();
                            showPreview(record.get('id'));
                        });
                        attributeValueElement.append(link)
                        attributeValueElement.css({ cursor: 'pointer' });
                    }
                    else {
                        attributeValueElement.append(recordValue ?? '-');
                    }
                }
                else {
                    return;
                }

                rowElement.append(attributeValueElement);
            });

            if (!isSubtitle && actions && 'onRowClick' in actions) {
                rowElement.addClass('list-item');
                rowElement.on('click', () => {
                    actions.onRowClick(record, rowElement, columns.size);
                });
            }
        }

        return rowElement;
    },

    generateColumn: (column, header, type, actions, sortState) => {
        let columnsElement = $(document.createElement('div'));
        columnsElement.addClass('attributes');
        columnsElement.addClass('category-table-column');

        let titleBarSubtitleElement = $(document.createElement('p'));
        titleBarSubtitleElement.addClass('subtitle');
        titleBarSubtitleElement.text(`${sortState[type.col] ? '⯅ ' : '⯆ '}${header}`);
        titleBarSubtitleElement.on('click', () => {
            sortState[type.col] = !sortState[type.col];
            actions.updateSort(sortState);
        });

        columnsElement.append(titleBarSubtitleElement);

        if (column.length !== 0) {
            $.each(column, (index, record) => {
                let attributeValueElementContainer = $(document.createElement('div'));
                attributeValueElementContainer.addClass('element');
                
                let attributeValueElement = $(document.createElement('p'));
                attributeValueElement.append(record.get('name') ?? '-');

                attributeValueElementContainer.append(attributeValueElement);

                type.row = index;

                attributeValueElementContainer.hover(
                    () => documentTableBuilder.onMoveableElementMouseEnter(attributeValueElement, type, index, actions),
                    () => documentTableBuilder.onMoveableElementMouseLeave(attributeValueElementContainer)
                );

                columnsElement.append(attributeValueElementContainer);
            });
        }

        return columnsElement;
    },
    
    generateDropDownView: (width) => {
        let detailsRow = $(document.createElement('tr'));
        let detailsContainer = $(document.createElement('td'));
        detailsContainer.attr('colspan', width);

        detailsRow.append(detailsContainer);

        return detailsContainer;
    },

    closeDropDownView: (openedRow, requireApprovement) => {
        if (requireApprovement && !confirm('Esetleges változtatások mentés nélkül elveszhetnek, folytatja?')) {
            return false;
        }

        openedRow.obj.remove();
        openedRow = null;

        return true;
    },
    
    onMoveableElementMouseEnter: (textElement, type, index, actions) => {
        if (type.pos === 'middle' || type.pos === 'right') {
            let moveLelftButton = buttonBuilder.createButton('◀');
            moveLelftButton.onclick = () => actions.moveElement(type.col, type.col - 1, index);
            textElement.before(moveLelftButton);
        }
        if (type.pos === 'middle' || type.pos === 'left') {
            let moveRightButton = buttonBuilder.createButton('▶');
            moveRightButton.onclick = () => actions.moveElement(type.col, type.col + 1, index);
            textElement.after(moveRightButton);
        }
    },
    
    onMoveableElementMouseLeave: (containerElement) => {
        containerElement.find('button').remove();
    },
    
    moveElement: (columns, from, to, index, actions, sortState) => {
        columns[to].content.push(columns[from].content[index]);
        columns[from].content.splice(index, 1);

        actions.refreshTable(columns, sortState);
    },

    sortElements: (records, sortState) => {
        if (records.length === 0) {
            return records;
        }

        records.sort((a, b) => {
            let valA = a.get(sortState.sortBy);
            let valB = b.get(sortState.sortBy);

            if (valA == null || valB == null) {
                let result = 0;

                if (valA == null) {
                    result++;
                }
                if (valB == null) {
                    result--;
                }

                return result;
            }

            return valA.localeCompare(valB);
        });

        if (!sortState.ascending) {
            records.reverse();
        }

        return records;
    }
};

const editDialogBuilder = {
    /**
     * Returns a dialog for editing values
     * @param {any} values a map describing the rows that should be edited:
     * when type is 'checkbox' or 'select':
     * ['recordKey', { type: type, title: recordTitle }]
     * when type is 'select':
     * ['recordKey', { type: type, options: options, title: recordTitle }]
     * where options is a map where the keys are the category ids and the values are the category names
     * @param {any} record the record which should be updated
     * @param {any} update the function which handles the update
     */
    createDialog: (values, record, update) => {
        let form = editDialogBuilder.generateForm(record, values);

        let dialog = $(document.createElement('div')).dialog({
            modal: true,
            title: 'Szerkesztés',
            autoOpen: false,
            dialogClass: 'edit-dialog',
            height: 300,
            resizable: false,
            open: function () {
                $(this).append(form.obj);
            },
            buttons: {
                'Mentés': function () {
                    let results = new Map();

                    for (let [key, field] of form.fields) {
                        if (values.get(key).type === 'checkbox') {
                            results.set(key, field.is(':checked'));
                        }
                        else if (values.get(key).type === 'select') {
                            results.set(key, field.val());
                        }
                        else {
                            results.set(key, field.val());
                        }
                    }

                    update(results, record);
                    $(this).dialog('close');
                },
                'Mégse': function () {
                    $(this).dialog('close');
                }
            }
        });

        return dialog;
    },
    
    generateForm: (record, values) => {
        let fields = new Map();

        let form = $(document.createElement('form'));

        for (let [key, value] of record) {
            let data = values.get(key)

            if (data) {
                form.append(editDialogBuilder.generateLabel(data, key));

                if (data.type !== 'checkbox') {
                    form.append($(document.createElement('br')));
                }

                fields.set(key, editDialogBuilder.generateInputs(data, key, value));
                form.append(fields.get(key));

                form.append($(document.createElement('br')));
            }
        }

        return {
            fields: fields,
            obj: form
        }
    },
    
    generateLabel: (data, key) => {
        let label = $(document.createElement('label'));
        label.attr('for', key);
        label.html(data.title);

        return label;
    },
    
    generateInputs: (data, key, value) => {
        if (data.type === 'select') {
            let input = $(document.createElement('select'));
            input.attr('name', key);

            data.options.forEach((name, id) => {
                let option = $(document.createElement('option'));
                option.attr('value', id);
                option.html(name);

                if (value === name) {
                    option.attr('selected', 'selected');
                }

                input.append(option);
            });

            return input;
        }

        let input = $(document.createElement('input'));
        input.attr('type', data.type);
        input.attr('name', key);

        if (data.type === 'checkbox') {
            input.prop('checked', value === 'Igen');
        }
        else {
            input.attr('value', value);
        }

        return input;
    }
};

const buttonBuilder = {
    createButton: (text) => {
        let button = document.createElement('BUTTON');
        button.className = "generated_button";

        let buttonText = document.createTextNode(text);
        button.appendChild(buttonText);

        return button;
    }
};

const storageHandler = {
    setCookie: (key, value, expireDays) => {
        const date = new Date();
        date.setTime(date.getTime() + (expireDays * 24 * 60 * 60 * 1000));

        document.cookie = `${key}=${value}; expires=${date.toUTCString()}; Secure; path=/`;
    },

    getCookie: (key) => {
        let name = key + '=';

        let decodedCookie = decodeURIComponent(document.cookie);
        let cookies = decodedCookie.split(';');

        for (let i = 0; i < cookies.length; i++) {
            let cookie = cookies[i];

            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1);
            }

            if (cookie.indexOf(name) === 0) {
                return cookie.substring(name.length, cookie.length);
            }
        }

        return '';
    },

    setFontSize: (fontSizeStr) => {
        let minFnotValue = $("#fontSize").attr("min") * 1;
        let maxFontValue = $("#fontSize").attr("max") * 1;
        let fontSize = fontSizeStr * 1;
        if (fontSize > maxFontValue) {
            fontSize = maxFontValue;
        }
        if (fontSize < minFnotValue) {
            fontSize = minFnotValue;
        }
        $("#fontSize").val(fontSize);
    },

    applyWatermarkSettings: () => {
        let watermarkSettingsJSON = localStorage.getItem('watermarkSettings');

        if (!watermarkSettingsJSON) {
            return;
        }

        let watermarkSettings = JSON.parse(watermarkSettingsJSON);

        if (watermarkSettings.watermarkOpacity) {
            $("#sliderInput").val(watermarkSettings.watermarkOpacity);
        }
        if (watermarkSettings.sideWatermarkPosition) {
            $("#sidedWatermarkPositionSelectMenu").val(watermarkSettings.sideWatermarkPosition);
        }
        if (watermarkSettings.centeredWatermarkHorizontalOffset) {
            $("#centeredWatermarkHorizontalOffset").val(watermarkSettings.centeredWatermarkHorizontalOffset);
        }
        if (watermarkSettings.centeredWatermarkVerticalOffset) {
            $("#centeredWatermarkVerticalOffset").val(watermarkSettings.centeredWatermarkVerticalOffset);
        }
        if (watermarkSettings.targetOfDocumentUsage) {
            $("#targetOfDocumentUsageSelectMenu").val(watermarkSettings.targetOfDocumentUsage);
        }
        if (watermarkSettings.fontSize) {
            storageHandler.setFontSize(watermarkSettings.fontSize);
        }
    },

    saveWatermarkSettings: () => {
        storageHandler.setFontSize($("#fontSize").val());

        let watermarkSettings = {
            watermarkOpacity: $("#sliderInput").val(),
            sideWatermarkPosition: $("#sidedWatermarkPositionSelectMenu").val(),
            centeredWatermarkHorizontalOffset: $("#centeredWatermarkHorizontalOffset").val(),
            centeredWatermarkVerticalOffset: $("#centeredWatermarkVerticalOffset").val(),
            targetOfDocumentUsage: $("#targetOfDocumentUsageSelectMenu").val(),
            fontSize: $("#fontSize").val()
        }

        localStorage.setItem('watermarkSettings', JSON.stringify(watermarkSettings));
    }
};

let showPreview = (documentId, refresh = false) => {
    if (!refresh) {
        storageHandler.applyWatermarkSettings();
    }
    
    let args = {
        id: documentId,
        watermarkOpacity: $("#sliderInput").val(),
        sideWatermarkPosition: $("#sidedWatermarkPositionSelectMenu").val(),
        centeredWatermarkHorizontalOffset: $("#centeredWatermarkHorizontalOffset").val(),
        centeredWatermarkVerticalOffset: $("#centeredWatermarkVerticalOffset").val(),
        targetOfDocumentUsage: $("#targetOfDocumentUsageSelectMenu").val(),
        fontSize: $("#fontSize").val()
    };

    requestInvoker
        .executeQuery('/Documents/DocumentPreview', args)
        .then((response) => {
            $("#pdfPreview").attr('src', 'data:application/pdf;base64,' + response.responseObject);

            $("#pdfViewer").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true,
                fadeDuration: 100,
                closeClass: 'icon-remove',
                modalClass: 'pdf-modal',
                closeText: 'X'
            });

            $("#updateWatermarkButton").unbind();

            $("#updateWatermarkButton").click((e) => {
                e.stopPropagation();
                $.modal.close();
                storageHandler.saveWatermarkSettings();
                showPreview(documentId, true);
            });
        });
}

let createCombobox = (selectElement, values) => {
    $.each(values, (index, value) => {
        let optionElement = $(document.createElement('option'));
        optionElement.attr('value', value.key);
        optionElement.text(value.value);
        if (index == 0) {
            optionElement.attr('selected', 'selected');
        }

        selectElement.append(optionElement);
    });
}
