namespace HRMBackend.Services.SMS_Service
{
    public interface ISMSService
    {


        ISMSService AddBatchSMS(string fullName, string message, string contact);
        ISMSService SendBatchSMS();
        ISMSService SendSMS();
        ISMSService AddRange(List<SMSDTO> list);
    }
}
