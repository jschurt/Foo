using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solstice.Comms;
using Solstice.Comms.MessageModels;
using Solstice.Members.MemberVoeModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solstice.Members
{
    internal sealed class MemberVoeBusinessLogic : IMemberVoeBusinessLogic
    {
        private readonly MemberVoeBusinessLogicSettings _voeSettings;
        private readonly IMessageBusinessLogic _messageBusinessLogic;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MemberVoeBusinessLogic> _logger;
        public const string HttpClientName_SsrsVoeRequest = "SsrsReportServerClient";

        public MemberVoeBusinessLogic(
            IOptions<MemberVoeBusinessLogicSettings> voeOptions,
            IMessageBusinessLogic messageBusinessLogic,
            IHttpClientFactory httpClientFactory,
            ILogger<MemberVoeBusinessLogic> logger
            )
        {
            _voeSettings = voeOptions.Value;
            _messageBusinessLogic = messageBusinessLogic;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<VoeRequestResult> RequestVoeByFaxAsync(string memberNumber, string faxNumber, int productId, bool inNetwork)
        {
            bool returnResult;
            string returnMessage;

            try
            {
                var isDental = productId == 1;
                if (!isDental)
                {
                    throw new NotImplementedException("Only dental VOE by Fax is supported at this time.");
                }

                var voeUrl = GenerateSsrsUrlForVoeReport(memberNumber, productId, inNetwork);

                EmailMessageAttachment pdfVoeAttachment = null;
                using (var client = _httpClientFactory.CreateClient(HttpClientName_SsrsVoeRequest))
                {
                    using (var response = await client.GetAsync(voeUrl))
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        
                        if (!response.IsSuccessStatusCode || content.Length == 0)
                        {
                            throw new HttpRequestException("VOE could not be generated for this request.");
                        }

                        pdfVoeAttachment = new EmailMessageAttachment
                        {
                            Name = "voe.pdf",
                            Content = content,
                            MimeTypeName = "application/pdf"
                        };
                    }
                }

                await _messageBusinessLogic.SendEmailAsync(
                    new EmailMessage(
                        fromAddress: _voeSettings.EFaxFromEmailAddress,
                        toAddress: $"{faxNumber}@{_voeSettings.EFaxDomain}",
                        subject: _voeSettings.EFaxSubjectLine,
                        body: _voeSettings.EFaxMessageBody,
                        isHtml : false)
                    { 
                        Attachments = new List<EmailMessageAttachment>()
                        {
                            pdfVoeAttachment
                        }
                    });

                returnResult = true;
                returnMessage = "Success";
            }
            catch (Exception e)
            {
                returnResult = false;
                var requestObj = new
                {
                    memberNumber, faxNumber, productId, inNetwork
                };
                returnMessage = $"Error requesting VOE. Request: {requestObj.ToJson()}. Error message: {e.Message}";
                _logger.LogError(e, returnMessage);
            }

            return new VoeRequestResult
            {
                MemberNumber = memberNumber,
                Message = returnMessage,
                Success = returnResult
            };
        }

        private string GenerateSsrsUrlForVoeReport(string memberNumber, int productId, bool inNetwork)
        {
            var voeUrl = $"{_voeSettings.SsrsReportServerBaseUri}?{_voeSettings.SsrsVoeDentalReportPath}&AltID={Uri.EscapeDataString(memberNumber)}&ProductID={productId}&InNetwork={(inNetwork ? "true" : "false")}&UserName=*&rs%3AParameterLanguage=en-US&rs:Format=PDF";
            
            if(!Uri.IsWellFormedUriString(voeUrl,UriKind.Absolute))
            {
                throw new FormatException(nameof(voeUrl));
            }

            return voeUrl;
        }
    }
}
