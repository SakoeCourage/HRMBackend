using Hangfire;
using HRMBackend.DB;
using HRMBackend.DTO.Applicant;
using HRMBackend.Services.SMS_Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMBackend.Controllers.Applicant
{
    public class ApplicantController : Controller
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public ApplicantController(Context context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] ApplicationLoginDTO loginDTo)
        {

            // Create a list of SMSDTO objects
            var smsList = new List<SMSDTO>
            {
            new SMSDTO { FullName = "Sakoe Jay", Contact = "+233203843143", Message = "This is a text" },
         
            };

            // Enqueue the background job
           new SMSService().AddRange(smsList).SendBatchSMS();

            return Ok("done");


            //Checking if user hasApplication
            var applicant = await _context.Applicant.FirstOrDefaultAsync(applicant => applicant.contact == loginDTo.contactNumber);
            if (applicant == null) { return NotFound("Applicant Not Found"); }

            //Checking if application has expired
            DateTime applicationCreationDate = applicant.createdAt; 
            bool hasExpired = DateTime.UtcNow.Date - applicationCreationDate.Date > TimeSpan.FromDays(3);
            if (hasExpired) { return UnprocessableEntity("Application expired."); }

            return Ok("done");

        }
    }
}
