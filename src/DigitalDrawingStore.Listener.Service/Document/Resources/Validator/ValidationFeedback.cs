using System;

namespace DigitalDrawingStore.Listener.Service.Document.Resources.Validator
{
    internal class ValidationFeedback
    {
        #region Properties
        public IRawDocument RawDocument { get; }
        public string Message { get; }
        #endregion

        #region ctor
        public ValidationFeedback(IRawDocument rawDocument, string message)
        {
            RawDocument = rawDocument ?? throw new ArgumentNullException(nameof(rawDocument));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
        #endregion
    }
}
