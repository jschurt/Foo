using Microsoft.Extensions.Logging;
using Solstice.Comms.MessageModels;
using Solstice.Utilities.Email;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.Comms
{
    internal sealed class MessageBusinessLogic : IMessageBusinessLogic
    {
        private readonly SmtpClientWrapper _smtpClientWrapper;
        private readonly ILogger<MessageBusinessLogic> _logger;

        public MessageBusinessLogic(
            SmtpClientWrapper smtpClientWrapper,
            ILogger<MessageBusinessLogic> logger
            )
        {
            _smtpClientWrapper = smtpClientWrapper;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            if (message.ToAddresses.Any(a => !a.IsValidEmailAddress()))
            {
                throw new ArgumentException(nameof(message.ToAddresses));
            }
            if (!message.FromAddress.IsValidEmailAddress())
            {
                throw new ArgumentException(nameof(message.FromAddress));
            }

            foreach (var toAddress in message.ToAddresses)
            {
                using (var mailMessage =
                    new MailMessage(message.FromAddress, toAddress, message.Subject, message.Body)
                    {
                        IsBodyHtml = message.IsHtml,
                        SubjectEncoding = Encoding.UTF8,
                        BodyEncoding = Encoding.UTF8
                    })
                {
                    foreach (var a in message.Attachments)
                    {
                        mailMessage.Attachments.Add(
                            new Attachment(new MemoryStream(a.Content), a.Name)
                            {
                                NameEncoding = Encoding.UTF8,
                                ContentType = new System.Net.Mime.ContentType(a.MimeTypeName)
                            });
                    }

                    const int maxAttemptCount = 3;
                    var attemptCount = 0;

                    retry:
                    try
                    {
                        attemptCount++;
                        await _smtpClientWrapper.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex) when (attemptCount < maxAttemptCount)
                    {
                        var millisecondsDelay = 5000 * attemptCount;
                        var errorMessage = ex.Message;

                        if (ex.InnerException != null)
                        {
                            errorMessage += " -> " + ex.InnerException.Message;
                        }

                        _logger.LogWarning(
                            "Error sending email ({0}) on attempt {1}/{2}. Waiting {3}ms to retry.",
                            errorMessage, attemptCount, maxAttemptCount, millisecondsDelay
                            );

                        await Task.Delay(millisecondsDelay);
                        goto retry;
                    }
                }
            }
        }
    }
}
