using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;

namespace XperiCad.DigitalDrawingStore.BL.Test.Documents.Factories
{
    /// <summary>
    /// This collector class is used to pass the original DocumentFactory object in test parameters because the IDocumentFactory interface is internal.
    /// </summary>
    public class TestDocumentFactory
    {
        #region Fields
        private IDocumentFactory? _documentFactory;
        #endregion

        #region Properties
        internal IDocumentFactory DocumentFactory
        {
            get
            {
                if (_documentFactory == null)
                {
                    throw new InvalidOperationException($"{nameof(TestDocumentFactory)} was used before initialization.");
                }

                return _documentFactory;
            }
            set
            {
                _documentFactory = value;
            }
        }
        #endregion
    }
}
