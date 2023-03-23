using XperiCad.Common.Core.Culture.Resource.Json;
using XperiCad.Common.Core.Feedback;
using XperiCad.Common.Infrastructure.Culture.Resource;
using XperiCad.Common.Infrastructure.Feedback;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n
{
    internal class Feedback
    {
        private const string LANGUAGE_QUERY_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Documents.Queries.hu_HU.FeedbackMessages.json";
        private const string LANGUAGE_COMMAND_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Documents.Commands.hu_HU.FeedbackMessages.json";

        #region Queries

        //TODO: investigate why error severity won't work
        #region Errors
        private static ICultureResource _error_NoSuchDocumentFoundInDatabase = new JsonCultureResource("Error_NoSuchDocumentFoundInDatabase", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Error_NoSuchDocumentFoundInDatabase = new FeedbackResource(Severity.Fatal, _error_NoSuchDocumentFoundInDatabase);

        //NOTE: needs parameter containing table name
        private static ICultureResource _error_NoSuchTableFoundInDatabase = new JsonCultureResource("Error_NoSuchTableFoundInDatabase", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Error_NoSuchTableFoundInDatabase = new FeedbackResource(Severity.Fatal, _error_NoSuchTableFoundInDatabase);

        private static ICultureResource _error_NoTargetOfDocumentUsageFoundInDatabase = new JsonCultureResource("Error_NoTargetOfDocumentUsageFoundInDatabase", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Error_NoTargetOfDocumentUsageFoundInDatabase = new FeedbackResource(Severity.Fatal, _error_NoTargetOfDocumentUsageFoundInDatabase);

        private static ICultureResource _error_CouldNotQueryBecauseOfInvalidGuid = new JsonCultureResource("Error_CouldNotQueryBecauseOfInvalidGuid", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotQueryBecauseOfInvalidGuid = new FeedbackResource(Severity.Fatal, _error_CouldNotQueryBecauseOfInvalidGuid);

        private static ICultureResource _error_CouldNotQueryParameter = new JsonCultureResource("Error_CouldNotQueryParameter", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotQueryParameter = new FeedbackResource(Severity.Fatal, _error_CouldNotQueryParameter);
        #endregion

        #region Warnings
        private static ICultureResource _warning_DocumentCouldNotBeProcessed = new JsonCultureResource("Warning_DocumentCouldNotBeProcessed", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Warning_DocumentCouldNotBeProcessed = new FeedbackResource(Severity.Warning, _warning_DocumentCouldNotBeProcessed);

        private static ICultureResource _warning_TargetOfDocumentUsageCouldNotBeProcessed = new JsonCultureResource("Warning_TargetOfDocumentUsageCouldNotBeProcessed", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Warning_TargetOfDocumentUsageCouldNotBeProcessed = new FeedbackResource(Severity.Warning, _warning_TargetOfDocumentUsageCouldNotBeProcessed);

        private static ICultureResource _warning_DocumentMetadataCouldNotBeProcessed = new JsonCultureResource("Warning_DocumentMetadataCouldNotBeProcessed", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Warning_DocumentMetadataCouldNotBeProcessed = new FeedbackResource(Severity.Warning, _warning_DocumentMetadataCouldNotBeProcessed);

        private static ICultureResource _warning_DocumentCategoryCouldNotBeProcessed = new JsonCultureResource("Warning_DocumentCategoryCouldNotBeProcessed", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Warning_DocumentCategoryCouldNotBeProcessed = new FeedbackResource(Severity.Warning, _warning_DocumentCategoryCouldNotBeProcessed);

        private static ICultureResource _warning_DocumentMetadataDefinitionCouldNotBeProcessed = new JsonCultureResource("Warning_DocumentMetadataDefinitionCouldNotBeProcessed", LANGUAGE_QUERY_RESOURCES_PATH);
        internal static IFeedbackResource Warning_DocumentMetadataDefinitionCouldNotBeProcessed = new FeedbackResource(Severity.Warning, _warning_DocumentMetadataDefinitionCouldNotBeProcessed);
        #endregion

        #endregion

        #region Commands

        #region Fatals
        private static ICultureResource _fatal_database = new JsonCultureResource("Error_CouldNotUpdateBecauseUnknown", "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Watermark.hu_HU.FeedbackMessages.json");
        internal static IFeedbackResource Fatal_database = new FeedbackResource(Severity.Fatal, _fatal_database);
        #endregion

        //TODO: investigate why error severity won't work
        #region Errors
        private static ICultureResource _error_CouldNotUpdateBecauseGivenIdIsInvalid = new JsonCultureResource("Error_CouldNotUpdateBecauseGivenIdIsInvalid", LANGUAGE_COMMAND_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotUpdateBecauseGivenIdIsInvalid = new FeedbackResource(Severity.Fatal, _error_CouldNotUpdateBecauseGivenIdIsInvalid);

        private static ICultureResource _error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid = new JsonCultureResource("Error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid", LANGUAGE_COMMAND_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid = new FeedbackResource(Severity.Fatal, _error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid);

        private static ICultureResource _error_CouldNotUpdateBecauseCategoryEntitiesAreInvalid = new JsonCultureResource("Error_CouldNotUpdateBecauseCategoryEntitiesAreInvalid", LANGUAGE_COMMAND_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotUpdateBecauseCategoryEntitiesAreInvalid = new FeedbackResource(Severity.Fatal, _error_CouldNotUpdateBecauseCategoryEntitiesAreInvalid);

        private static ICultureResource _error_CouldNotUpdateBecauseMetadataDefinitionsAreInvalid = new JsonCultureResource("Error_CouldNotUpdateBecauseMetadataDefinitionsAreInvalid", LANGUAGE_COMMAND_RESOURCES_PATH);
        internal static IFeedbackResource Error_CouldNotUpdateBecauseMetadataDefinitionsAreInvalid = new FeedbackResource(Severity.Fatal, _error_CouldNotUpdateBecauseMetadataDefinitionsAreInvalid);

        private static ICultureResource _error_DocumentIsNotExists = new JsonCultureResource("Error_DocumentIsNotExists", "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Watermark.hu_HU.FeedbackMessages.json");
        internal static IFeedbackResource Error_DocumentIsNotExists = new FeedbackResource(Severity.Fatal, _error_DocumentIsNotExists);

        private static ICultureResource _error_DocumentExtensionIsNotSupported = new JsonCultureResource("Error_DocumentExtensionIsNotSupported", "XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n.Watermark.hu_HU.FeedbackMessages.json");
        internal static IFeedbackResource Error_DocumentExtensionIsNotSupported = new FeedbackResource(Severity.Fatal, _error_DocumentExtensionIsNotSupported);
        #endregion

        #region Informations
        //private static ICultureResource _error_NoSuchDocumentFoundInDatabase = new JsonCultureResource("Error_NoSuchDocumentFoundInDatabase", LANGUAGE_COMMAND_RESOURCES_PATH);
        //internal static IFeedbackResource Error_NoSuchDocumentFoundInDatabase = new FeedbackResource(Severity.Error, _error_NoSuchDocumentFoundInDatabase);

        #endregion

        #endregion
    }
}
