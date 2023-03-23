using System.Collections.Generic;

namespace DigitalDrawingStore.Listener.Service.Application
{
    internal interface IFeedbackProperties
    {
        string SenderEmail { get; }
        IEnumerable<string> EmailRecipients { get; }
        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; } // TODO: secure string + encoding
        bool IsUseDefaultCredentials { get; }
        bool IsEnableSsl { get; }
    }
}
