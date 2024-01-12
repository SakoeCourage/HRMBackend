using HRMBackend.DB;
using HRMBackend.DTO.Applicant;
using HRMBackend.Model.Applicant;
using HRMBackend.Providers;
using HRMBackend.Services.SMS_Service;
using HRMBackend.Types.Applicantresponsetypes;
using HRMBackend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMBackend.Controllers.Applicant
{
    [Route("api/applicant")]
    [ApiController]
    public class ApplicantController : Controller
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly ISMSService _smsService;
        private readonly JwtAuthProvider _jwtProvider;
        private readonly Authserviceprovider _authprovider;
 
        public ApplicantController(Context context, Authserviceprovider authprovider,JwtAuthProvider jwtProvider,IConfiguration configuration, ISMSService smsService)
        {
            _context = context;
            _configuration = configuration;
            _smsService = smsService;
            _jwtProvider = jwtProvider;
            _authprovider = authprovider;
        }

        [HttpGet("generate-otp/{contact}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GenerateOTP(string contact)
        {
            //Checking if user hasApplication
            var applicant = await _context.Applicant.FirstOrDefaultAsync(applicant => applicant.contact == contact);
            if (applicant == null) { return NotFound("Your application was not found"); }

            //Checking if application has expired
            DateTime applicationCreationDate = applicant.createdAt;
            bool hasExpired = DateTime.UtcNow.Date - applicationCreationDate.Date > TimeSpan.FromDays(3);
            if (hasExpired) { return UnprocessableEntity("Application expired."); }

            var otp = Stringutilities.GenerateRandomOtp();

            //Checking if applicant has been sent otp earlier
            var contactOtp = await _context.ApplicantHasOTP.FirstOrDefaultAsync(applicant => applicant.contact == contact);

            if (contactOtp != null)
            {
                contactOtp.updatedAt = DateTime.Now;
                contactOtp.otp = otp;
            }
            else {
                await _context.ApplicantHasOTP.AddAsync(new ApplicantHasOTP
                {
                    otp = otp,
                    applicant = applicant,
                    applicantID = applicant.id,
                    contact = contact,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                });
            }
            await _context.SaveChangesAsync();
            var message = SMSMessages.OTPMessage(otp, 10);
            _smsService.SendSMS(contact, message);
            return NoContent();
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Applicantloginresponsetype), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] ApplicantLoginDTO loginData) {

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var hasOTP = await _context
            .ApplicantHasOTP
            .Include(a=>a.applicant)
            .FirstOrDefaultAsync(a => a.contact == loginData.contact && a.otp == loginData.otp);

            if (hasOTP == null) return NotFound("Application not found");

            DateTime OTPCreatedDate = hasOTP.updatedAt;
            bool hasExpired = DateTime.UtcNow.Date - OTPCreatedDate.Date > TimeSpan.FromMinutes(10);

            if (hasExpired) return BadRequest("Otp Has Expired");

            var response = new Applicantloginresponsetype
            {
                accessToken = _jwtProvider.Authenticate(hasOTP.applicant.id, AuthorizationDecisionType.Applicant)
            };
            return Ok(response);
        }

        [Authorize]
        [HttpGet("authenticate")]
        [ProducesResponseType(typeof(HRMBackend.Model.Applicant.Applicant),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authapplicant()
        {
            var authApplicant = await _authprovider.GetAuthenticatedApplicant();
            if (authApplicant == null) return BadRequest("Applicant information not found.");
            return Ok(authApplicant);
        }

   
    }
}
