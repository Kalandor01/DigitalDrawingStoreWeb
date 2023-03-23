using DigitalDrawingStore.Listener.Service.Application;
using DigitalDrawingStore.Listener.Service.Document.Factories;
using System.Collections.Generic;
using System.IO;

namespace DigitalDrawingStore.Listener.Service.Document.Scout
{
    internal class DocumentScout : IDocumentScout
    {
        #region Fields
        private readonly IApplicationProperties _applicationProperties;
        private readonly IRawDocumentFactory _rawDocumentFactory;
        #endregion

        #region Constructor
        public DocumentScout(
            IApplicationProperties applicationProperties,
            IRawDocumentFactory rawDocumentFactory)
        {
            _applicationProperties = applicationProperties ?? throw new System.ArgumentNullException(nameof(applicationProperties));
            _rawDocumentFactory = rawDocumentFactory ?? throw new System.ArgumentNullException(nameof(rawDocumentFactory));
        }
        #endregion

        #region IDocumentScout members
        public IEnumerable<IRawDocument> FindDocuments()
        {
            var observedLocations = _applicationProperties.ObservedLocations;
            var result = new List<IRawDocument>();

            if (observedLocations == null)
            {
                // TODO: feedback
                return result;
            }

            foreach (var observedLocation in observedLocations)
            {
                if (Directory.Exists(observedLocation))
                {
                    var directoryInfo = new DirectoryInfo(observedLocation);
                    var foundFiles = directoryInfo.GetFiles();

                    foreach (var foundFile in foundFiles)
                    {
                        var filePath = Path.GetFullPath(foundFile.FullName);
                        var rawDocument = _rawDocumentFactory.CreateRawDocument(filePath);
                        if (rawDocument != null)
                        {
                            result.Add(rawDocument);
                        }
                    }
                }
                else
                {
                    // TODO: feedback
                }
            }

            return result;
        }
        #endregion
    }
}
