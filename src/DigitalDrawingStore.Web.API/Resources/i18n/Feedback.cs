using XperiCad.Common.Core.Culture.Resource.Json;
using XperiCad.Common.Core.Feedback;
using XperiCad.Common.Infrastructure.Culture.Resource;
using XperiCad.Common.Infrastructure.Feedback;

namespace XperiCad.DigitalDrawingStore.Web.API.Resources.i18n
{
    internal class Feedback
    {
        private const string LANGUAGE_RESOURCES_PATH = "XperiCad.DigitalDrawingStore.Web.API.Resources.i18n.Commands.hu_HU.FeedbackMessages.json";

        private static ICultureResource _error_TargetOfDocumentUsageIsNull = new JsonCultureResource("Error_TargetOfDocumentUsageIsNull", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_TargetOfDocumentUsageIsNull = new FeedbackResource(Severity.Error, _error_TargetOfDocumentUsageIsNull);
        
        private static ICultureResource _error_ClientUsernameIsNull = new JsonCultureResource("Error_ClientUsernameIsNull", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_ClientUsernameIsNull = new FeedbackResource(Severity.Error, _error_ClientUsernameIsNull);
        
        private static ICultureResource _error_ClientMachineNameIsNull = new JsonCultureResource("Error_ClientMachineNameIsNull", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_ClientMachineNameIsNull = new FeedbackResource(Severity.Error, _error_ClientMachineNameIsNull);
        
        private static ICultureResource _error_ClientIpIsNull = new JsonCultureResource("Error_ClientIpIsNull", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_ClientIpIsNull = new FeedbackResource(Severity.Error, _error_ClientIpIsNull);
        
        private static ICultureResource _error_SideWatermarkPositionIsNull = new JsonCultureResource("Error_SideWatermarkPositionIsNull", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_SideWatermarkPositionIsNull = new FeedbackResource(Severity.Error, _error_SideWatermarkPositionIsNull);

        private static ICultureResource _error_DocumentIdIsEmpty = new JsonCultureResource("Error_DocumentIdIsEmpty", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_DocumentIdIsEmpty = new FeedbackResource(Severity.Error, _error_DocumentIdIsEmpty);

        private static ICultureResource _error_CategoryIdIsEmpty = new JsonCultureResource("Error_CategoryIdIsEmpty", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Error_CategoryIdIsEmpty = new FeedbackResource(Severity.Error, _error_CategoryIdIsEmpty);
        
        private static ICultureResource _fatal_Wrong_Document_Database_Connection_String = new JsonCultureResource("Fatal_Wrong_Document_Database_Connection_String", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Wrong_Document_Database_Connection_String = new FeedbackResource(Severity.Fatal, _fatal_Wrong_Document_Database_Connection_String);

        private static ICultureResource _fatal_Wrong_Email_Format = new JsonCultureResource("Fatal_Wrong_Email_Format", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Wrong_Email_Format = new FeedbackResource(Severity.Fatal, _fatal_Wrong_Email_Format);

        private static ICultureResource _fatal_Not_Number = new JsonCultureResource("Fatal_Not_Number", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Not_Number = new FeedbackResource(Severity.Fatal, _fatal_Not_Number);

        private static ICultureResource _fatal_Port_Number_Not_In_Range = new JsonCultureResource("Fatal_Port_Number_Not_In_Range", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Port_Number_Not_In_Range = new FeedbackResource(Severity.Fatal, _fatal_Port_Number_Not_In_Range);

        private static ICultureResource _information_Successfully_Modified = new JsonCultureResource("Information_Successfully_Modified", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Information_Successfully_Modified = new FeedbackResource(Severity.Information, _information_Successfully_Modified);

        private static ICultureResource _fatal_Couldnt_Modify = new JsonCultureResource("Fatal_Couldnt_Modify", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Couldnt_Modify = new FeedbackResource(Severity.Fatal, _fatal_Couldnt_Modify);

        private static ICultureResource _fatal_Empty_Field = new JsonCultureResource("Fatal_Empty_Field", LANGUAGE_RESOURCES_PATH);
        internal static IFeedbackResource Fatal_Empty_Field = new FeedbackResource(Severity.Fatal, _fatal_Empty_Field);
    }
}
