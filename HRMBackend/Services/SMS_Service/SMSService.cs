using Azure;
using Hangfire;
using HRMBackend.DB;
using HRMBackend.Model.SMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace HRMBackend.Services.SMS_Service
{


    public class SMSService : ISMSService
    {
        private List<SMSCampaignReceipient> batchSMSList;
        private String apiKey;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ISMSService> _logger;
        private Context _context;
        private string appName;

        public SMSService(ILogger<ISMSService> logger, IConfiguration configuration, Context context)
        {
            batchSMSList = new List<SMSCampaignReceipient>();
            _logger = logger;
            _configuration = configuration;
            _context = context;
            apiKey = _configuration.GetValue<string>("siteSettings:arkeselClientKey");
            appName = _configuration.GetValue<string>("siteSettings:AppName");
        }
        public ISMSService AddToBatchSMS(SMSCampaignReceipient recipientData)
        {
            batchSMSList.Add(recipientData);
            return this;
        }

        public ISMSService AddRange(List<SMSCampaignReceipient> list)
        {
            batchSMSList.AddRange(list);
            return this;
        }

        public async void SendBatchSMS()
        {
            await SendBatchSMSJob();
        }

        //[AutomaticRetry(Attempts = 3)]
        public async Task SendBatchSMSJob()
        {
            _logger.LogInformation("Batch SMS Job Has Started...");

            foreach (var sms in batchSMSList)
            {
                BackgroundJob.Enqueue(() => SendSMSAsync(apiKey, sms.contact,sms.message));
                //await SendSMSAsync(apiKey, sms);
            }
            _logger.LogInformation("Batch SMS Job completed.");
        }

        public async Task SendSMSJoB(string contact, string message)
        {
            _logger.LogInformation("SMS Job Has Started...");
            await SendSMSAsync(apiKey, contact,message);
            _logger.LogInformation("SMS Job completed");

        }

        public async Task SendSMSAsync(string apiKey, string contact, string message)
        {
            using (HttpClient client = new HttpClient())
            {   

                var queryParams = $"api_key={apiKey}&to={contact}&from={appName}&sms={message}";
                var url = $"https://sms.arkesel.com/sms/api?action=send-sms&{queryParams}";
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {  
                        _logger.LogInformation("SMS Sent successfully.");
                    }
                    else
                    {
                        _logger.LogError($"Error sending SMS. HTTP Status Code: {response.StatusCode}.");
                    }
                }
                catch (Exception ex)
                {

                    _logger.LogError($"An error occurred: {ex.Message}");

                }
            }
        }

        [AutomaticRetry(Attempts = 3)]
        public void SendSMS(string contact, string message)
        {
            BackgroundJob.Enqueue(() => this.SendSMSJoB(contact, message));
        }


      }
}
