using XperiCad.Common.Infrastructure.Behaviours.Queries;

namespace XperiCad.DigitalDrawingStore.BL.Documents.Queries
{
    public interface IFeedbackPropertyQuery
    {
        /// <summary>
        /// Queries a feedback property value based on a key.
        /// </summary>
        /// <returns></returns>
        Task<IPromise<string>> QueryFeedbackPropertyAsync(string propertyKey);
    }
}
