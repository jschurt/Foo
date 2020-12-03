using System.Collections.Generic;

namespace Solstice.Comms.MessageModels
{
    public class EmailMessage
    {
        public string FromAddress { get; set; }
        public List<string> ToAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public List<EmailMessageAttachment> Attachments { get; set; }

        public EmailMessage() { }

        public EmailMessage(string fromAddress, string toAddress, string subject, string body, bool isHtml)
            : base()
        {
            FromAddress = fromAddress;
            ToAddresses = new List<string> { toAddress };
            Subject = subject;
            Body = body;
            IsHtml = IsHtml;
        }
    }
}
