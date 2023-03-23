using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Documents.Exporters;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Exporters;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories
{
    public class DocumentFactory : IDocumentFactory
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IEnumerable<IDocumentExporter> _documentExporters;
        private readonly IDocumentWatermarkProvider _documentWatermarkProvider;
        #endregion

        #region ctor
        public DocumentFactory(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IEnumerable<IDocumentExporter> documentExporters,
            IDocumentWatermarkProvider documentWatermarkProvider)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentExporters = documentExporters ?? throw new ArgumentNullException(nameof(documentExporters));
            _documentWatermarkProvider = documentWatermarkProvider ?? throw new ArgumentNullException(nameof(documentWatermarkProvider));
        }
        #endregion

        #region IDocumentFactory members
        public IDocument CreatePdfDocument(Guid id, string path)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Parameter {nameof(id)} could not be empty.");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            var pdfExporter = _documentExporters.OfType<PdfDocumentExporter>().FirstOrDefault()
                                  ?? throw new ArgumentException("No pdf exporters found in registered document exporters.");

            return new MsSqlPdfDocument(_msSqlDataSource, _dataParameterFactory, _sqlTableNames, id, path, pdfExporter, _documentWatermarkProvider);
        }
        #endregion
    }
}
