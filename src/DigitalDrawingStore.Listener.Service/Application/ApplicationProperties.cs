using DigitalDrawingStore.Listener.Service.Services.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using XperiCad.Common.Infrastructure.Application.DataSource;

namespace DigitalDrawingStore.Listener.Service.Application
{
    internal class ApplicationProperties : IApplicationProperties
    {
        #region Properties
        public IDocumentResourceProperies DocumentResourceProperies { get; }
        public IEnumerable<string> ObservedLocations => GetObservedLocations();
        public string DocumentDropDirectory => GetDocumentDropDirectory();
        public IFeedbackProperties FeedbackProperties { get; }
        #endregion

        #region Fields
        private readonly IApplicationConfigurationService _applicationConfigurationService;
        #endregion

        #region ctor
        public ApplicationProperties(
            IDocumentResourceProperies documentResourceProperies,
            IApplicationConfigurationService applicationConfigurationService,
            IFeedbackProperties feedbackProperties)
        {
            DocumentResourceProperies = documentResourceProperies ?? throw new ArgumentNullException(nameof(documentResourceProperies));
            _applicationConfigurationService = applicationConfigurationService ?? throw new ArgumentNullException(nameof(applicationConfigurationService));
            FeedbackProperties = feedbackProperties ?? throw new ArgumentNullException(nameof(feedbackProperties));
        }
        #endregion

        #region Private members
        private IEnumerable<string> GetObservedLocations()
        {
            var locations = _applicationConfigurationService.Query.GetStringCollectionByName("ObservedLocations");
            var normalizedLocations = new List<string>();

            if (locations != null)
            {
                foreach (var location in locations)
                {
                    if (!string.IsNullOrWhiteSpace(location))
                    {
                        normalizedLocations.Add(ServicePath.GetFullPath(location));
                    }
                }
            }

            return normalizedLocations;
        }

        private string GetDocumentDropDirectory()
        {
            var location = _applicationConfigurationService.Query.GetStringPropertyByName("DropLocation");
            var normalizedLocation = ServicePath.GetFullPath(location);

            return normalizedLocation;
        }
        #endregion
    }
}
