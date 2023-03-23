let openedRow = null;
let mainTableSortState = {
    sortBy: 'nameWithExtension',
    ascending: true
};
let detailsTableSortState = {
    sortBy: 'metadataName',
    ascending: true
};

$(document).ready(() => {
    let searchButton = $('#button-addon2');
    searchButton.on('click', (e) => {
        e.stopPropagation();

        let content = $('#documentTableContainer');
        content.remove();
        populatePage();
    });

    populatePage();
});

function populatePage() {
    requestInvoker
        .executeQuery('/Documents', { searchText: $('#search-txt').val() ?? '*' })
        .then((response) => {
            let contentContainerElement = $('#contentContainer');
            let documents = response.responseObject;

            let tableName = 'Dokumentumok';

            let columns = new Map();
            columns.set('nameWithExtension', 'Név');
            columns.set('category', 'Kategória');
            columns.set('actions', 'Műveletek');

            let records = [];

            let tableContainer = $(document.createElement('div'));
            tableContainer.attr('id', 'documentTableContainer');

            $.each(documents, (index, document) => {
                let record = new Map();

                record.set('id', document.id);
                record.set('nameWithExtension', document.nameWithExtension);
                record.set('category', document.category.categoryName);
                record.set('categoryId', document.category.id)
                record.set('attributes', document.attributes);

                let button = buttonBuilder.createButton('Szerkesztés');
                button.onclick = (e) => {
                    e.stopPropagation();
                    editDocumentCategory(record, columns);
                };
                record.set('actions', button);

                records.push(record);
            });

            let table;

            let actions = {
                refresh: (newTable, newMainTableSortState) => {
                    mainTableSortState = newMainTableSortState;
                    table.remove();
                    table = newTable;
                    tableContainer.append(table);
                },
                onRowClick: onRowClick
            };

            table = documentTableBuilder.createTable(tableName, columns, documentTableBuilder.sortElements(records, mainTableSortState), actions, 1, mainTableSortState);
            tableContainer.append(table);
            contentContainerElement.append(tableContainer);
        });
}

function onRowClick(record, row, numParentColumns) {
    requestInvoker
        .executeQuery('/Documents/Metadata', { documentId: record.get('id') })
        .then((response) => {
            let columns = new Map();
            columns.set('metadataName', 'Metaadat neve');
            columns.set('metadataValue', 'Értéke');
            columns.set('actions', 'Műveletek');

            let attributes = [];

            $.each(response.responseObject, (key, value) => {
                let attribute = new Map();
                attribute.set('metadataName', key);
                attribute.set('metadataValue', value);
                attribute.set('documentId', record.get('id'));

                let button = buttonBuilder.createButton('Szerkesztés');
                button.onclick = (e) => {
		            e.stopPropagation();
                    editAttribute(attribute, columns, () => onRowClick(record, row, numParentColumns));
		        };
                attribute.set('actions', button);

                attributes.push(attribute);
            });

            openDetails(record, row, numParentColumns, {
                attributes: attributes,
                columns: columns
            });
        });
}

function openDetails(record, row, numParentColumns, details) {
    if (openedRow) {
        documentTableBuilder.closeDropDownView(openedRow, false);
    }

    let detailsContainer = documentTableBuilder.generateDropDownView(numParentColumns);
    detailsContainer.addClass('opened');
    
    let tableContainer = $(document.createElement('div'));
    let table;

    let actions = {
        refresh: (newTable, newDetailsTableSortState) => {
            detailsTableSortState = newDetailsTableSortState;
            table.remove();
            table = newTable;
            tableContainer.append(table);
            table.addClass('opened');
        }
    }

    table = documentTableBuilder.createTable(record.get('nameWithExtension'), details.columns, documentTableBuilder.sortElements(details.attributes, detailsTableSortState), actions, 1, detailsTableSortState);
    table.addClass('opened');

    tableContainer.append(table)
    detailsContainer.append(tableContainer);

    let closeButton = buttonBuilder.createButton('Bezárás');
    closeButton.onclick = () => documentTableBuilder.closeDropDownView(openedRow, false);
    detailsContainer.append(closeButton);

    row.after(detailsContainer.parent());

    openedRow = {
        obj: detailsContainer.parent(),
        id: record.get('id')
    };
}

function editAttribute(attribute, columns, refresh) {
    let values = new Map();
    values.set('metadataName', { type: 'text', title: columns.get('metadataName') });
    values.set('metadataValue', { type: 'text', title: columns.get('metadataValue') });

    editDialogBuilder.createDialog(values, attribute, (iResults, iAttribute) => updateAttribute(iResults, iAttribute, refresh)).dialog('open');
}

function editDocumentCategory(document, columns) {
    requestInvoker
        .executeQuery('/Categories', { searchText: '*' })
        .then((response) => {
            let categories = response.responseObject;
            let options = new Map();

            $.each(categories, (index, category) => {
                options.set(category.id, category.categoryName);
            });

            let values = new Map();
            values.set('category', { type: 'select', options: options, title: columns.get('category') });

            editDialogBuilder.createDialog(values, document, (iResult, iDocument) => updateDocumentCategory(iResult, iDocument, () => {
                let content = $('#documentTableContainer');
                content.remove();
                populatePage();
            })).dialog('open');
        });
}

function updateAttribute(results, attribute, refresh) {
    requestInvoker
        .executeUpdate('/Documents/UpdateMetadata',
            {
                documentId: attribute.get('documentId'),
                metadataName: results.get('metadataName'),
                metadataValue: results.get('metadataValue'),
                oldMetadataName: attribute.get('metadataName')
            })
        .then((response) => {
            refresh();
            //TODO: feedback to the user whether the update was successful
        });
}

function updateDocumentCategory(result, document, refresh) {
    requestInvoker
        .executeUpdate('/Documents/UpdateDocumentCategoryRelation',
            {
                documentId: document.get('id'),
                newCategoryId: result.get('category')
            })
        .then((response) => {
            refresh();
        });
}
