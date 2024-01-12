using Hangfire;
using HRMBackend.DB;
using HRMBackend.Providers;
using HRMBackend.Services.SMS_Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetValue<string>("siteSettings:appkey");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddSingleton<JwtAuthProvider>(new JwtAuthProvider(key));
//Adding DB Context
builder.Services.AddDbContext<Context>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ISMSService, SMSService>();

builder.Services.AddTransient<Authserviceprovider>(serviceProvider =>
{
    var context = serviceProvider.GetRequiredService<Context>();
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    return new Authserviceprovider(context, httpContextAccessor);
});

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

var app = builder.Build();
// Configure the HTTP request pipeline.

void SeedDatabase() { 
    using (var scope = app.Services.CreateScope())
    try
    {
        var context = scope.ServiceProvider.GetService<Context>();
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        var seeder = new DBSeeder(context, logger);
        seeder.Seed();
    }
    catch
    {
        throw;
    }
}
SeedDatabase();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
