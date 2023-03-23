let openedRow = null;
let mainTableSortState = {
    sortBy: 'categories',
    ascending: true
};

$(document).ready(() => populatePage());

populatePage = () => {
    requestInvoker
        .executeQuery('/Categories', { searchText: '*' })
        .then((response) => {
            let contentContainerElement = $('#categoriesContentContainer');
            let categories = response.responseObject;

            let tableName = 'Kategóriák';

            let columns = new Map();
            columns.set('categories', 'Dokumentum kategóriák');
            columns.set('isDesigned', 'Tervezett kategória');
            columns.set('actions', 'Műveletek');

            let table;

            let records = [];
            $.each(categories, (index, category) => {
                let record = new Map();

                record.set('categories', category.categoryName);
                if (category.isDesigned) {
                    record.set('isDesigned', 'Igen');
                } else {
                    record.set('isDesigned', 'Nem');
                }
                record.set('id', category.id);

                let button = buttonBuilder.createButton('Szerkesztés');
                button.onclick = (e) => {
                    e.stopPropagation();
                    editCategory(record, columns);
                };
                record.set('actions', button);

                records.push(record);
            });

            let actions = {
                refresh: (newTable, newMainTableSortState) => {
                    mainTableSortState = newMainTableSortState;
                    table.remove();
                    table = newTable;
                    table.attr('id', 'categoriesContent');
                    contentContainerElement.append(table);
                },
                onRowClick: getDetails
            };

            table = documentTableBuilder.createTable(tableName, columns, documentTableBuilder.sortElements(records, mainTableSortState), actions, 1, mainTableSortState);
            table.attr('id', 'categoriesContent');
            contentContainerElement.append(table);
        });
}

editCategory = (record, columns) => {
    let values = new Map();
    values.set('categories', {type: 'text', title: columns.get('categories')});

    editDialogBuilder.createDialog(values, record, updateCategory).dialog('open');
}


getDetails = (record, row, numParentColumns) => {
    requestInvoker
        .executeQuery('/Categories/Metadata', { categoryId: record.get('id') })
        .then((response) => {
            let result = response.responseObject;

            let usedEntities = [];
            for (const [key, value] of Object.entries(result.usedEntities)) {
                let map = new Map();
                map.set('name', value);
                map.set('id', key);

                usedEntities.push(map);
            }

            let unusedEntities = [];
            for (const [key, value] of Object.entries(result.unusedEntities)) {
                let map = new Map();
                map.set('name', value);
                map.set('id', key);

                unusedEntities.push(map);
            }

            let columns = [
                { title: 'Hozzáadott tulajdonság', content: usedEntities },
                { title: 'Szabad tulajdonság', content: unusedEntities }
            ]

            openDetails(record, row, numParentColumns, columns);
        });
}

openDetails = (record, row, numParentColumns, columns, sortState) => {
    if (openedRow) {
        if (!documentTableBuilder.closeDropDownView(openedRow, 'updated' in openedRow && openedRow.updated && 'refresh' in openedRow && !openedRow.refresh)) {
            openedRow.updated = false;
            return;
        }

        if ('refresh' in openedRow && openedRow.refresh) {
            openedRow.refresh = false;
        } else if ('updated' in openedRow && openedRow.updated) {
            openedRow.updated = false;
        }
    } else {
        openedRow = {};
    }

    if (!sortState) {
        sortState = [true, true];
    }

    columns[0].content = documentTableBuilder.sortElements(columns[0].content, {
        sortBy: 'name',
        ascending: sortState[0]
    });
    columns[1].content = documentTableBuilder.sortElements(columns[1].content, {
        sortBy: 'name',
        ascending: sortState[1]
    });

    let detailsContainer = documentTableBuilder.generateDropDownView(numParentColumns);
    detailsContainer.addClass('opened');
    let actions = {
        refreshTable: (iColumns, iSortState) => {
            openedRow.refresh = true;
            openedRow.updated = true;
            openDetails(record, row, numParentColumns, iColumns, iSortState);
        }
    };
    let table = documentTableBuilder.createTableWithMovableProperties(record.get('categories'), columns, actions, sortState);

    detailsContainer.append(table);

    let closeButton = buttonBuilder.createButton('Bezárás');
    closeButton.onclick = () => {
        documentTableBuilder.closeDropDownView(openedRow, 'updated' in openedRow && openedRow.updated);
        openedRow = null;
    };
    detailsContainer.append(closeButton);

    let saveButton = buttonBuilder.createButton('Mentés');
    saveButton.onclick = () => {
        if (openedRow.updated) {
            updateCategoryEntities(columns, record);
            openedRow.updated = false;
        }
    };
    detailsContainer.append(saveButton);

    row.after(detailsContainer.parent());

    openedRow.obj = detailsContainer.parent();
    openedRow.id = record.get('id');
}

updateCategoryEntities = (results, record) => {
    console.log('results: ', results)
    console.log('record: ', record)

    let keys = [];
    let values = [];

    $.each(results[0].content, (index, map) => {
        keys.push(map.get('id'));
        values.push(map.get('name'));
    });

    let result = JSON.stringify({ categoryId: record.get('id'), keys: keys, values: values }); // TODO: refactor parameter passing

    requestInvoker
        .executeUpdate('/Categories/UpdateMetadata',
            { result: result })
        .then((response) => {
            //TODO: feedback to the user whether the update was successful
        });
}

updateCategory = (result, record) => {
    requestInvoker
        .executeUpdate('/Documents/UpdateCategory',
            {id: record.get('id'), categoryName: result.get('categories')})
        .then((response) => {
            let table = $('#categoriesContent');
            table.remove();
            populatePage();
            //TODO: feedback to the user whether the update was successful
        });
}
