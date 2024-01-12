using HRMBackend.Model.SMS;
using HRMBackend.Services.SMS_Service;
using System.Collections;

namespace HRMBackend.DB.Factory
{
    public class SMSFactory : IDBFactory<SMSTemplate>
    {
        public static List<SMSTemplate> Data { get; private set; } = new List<SMSTemplate>() {
        new SMSTemplate{
                name = "New Recruitment",
                message = "Your application has been received. \n Visit link to complete your registeration",
                description = "Template for a new recruitment SMS campaign",
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                readOnly = true
            }
        };        
    }
}
