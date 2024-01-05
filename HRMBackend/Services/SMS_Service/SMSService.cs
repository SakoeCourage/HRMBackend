using Hangfire;
using System;
using System.Net.Http;

namespace HRMBackend.Services.SMS_Service
{

    public class SMSDTO
    {
        public string FullName { get; set; }
        public string Message { get; set; }
        public string Contact { get; set; }
    }
    public class SMSService : ISMSService
    {
        private List<SMSDTO> batchSMSList;
        private String apiKey = "SlFIbWtwVHNZWWZrcmpLSHhaTEg";
        private readonly IConfiguration _configuration;

        public SMSService()
        {
            batchSMSList = new List<SMSDTO>();
       
        }
        public ISMSService AddBatchSMS(string fullName, string message, string contact)
        {
            batchSMSList.Add(new SMSDTO
            {
                FullName = fullName,
                Message = message,
                Contact = contact
            });
            return this;
        }

        public ISMSService AddRange(List<SMSDTO> list)
        {
            batchSMSList.AddRange(list);
            return this;
        }

        public ISMSService SendBatchSMS()
        {
            // Synchronously execute the background job
            BackgroundJob.Enqueue(() => SendBatchSMSJob());

            return this;
        }

        public async Task SendBatchSMSJob()
        {
            using (HttpClient client = new HttpClient())
            {
                foreach (var sms in batchSMSList)
                {
                    var queryParams = $"api_key={apiKey}&to={sms.Contact}&from=Test System&sms={sms.Message}";
                    var url = $"https://sms.arkesel.com/sms/api?action=send-sms&{queryParams}";

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("SMS Sent successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Error sending SMS. HTTP Status Code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }
        public ISMSService SendSMS()
        {
            Console.WriteLine($"Sending SMS to {batchSMSList.First().FullName} ({batchSMSList.First().Contact}): {batchSMSList.First().Message}");
            return this;
        }

       
    }
}
