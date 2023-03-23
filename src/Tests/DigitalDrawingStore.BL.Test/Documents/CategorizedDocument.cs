using Moq;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents
{
    public class CategorizedDocument
    {
        #region Properties
        public string DocumentCategoryName { get; }
        public IDocumentCategory DocumentCategory { get; }
        public IDocument Document { get; }
        #endregion

        #region ctor
        public CategorizedDocument(Guid documentCategoryId, string documentCategoryName, IDocument document)
        {
            if (string.IsNullOrWhiteSpace(documentCategoryName))
            {
                throw new ArgumentException($"'{nameof(documentCategoryName)}' cannot be null or whitespace.", nameof(documentCategoryName));
            }

            if (documentCategoryId == Guid.Empty)
            {
                throw new ArgumentException($"Argument {nameof(documentCategoryId)} could not be an empty Guid.");
            }

            DocumentCategoryName = documentCategoryName;
            DocumentCategory = new MsSqlDocumentCategory(documentCategoryId, Mock.Of<IDataSource>(), Mock.Of<IDataParameterFactory>(), new Dictionary<string, string>());
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public CategorizedDocument(IDocumentCategory documentCategory, IDocument document)
        {
            DocumentCategoryName = "TestCategory";
            DocumentCategory = documentCategory ?? throw new ArgumentNullException(nameof(documentCategory));
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public CategorizedDocument(string documentCategoryName, IDocumentCategory documentCategory, IDocument document)
        {
            DocumentCategoryName = documentCategoryName ?? throw new ArgumentNullException(nameof(documentCategoryName));
            DocumentCategory = documentCategory ?? throw new ArgumentNullException(nameof(documentCategory));
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
        #endregion
    }
}
