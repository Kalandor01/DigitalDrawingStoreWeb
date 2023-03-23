using DigitalDrawingStore.Listener.Service.Application;
using System;
using System.Collections.Generic;
using System.IO;

namespace DigitalDrawingStore.Listener.Service.Document.Resources
{
    internal class DropLocationDocumentResource : IDocumentResource
    {
        #region Fields
        private readonly IApplicationProperties _applicationProperties;
        #endregion

        #region Constructor
        public DropLocationDocumentResource(IApplicationProperties applicationProperties)
        {
            _applicationProperties = applicationProperties ?? throw new ArgumentNullException(nameof(applicationProperties));
        }
        #endregion

        #region IDocumentResource members
        public void SaveDocuments(IEnumerable<IRawDocument> rawDocuments)
        {
            if (rawDocuments is null)
            {
                throw new ArgumentNullException(nameof(rawDocuments));
            }

            var dropDirectory = _applicationProperties.DocumentDropDirectory;

            foreach (var rawDocument in rawDocuments)
            {
                var documentPath = rawDocument.DocumentData.DocumentPath;

                if (File.Exists(documentPath) && Directory.Exists(dropDirectory))
                {
                    var targetPath = $"{dropDirectory}\\{Path.GetFileName(documentPath)}";

                    if (!File.Exists(targetPath))
                    {
                        File.Copy(documentPath, targetPath);
                        File.Delete(documentPath);
                        rawDocument.DocumentData.DocumentPath = targetPath;
                    }
                }
            }
        }
        #endregion
    }
}
