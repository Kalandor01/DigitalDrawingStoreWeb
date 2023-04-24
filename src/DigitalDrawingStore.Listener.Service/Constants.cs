namespace DigitalDrawingStore.Listener.Service
{
    internal static class Constants
    {

        internal static class DocumentDatabase
        {
            internal const string DOCUMENTS_TABLE_NAME_KEY = "Documents";
            internal const string DOCUMENTS_METADATA_TABLE_NAME_KEY = "DocumentMetadata";
            internal const string DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY = "DocumentMetadataDefinitions";
            internal const string DOCUMENT_CATEGORIES_TABLE_NAME_KEY = "DocumentCategories";
            internal const string DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY = "DocumentCategoryEntities";
            public const string APPLICATION_PROPERTIES_TABLE_NAME_KEY = "ApplicationPropertiesTable";
            public const string APPLICATION_PROPERTIES_DICTIONARY_TABLE_NAME_KEY = "ApplicationPropertiesDictionaryTable";
            internal const string USER_EVENT_LOGS_TABLE_NAME_KEY = "UserEventLogsTable";
        }

        internal static class RawDocumentAttributeNames
        {
            internal const string DOCUMENT_CATEGORY = "DocumentCategory";
            internal const string DOCUMENT_PATH = "DocumentPath";
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
    }
}
