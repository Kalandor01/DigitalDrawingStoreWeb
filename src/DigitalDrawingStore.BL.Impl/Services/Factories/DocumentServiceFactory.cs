using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.BL.Services.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories
{
    public class DocumentServiceFactory : IDocumentServiceFactory
    {
        public IDocumentService CreateDocumentService(string applicationConfigurationFilePath)
        {
            if (string.IsNullOrWhiteSpace(applicationConfigurationFilePath))
            {
                throw new ArgumentException($"'{nameof(applicationConfigurationFilePath)}' cannot be null or whitespace.", nameof(applicationConfigurationFilePath));
            }

            var sqlTableNames = new Dictionary<string, string>()
            {
                { Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, "DocumentCategories" },
                { Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, "DocumentCategoryEntities" },
                { Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY, "DocumentMetadataDefinitions" },
                { Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY, "DocumentMetadata" },
                { Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY, "Documents" },
                { Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY, "ApplicationProperties" },
                { Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_DICTIONARY_TABLE_NAME_KEY, "ApplicationPropertiesDictionary" },
                { Constants.Documents.Resources.DatabaseTables.USER_EVENT_LOGS_TABLE_NAME_KEY, "UserEventLogs" },
            };

            return CreateDocumentService(applicationConfigurationFilePath, sqlTableNames);
        }

        public IDocumentService CreateDocumentService(string applicationConfigurationFilePath, IDictionary<string, string> sqlTableNames)
        {
            if (string.IsNullOrWhiteSpace(applicationConfigurationFilePath))
            {
                throw new ArgumentException($"'{nameof(applicationConfigurationFilePath)}' cannot be null or whitespace.", nameof(applicationConfigurationFilePath));
            }

            if (sqlTableNames is null)
            {
                throw new ArgumentNullException(nameof(sqlTableNames));
            }

            applicationConfigurationFilePath = Path.GetFullPath(applicationConfigurationFilePath);

            var container = new ContainerFactory().CreateContainer();

            var feedbackMessageFactory = container.Resolve<IFeedbackMessageFactory>();

            var applicationConfigurationServiceFactory = container.Resolve<IApplicationConfigurationServiceFactory>();
            var applicationConfigurationService = applicationConfigurationServiceFactory.CreateApplicationConfigurationService(applicationConfigurationFilePath);

            var documentResourceProperties = new DocumentResourceProperies(applicationConfigurationService);

            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            commonApplicationProperties.GeneralApplicationProperties.ApplicationConfigurationFilePath = applicationConfigurationFilePath;

            var msSqlDataSource = default(IDataSource);
            var dataSourceFactory = container.Resolve<IDataSourceFactory>();
            var documentResourcePath = documentResourceProperties.ResourcePath;

            var dataParameterFactory = container.Resolve<IDataParameterFactory>();
            if (!string.IsNullOrWhiteSpace(documentResourcePath))
            {
                msSqlDataSource = dataSourceFactory.CreateMsSqlDataSource(documentResourcePath);
            }
            else
            {
                msSqlDataSource = dataSourceFactory.CreateMsSqlDataSource(Constants.Documents.Resources.DEFAULT_CONNECTION_STRING);
            }
            
            var documentCategoryFactory = new DocumentCategoryFactory(msSqlDataSource, dataParameterFactory, sqlTableNames);
            var documentCategoryQuery = new MsSqlDocumentCategoryQuery(msSqlDataSource, dataParameterFactory, sqlTableNames, documentCategoryFactory, feedbackMessageFactory);
            var watermarkProvider = new PdfDocumentWatermarkProvider();
            var documentExporters = new List<IDocumentExporter>() { new PdfDocumentExporter() };
            var documentFactory = new DocumentFactory(msSqlDataSource, dataParameterFactory, sqlTableNames, documentExporters, watermarkProvider);
            var documentQuery = new MsSqlDocumentQuery(msSqlDataSource, dataParameterFactory, sqlTableNames, documentFactory, feedbackMessageFactory);
            var categoryAttributeQuery = new MsSqlDocumentCategoryEntitiesQuery(msSqlDataSource, dataParameterFactory, sqlTableNames, documentCategoryFactory, feedbackMessageFactory);
            var documentCategoryCommand = new MsSqlUpdateDocumentCategoryCommand(msSqlDataSource, dataParameterFactory, sqlTableNames);
            var documentMetadataCommand = new MsSqlUpdateDocumentMetadataCommand(msSqlDataSource, dataParameterFactory, sqlTableNames, documentFactory);
            var documentCategoryEntityCommand = new MsSqlUpdateDocumentCategoryEntitiesCommand(msSqlDataSource, dataParameterFactory, sqlTableNames, documentCategoryFactory);
            var documentCategoryRelationCommand = new MsSqlUpdateDocumentCategoryRelationCommand(msSqlDataSource, dataParameterFactory, sqlTableNames);

            return new DocumentService(documentCategoryQuery, documentQuery, categoryAttributeQuery, documentCategoryCommand, documentMetadataCommand, documentCategoryEntityCommand, documentCategoryRelationCommand);
        }
    }
}
