namespace HRMBackend.Services.SMS_Service
{
    public class SMSMessages
    {

        public static string OTPMessage(string opt, int timeoutMins) {
            return $"Your otp is {opt}. Valid for only {timeoutMins}mins";
        }
    }
}
