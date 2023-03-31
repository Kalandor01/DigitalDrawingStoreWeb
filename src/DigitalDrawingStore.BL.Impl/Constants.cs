namespace XperiCad.DigitalDrawingStore.BL.Impl
{
    public static class Constants
    {
        internal static class ApplicationProperties
        {
            internal const string SETTINGS_NAMESPACE = @"XperiCad\DigitalDrawingStore";
        }
        internal static class FeedbackProperties
        {
            internal const string SENDER_EMAIL_NAME_KEY = "SenderEmail";
            internal const string EMAIL_RECIPIENTS_NAME_KEY = "EmailRecipients";
            internal const string SMTP_HOST_NAME_KEY = "SmtpHost";
            internal const string SMTP_PORT_NAME_KEY = "SmtpPort";
            internal const string SMTP_USERNAME_NAME_KEY = "SmtpUsername";
            internal const string SMTP_PASSWORD_NAME_KEY = "SmtpPassword";
            internal const string IS_USE_DEFAULT_SMTP_CREDENTIALS_NAME_KEY = "UseDefaultSmtpCredentials";
            internal const string IS_SMTP_SSL_ENABLED_NAME_KEY = "EnableSmtpSsl";
        }

        internal static class Documents
        {
            internal static class Resources
            {
                internal const string DEFAULT_CONNECTION_STRING = "Server=(localdb)\\MSSQLLocalDB;Database=DDSW;Trusted_Connection=True;";

                internal static class DatabaseTables
                {
                    internal const string DOCUMENTS_TABLE_NAME_KEY = "Documents";
                    internal const string DOCUMENTS_METADATA_TABLE_NAME_KEY = "DocumentMetadata";
                    internal const string DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY = "DocumentMetadataDefinitions";
                    internal const string DOCUMENT_CATEGORIES_TABLE_NAME_KEY = "DocumentCategories";
                    internal const string DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY = "DocumentCategoryEntities";
                    internal const string APPLICATION_PROPERTIES_TABLE_NAME_KEY = "ApplicationPropertiesTable";
                    internal const string DOCUMENT_USAGES_TABLE_NAME_KEY = "DocumentUsagesTable";
                    internal const string USER_EVENT_LOGS_TABLE_NAME_KEY = "UserEventLogsTable";
                }
            }

            internal static class AttributeKeys
            {
                internal const string DOCUMENT_CATEGORY = "DocumentCategory";
                internal const string DOCUMENT_PATH = "Path";
                internal const string CHANGE_NUMBER = "ChangeNumber";
                internal const string DOCUMENT_NUMBER = "DocumentNumber";
                internal const string DRAWING_NUMBER = "DrawingNumber";
                internal const string LANGUAGE = "Language";
                internal const string PREFIX = "Prefix";
                internal const string REVISION = "Revision";
                internal const string DOCUMENT_TITLE = "DocumentTitle";
                internal const string TYPE_OF_PRODUCT_ON_DRAWING = "TypeOfProductOnDrawing";
                internal const string DOCUMENT_VERSION = "DocumentVersion";
                internal const string DOCUMENT_REVISION_ID = "DocumentRevId";
                internal const string DATE_TIME_OF_DOCUMENT_CREATION = "DateTimeOfDocumentCreation";
                internal const string DATE_TIME_OF_DOCUMENT_APPROVE = "DateTimeOfDocumentApprove";
            }

            internal static class Watermark
            {
                internal const int MIN_FONT_SIZE = 11;
                internal const int MAX_FONT_SIZE = 72;

                internal const int MIN_OFFSET = -100;
                internal const int MAX_OFFSET = 100;

                internal const int MIN_OPACITY = 35;
                internal const int MAX_OPACITY = 100;
            }

            public static class DocumentEventId
            {
                public const string Open = "open";
                public const string Download = "download";
                public const string Print = "print";
            }
        }
    }
}
