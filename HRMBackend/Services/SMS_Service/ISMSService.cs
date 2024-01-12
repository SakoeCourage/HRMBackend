using HRMBackend.Model.SMS;

namespace HRMBackend.Services.SMS_Service
{
    public interface ISMSService
    {
        ISMSService AddToBatchSMS(SMSCampaignReceipient recipientData);
        ISMSService AddRange(List<SMSCampaignReceipient> list);
        void SendBatchSMS();
        void SendSMS(string contact, string message);
    }
}
