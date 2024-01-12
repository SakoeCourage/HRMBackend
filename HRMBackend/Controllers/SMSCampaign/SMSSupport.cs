using HRMBackend.DB;
using HRMBackend.DTO.SMS;
using HRMBackend.Model.SMS;
using HRMBackend.Services.SMS_Service;
using HRMBackend.Types;
using HRMBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMBackend.Controllers.SMSCampaign
{
    public class SMSSupport:Controller
    {
        private readonly Context _context;
        private readonly ISMSService _smsService;
        public SMSSupport(Context context,ISMSService smsService)
        {
            _context = context;
            _smsService = smsService;
        }
        public async Task HandleCampaignWithTemplateFile([FromForm] NewFileTemplateSMSDTO requestData)
        {
            if (!ModelState.IsValid) {
                var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

                if (errors?.Any() == true)
                {
                    throw new Exception(string.Join(",", errors));
                }
            } 
         
            try
            {
                ///////////////////////////////////////////// INITIAL VALIDATIONS ////////////////////////////////////////////////////////////
                var campaignNameExist = await _context.SMSCampaignHistory.AnyAsync(c => c.campaignName == requestData.campaingName);
                if (campaignNameExist) throw new Exception("Campaign Name is already take");

                var smsTemplate = await _context.SMSTemplate.FindAsync(requestData.smsTemplateId);
                if (smsTemplate is null) throw new Exception("SMS Template Not Found");

                var smsReceipients = Contactutilities.GetContactsFromFile(requestData.templateFile);
                if (smsReceipients.Count == 0) throw new Exception("Failed to resolve any contact");

                var anyContactNumberError = smsReceipients.Any(i => string.IsNullOrWhiteSpace(i.contact) || i.contact.Length < 9);
                if (anyContactNumberError) throw new Exception("Failed to resolve some contact(s)");

                var contacts = smsReceipients.Select(i => i.contact).ToList();

                bool hasDuplicateContacts = contacts
                    .GroupBy(contact => contact)
                    .Any(g => g.Count() > 1);
                if (hasDuplicateContacts) throw new Exception("Failed to resolve duplicate contacts");

                var existingApplicantWithSameContacts = await _context.Applicant
                    .Where(a => contacts.Contains(a.contact))
                    .Select(a => a.contact)
                    .ToListAsync();

                if (existingApplicantWithSameContacts.Count > 0)
                {
                    var existingUniqueContact = existingApplicantWithSameContacts.GroupBy(c => c).Select(g => g.Key);
                    throw new Exception($"Failed to resolve already existing contact(s): {string.Join(",", existingUniqueContact)}");
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var newCampaign = new SMSCampaignHistory
                        {
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now,
                            campaignName = requestData.campaingName,
                            smsTemplateId = requestData.smsTemplateId,
                            receipients = smsReceipients.Count()
                        };

                        var newSMSHistory = await _context.SMSCampaignHistory.AddAsync(newCampaign);
                        await _context.SaveChangesAsync();

                        var newCampaignId = newSMSHistory.Entity.id;

                        List<SMSCampaignReceipient> newReceipientsList = smsReceipients
                            .Select(r => new SMSCampaignReceipient
                            {
                                firstName = r.firstName,
                                lastName = r.lastName,
                                contact = r.contact,
                                message = smsTemplate.message,
                                campaignHistoryId = newCampaignId,
                                status = SMSStatus.successfull
                            }).ToList();

                        if (smsTemplate.name == "New Recruitment")
                        {
                            List<HRMBackend.Model.Applicant.Applicant> newApplicantList = smsReceipients.
                                Select(r => new HRMBackend.Model.Applicant.Applicant
                                {
                                    firsName = r.firstName,
                                    lastName = r.lastName,
                                    email = null,
                                    contact = r.contact,
                                    createdAt = DateTime.Now,
                                    updatedAt = DateTime.Now,
                                }).ToList();
                            await _context.Applicant.AddRangeAsync(newApplicantList);
                        }

                        await _context.SMSCampaignReceipient.AddRangeAsync(newReceipientsList);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        _smsService.AddRange(newReceipientsList).SendBatchSMS();

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }


    
        public async Task HandleCampaignWithNoTemplateFile([FromForm]NewNonFileTemplateSMSDTO requestData)
        {
             throw new Exception("Feature Not Available");
        }
    }

}
