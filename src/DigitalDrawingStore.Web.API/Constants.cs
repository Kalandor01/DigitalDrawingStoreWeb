namespace XperiCad.DigitalDrawingStore.Web.API
{
    public static class Constants
    {
        internal static class Documents
        {
            internal static class Resources
            {
                public const string APPLICATION_CONFIGURATION_FILE_PATH = @".\Preferences\ApplicationConfiguration.xml";
            }
        }

        public static class Authorization
        {
            public static class Policies
            {
                public const string ADMIN = "AdminPolicy";
                public const string USER = "UserPolicy";
            }
        }
    }
}
