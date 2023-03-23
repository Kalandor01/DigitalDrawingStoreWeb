using System.Reflection;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Culture;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    public class DdswGeneralApplicationProperties : IGeneralApplicationProperties
    {
        #region Constants
        private const string DEFAULT_APPLICATON_CONFIGURATION_FILE_PATH = @".\Preferences\ApplicationConfiguration.xml";
        #endregion

        #region Fields
        private readonly IGeneralApplicationProperties _generalApplicationProperties;
        #endregion

        #region Properties
        public ICultureInformation SelectedCulture
        {
            get => _generalApplicationProperties.SelectedCulture;
            set => _generalApplicationProperties.SelectedCulture = value;
        }

        private string _applicationConfigurationFilePath;
        public string ApplicationConfigurationFilePath
        {
            get => GetApplicationFilePath();
            set => SetApplicationFilePath(value);
        }
        #endregion

        #region ctor
        public DdswGeneralApplicationProperties(IGeneralApplicationProperties generalApplicationProperties)
        {
            _generalApplicationProperties = generalApplicationProperties ?? throw new ArgumentNullException(nameof(generalApplicationProperties));
            _applicationConfigurationFilePath = DEFAULT_APPLICATON_CONFIGURATION_FILE_PATH;
        }
        #endregion

        #region Private members
        private string GetApplicationFilePath()
        {
            if (string.IsNullOrWhiteSpace(_applicationConfigurationFilePath))
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var path = Uri.UnescapeDataString(new UriBuilder(location).Path);
                var dir = Path.GetDirectoryName(path);

                if (dir != null)
                {
                    _applicationConfigurationFilePath = Path.Combine(dir, "Preferences", "ApplicationConfiguration.xml");
                }

                _applicationConfigurationFilePath = DEFAULT_APPLICATON_CONFIGURATION_FILE_PATH;
            }

            return _applicationConfigurationFilePath;
        }

        private void SetApplicationFilePath(string value)
        {
            _applicationConfigurationFilePath = value;
        }
        #endregion
    }
}
