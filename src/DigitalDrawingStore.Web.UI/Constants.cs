namespace DigitalDrawingStore.Web.UI
{
    public static class Constants
    {
        public static class Documents
        {
            public static class Resources
            {
                public const string APPLICATION_CONFIGURATION_FILE_PATH = @".\Preferences\ApplicationConfiguration.xml";
            }
        }
        public static class Autorization
        {
            public static class Policies
            {
                public const string ADMIN = XperiCad.DigitalDrawingStore.Web.API.Constants.Authorization.Policies.ADMIN;
                public const string USER = XperiCad.DigitalDrawingStore.Web.API.Constants.Authorization.Policies.USER;
            }
        }
    }
}
