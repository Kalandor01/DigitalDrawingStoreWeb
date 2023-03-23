using DigitalDrawingStore.Listener.Service.Application;
using DigitalDrawingStore.Listener.Service.Document.Factories;
using DigitalDrawingStore.Listener.Service.Document.Resources;
using DigitalDrawingStore.Listener.Service.Document.Resources.Validator;
using DigitalDrawingStore.Listener.Service.Document.Scout;
using System;
using System.Collections.Generic;
using System.IO;
using XperiCad.Common.Core.Application.DataSource;
using XperiCad.Common.Core.DataSource;

namespace DigitalDrawingStore.Listener.Service.Services.Factories
{
    internal class ListenerServiceFactory : IListenerServiceFactory
    {
        public IListenerService CreateDocumentListenerService(string applicationConfigurationFilePath, IDictionary<string, string> sqlTableNames)
        {
            applicationConfigurationFilePath = Path.GetFullPath(applicationConfigurationFilePath);

            var applicationConfigurationCommand = new XmlApplicationConfigurationCommand(applicationConfigurationFilePath);
            var applicationConfigurationQuery = new XmlApplicationConfigurationQuery(applicationConfigurationFilePath);
            var applicationConfigurationService = new ApplicationConfigurationService(applicationConfigurationQuery, applicationConfigurationCommand);
            var documentResourceProperties = new DocumentResourceProperies(applicationConfigurationService);
            var connectionString = documentResourceProperties.ResourcePath;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Could not load database connection string from configuration file: {applicationConfigurationFilePath}.");
            }
            var dataParameterFactory = new DataParameterFactory();
            var msSqlDataSource = new MsSqlDataSource(connectionString);
            var feedbackProperties = new FeedbackProperties(dataParameterFactory, msSqlDataSource, sqlTableNames);
            var applicationProperties = new ApplicationProperties(documentResourceProperties, applicationConfigurationService, feedbackProperties);

            var msSqlDocumentResource = new MsSqlDocumentResource(dataParameterFactory, msSqlDataSource, sqlTableNames);
            var documentScout = new DocumentScout(applicationProperties, new RawDocumentFactory());
            var dropLocationDocumentResource = new DropLocationDocumentResource(applicationProperties);

            var documentResourcePool = new DocumentResourceValidator(
                new DocumentResourcePool(new List<IDocumentResource>() { dropLocationDocumentResource, msSqlDocumentResource }),
                dataParameterFactory,
                msSqlDataSource,
                sqlTableNames,
                applicationProperties);

            var listenerService = new DocumentListenerService(documentScout, documentResourcePool);

            return listenerService;
        }
    }
}
