using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Application.DataSource;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Queries;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.BL.Services.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories
{
    public class FeedbackPropertiesServiceFactory : IFeedbackPropertiesServiceFactory
    {
        public IFeedbackPropertiesService CreateFeedbackPropertyService(string applicationConfigurationFilePath)
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
            };

            applicationConfigurationFilePath = Path.GetFullPath(applicationConfigurationFilePath);

            var container = new ContainerFactory().CreateContainer();

            var feedbackMessageFactory = container.Resolve<IFeedbackMessageFactory>();

            var applicationConfigurationServiceFactory = container.Resolve<IApplicationConfigurationServiceFactory>();
            var applicationConfigurationService = applicationConfigurationServiceFactory.CreateApplicationConfigurationService(applicationConfigurationFilePath);

            var documentResourceProperties = new DocumentResourceProperies(applicationConfigurationService);

            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            commonApplicationProperties.GeneralApplicationProperties.ApplicationConfigurationFilePath = applicationConfigurationFilePath;

            var dataSourceFactory = container.Resolve<IDataSourceFactory>();
            var dataParameterFactory = container.Resolve<IDataParameterFactory>();
            var msSqlDataSource = default(IDataSource);

            var documentResourcePath = documentResourceProperties.ResourcePath;

            if (!string.IsNullOrWhiteSpace(documentResourcePath))
            {
                msSqlDataSource = dataSourceFactory.CreateMsSqlDataSource(documentResourcePath);
            }
            else
            {
                msSqlDataSource = dataSourceFactory.CreateMsSqlDataSource(Constants.Documents.Resources.DEFAULT_CONNECTION_STRING);
            }

            var feedbackPropertyQuery = new FeedbackPropertyQuery(msSqlDataSource, dataParameterFactory, sqlTableNames, feedbackMessageFactory);
            var updateFeedbackPropertyCommand = new UpdateFeedbackPropertiesCommand(msSqlDataSource, dataParameterFactory, sqlTableNames);

            return new FeedbackPropertiesService(feedbackPropertyQuery, updateFeedbackPropertyCommand);
        }
    }
}
