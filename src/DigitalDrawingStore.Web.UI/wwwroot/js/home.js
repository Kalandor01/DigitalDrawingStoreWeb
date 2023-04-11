$(document).ready(() => {
    let searchButton = $('#button-addon2');
    searchButton.on('click', (e) => {
        e.stopPropagation();

        let content = $('#homeTablesContainer');
        content.remove();
        populatePage();
    });

    populatePage();
});

populatePage = () => {
    requestInvoker
        .executeQuery('/Categories/', { searchText: $('#search-txt').val() ?? '*' })
        .then((response) => {
            let contentContainerElement = $('#contentContainer');

            let tableContainer = $(document.createElement('div'));
            tableContainer.attr('id', 'homeTablesContainer');

            let categories = response.responseObject;

            $.each(categories, (index, category) => {
                let tableContainerElement = $(document.createElement('div'));
                if (category.isDesigned) {
                    let tableName = category.categoryName;

                    let columns = new Map();
                    $.each(category.attributes, (index, categoryAttribute) => {
                        columns.set(index, categoryAttribute);
                    });

                    let records = [];
                    $.each(category.documents, (index, document) => {
                        let record = new Map();
                        record.set('id', document.id)

                        columns.forEach((columnTitle, columnKey) => {
                            record.set(columnKey, document.attributes[columnKey]);
                        });

                        record.set('documentPath', document.path);

                        records.push(record);
                    });

                    let table;

                    let actions = {
                        refresh: (newTable) => {
                            table.remove();
                            table = newTable;
                            tableContainerElement.append(table);
                        }
                    };

                    let sortState = {
                        sortBy: 'nameWithExtension',
                        ascending: true
                    };

                    table = documentTableBuilder.createTable(tableName, columns, documentTableBuilder.sortElements(records, sortState), actions, 1, sortState);
                    tableContainerElement.append(table);

                    tableContainer.append(tableContainerElement);
                }
            });

            contentContainerElement.append(tableContainer);
        });
}
