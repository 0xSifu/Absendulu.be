using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Middleware;
using AbsenDulu.BE.Services.Authentication;
using AbsenDulu.BE.Services.PasswordHash;
using AbsenDulu.BE.Interfaces.IServices.PasswordHash;
using AbsenDulu.BE.Interfaces.IServices.Authentication;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices.Activation;
using AbsenDulu.BE.Interfaces.IServices.Email;
using AbsenDulu.BE.Services.Email;
using static AbsenDulu.BE.Services.Activation.UserActivationService;
using AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
using AbsenDulu.BE.Services.Subcribes;
using AbsenDulu.BE.Services.LogError;
using AbsenDulu.BE.Interfaces.IServices.LogError;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Caching.Distributed;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Services.CompanyData;
using AbsenDulu.BE.Interfaces.IServices.Attendances;
using AbsenDulu.BE.Services.Attendances;
using AbsenDulu.BE.Interfaces.IServices.Dashboard;
using AbsenDulu.BE.Services.Dashboard;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;
using AbsenDulu.BE.Services.PushNotification;
using AbsenDulu.BE.Interfaces.IServices.ResetPassword;
using AbsenDulu.BE.Services.ResetPassword;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Interfaces.IServices.Approval;
using AbsenDulu.BE.Services.Approval;
using AbsenDulu.BE.Interfaces.IServices.Calender;
using AbsenDulu.BE.Services.Calender;
using AbsenDulu.BE.Interfaces.IServices.Workflows;
using AbsenDulu.BE.Services.Workflows;
using AbsenDulu.BE.Services.Schedules;
using AbsenDulu.BE.Interfaces.IServices.Schedules;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Interfaces.IServices.MenuAccess;
using AbsenDulu.BE.Services.MenuAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("EfPostgresDb"));

    });


builder.Services.AddAuthentication(auth =>
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
        {
            var secretKey = builder.Configuration.GetSection("JwtToken:SigningKey").Value;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetSection("JwtToken:Issuer").Value,
                ValidateAudience = false,
                //ValidAudience = builder.Configuration.GetSection("JwtToken:Audience").Value,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

builder.Services.AddHttpContextAccessor();
// builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IWorkshopService, WorkshopService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IReimbursementService, ReimbursementService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
// Add services to the container.

builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICreatePasswordHashService, CreatePasswordHashService>();

builder.Services.AddScoped<IVerifyPasswordHashService, VerifypasswordHasService>();
builder.Services.AddScoped<TokenConfiguration>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserActivationService, USerActivationService>();
builder.Services.AddScoped<IDetailSubcribeService, DetailSubcribeService>();
builder.Services.AddScoped<ILogErrorService, LogErrorService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IUserResetPasswordService, UserResetPasswordService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
// builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddScoped<IApprovalLogService, ApprovalLogService>();
builder.Services.AddScoped<IGenerateCalenderServices, GenerateCalenderServices>();
builder.Services.AddScoped<IWorkflowsService, WorkflowsService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IEmployeeScheduleService, EmployeeScheduleService>();
builder.Services.AddScoped<IApprovalLeaveService, ApprovalLeaveService>();
builder.Services.AddScoped<IApprovalRemainingService, ApprovalRemainingService>();
builder.Services.AddScoped<IApprovalReimburseService, ApprovalReimburseService>();
builder.Services.AddScoped<IApprovalReimburseLogService, ApprovalReimburseLogService>();
builder.Services.AddScoped<IApprovalWorkShopLogService, ApprovalWorkShopLogService>();
builder.Services.AddScoped<IApprovalWorkShopService, ApprovalWorkShopService>();
builder.Services.AddScoped<IMenuAccessService, MenuAccessService>();
builder.Services.AddScoped<IAvailableAccessService, AvailableAccessService>();






builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Absen Dulu Backend",
            Version = "v1",
            Description = "testing Restfull Api Service Absen Dulu",
            Contact = new OpenApiContact
            {
                Name = "SUPERMAN TEAM",
                Email = "your.email@example.com",
                Url = new Uri("https://example.com")
            }
        });
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthorizeTokenFilterService>();
builder.Services.AddScoped<TokenValidate>();
builder.Services.AddScoped<BalanceLeave>();
builder.Services.AddHealthChecks();
// builder.Services.AddSingleton<TokenConfiguration>();
// builder.Services.AddSingleton<TokenConfiguration>();
// builder.Services.AddSingleton<IUriService>(o =>
// {
//     var accessor = o.GetRequiredService<IHttpContextAccessor>();
//     var request = accessor.HttpContext.Request;
//     var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
//     return new UriService(uri);
// });
// builder.Services.AddElmahIo(o =>
// {
//     o.ApiKey = "a465c251032d4e98902d36c6686f42b3";
//     o.LogId = new Guid("20a2f32a-6015-4c65-94cf-a0c33c8fc042");
// });
builder.Services.AddScoped<ICache>(p =>
{
    ICache cache = CacheManager.GetCache("AbsenduluCache");
    return cache;
});
builder.Services.AddNCacheDistributedCache(configuration =>
         {
             configuration.CacheName = builder.Configuration.GetSection("NCacheConfig:AbsenduluCache").Value;
         });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    // ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthorization();
app.UseMiddleware<MiddlewareAbsendulu>();
app.MapControllers();
app.MapHealthChecks("/health");
// app.UseElmahIo();

app.Run();
