using XperiCad.Common.Infrastructure.Feedback;

namespace XperiCad.DigitalDrawingStore.BL.Services.Factories
{
    public interface IFeedbackPropertiesServiceFactory
    {
        IFeedbackPropertiesService CreateFeedbackPropertyService(string applicationConfigurationFilePath);
    }
}
