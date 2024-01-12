using HRMBackend.DTO.SMS;
using HRMBackend.Model.SMS;
using Microsoft.AspNetCore.Mvc;
using HRMBackend.DB;
using Microsoft.EntityFrameworkCore;
using HRMBackend.Utilities;
using HRMBackend.Types;

namespace HRMBackend.Controllers.ApplicationSetup.SMSSetup
{

    [Route("api/appsettings/sms-template")]
    [ApiController]
    public class SMSSetupController : Controller
    {
        private readonly Context _context;
        public SMSSetupController(Context context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaginatedData<SMSTemplate>>> index([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? search = "")
        {
            var smsTemplateQuery = _context.SMSTemplate.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                smsTemplateQuery = smsTemplateQuery
                    .Where(task => task.name.Contains(search))
                    .OrderByDescending(t => t.createdAt);
            }
            var response = PagedList<SMSTemplate>.ToPagedList(smsTemplateQuery, page ?? 1, pageSize ?? 10, this.HttpContext);
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SMSTemplate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> findByID(int id)
        {
            var response = await _context.SMSTemplate.Where(v => v.id == id)
                .FirstOrDefaultAsync();
            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(SMSTemplate), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> createSMSTemplate([FromBody] NewSMSTemplateDTO data)
        {
            if (!ModelState.IsValid) return UnprocessableEntity();
            var exist = await _context.SMSTemplate.AnyAsync(i => i.name.ToLower() == data.name.ToLower());
            if (exist) return UnprocessableEntity(new { errors = new { name = "Name has already been taken" } });
   
            var newData = new SMSTemplate
            {
                name = data.name,
                description = data.description,
                message = data.message,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                readOnly = false
            };
            await _context.SMSTemplate.AddAsync(newData);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(findByID), new { id = newData.id }, newData);
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> update(int id, [FromBody] NewSMSTemplateDTO modifiedTemplate)
        {
            var smsTemplate = await _context.SMSTemplate.FirstOrDefaultAsync(t => t.id == id);
            if (smsTemplate == null) return NotFound();
            if (id != smsTemplate.id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            var exist = await _context.SMSTemplate
             .AnyAsync(i => (i.name.ToLower() == modifiedTemplate.name.ToLower()) && (i.id != id));

            if (exist) return UnprocessableEntity("Name has already been taken");
            if (smsTemplate.readOnly) return UnprocessableEntity($"Unable to make changes to {smsTemplate.name}");
            smsTemplate.description = modifiedTemplate.description;
            smsTemplate.name = modifiedTemplate.name;
            smsTemplate.message = modifiedTemplate.message;
            smsTemplate.updatedAt = DateTime.Now;
            smsTemplate.readOnly = false;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> delete(int id)
        {
            var sms = await _context.SMSTemplate.FindAsync(id);

            if (sms is null) return NotFound();
            if (sms.readOnly) return UnprocessableEntity("Failed to remove template");
            _context.SMSTemplate.Remove(sms);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
