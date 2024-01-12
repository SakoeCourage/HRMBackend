using HRMBackend.DB;
using HRMBackend.DTO.SMS;
using HRMBackend.Model.SMS;
using HRMBackend.Services.SMS_Service;
using HRMBackend.Types;
using HRMBackend.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HRMBackend.Controllers.SMSCampaign
{
    [Route("api/sms-campaign")]
    [ApiController]
    public class SMSCampaignController : Controller
    {

        private readonly Context _context;
        private readonly ISMSService _smsService;
        private readonly ILogger<SMSCampaignController> _logger;
        public SMSCampaignController(Context context, ISMSService smsService, ILogger<SMSCampaignController> logger)
        {
            _context = context;
            _smsService = smsService;
            _logger = logger;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedData<SMSCampaignHistory>>> Index([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? search = "")
        {
            var smsHistoryQuery = _context.SMSCampaignHistory.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                smsHistoryQuery = smsHistoryQuery

                            .Where(q => q.campaignName.Contains(search))
                           .OrderByDescending(t => t.createdAt);
            }
            var response = PagedList<SMSCampaignHistory>.ToPagedList(smsHistoryQuery.Include(q => q.smsTemplate), page ?? 1, pageSize ?? 10, this.HttpContext);

            return Ok(response);

        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SMSCampaignHistory>> Get(int id)
        {
            var response = await _context.SMSCampaignHistory
                .Include(q => q.smsTemplate)
                .Include(q => q.smsReceipients)
                .FirstOrDefaultAsync(q => q.id == id);
            if (response is null) return NotFound();   
            return Ok(response);

        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> NewCampaign([FromForm] NewSMSCampaignDTO requestData)
        {
            if (requestData is null)
            {
                return BadRequest("Invalid Request");
            }

            if (ModelState.IsValid == false) return (UnprocessableEntity(ModelState));

            _logger.LogInformation($"Received request: new campaign request");
            if (requestData.templateFile is null)
            {
                return BadRequest(new { error = "Feature not available" });
            }
            if (requestData.templateFile != null)
            {
                try
                {
                    var filtemplateDto = new NewFileTemplateSMSDTO
                    {
                        campaingName = requestData.campaingName,
                        smsTemplateId = requestData.smsTemplateId,
                        templateFile = requestData.templateFile,
                        message = requestData?.message,
                        frequency = requestData?.frequency
                    };
                    await new SMSSupport(_context, _smsService).HandleCampaignWithTemplateFile(filtemplateDto);
                }
                catch (Exception ex)
                {
                    return UnprocessableEntity(ex.Message);
                }

            }
            return NoContent();
        }
    }
}
