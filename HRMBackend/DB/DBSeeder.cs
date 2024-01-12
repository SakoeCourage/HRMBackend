using HRMBackend.DB;
using HRMBackend.Model.SMS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HRMBackend.DB.Factory;

public class DBSeeder
{
    private readonly Context _context;
    private readonly ILogger _logger;
    public DBSeeder(Context context,ILogger logger)
    {
        _context = context;
        _logger = logger;
      
    }

    public void Seed()
    {
        if (!_context.SMSTemplate.Any())
        {
            _logger.LogInformation("Seeding SMS Templates");
            _context.SMSTemplate.AddRange(SMSFactory.Data);
            _context.SaveChanges();
            _logger.LogInformation("Finished..");
        }
    }
}
