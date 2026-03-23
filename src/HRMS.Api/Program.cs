using HRMS.Api;
using HRMS.Api.Filters;
using HRMS.Core.ConfigOptions;
using HRMS.Core.Interfaces.Admin;
using HRMS.Core.Interfaces.Auth;
using HRMS.Data;
using HRMS.Data.Services.Admin;
using HRMS.Data.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

// 1. THÊM 3 THƯ VIỆN NÀY ĐỂ XỬ LÝ JWT TOKEN
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HRMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Business services
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity configuration
builder.Services.Configure<JwtTokenSetting>(builder.Configuration.GetSection("JwtTokenSetting"));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HRMSContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["JwtTokenSetting:Audience"],
        ValidIssuer = builder.Configuration["JwtTokenSetting:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JwtTokenSetting:Key"]
        ))
    };
});
builder.Services.AddControllers();

// Swagger Configuration (Đã chuẩn hóa về v1)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });

    c.SwaggerDoc("AdminApi", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API for Administrators",
        Description = "API for HRMS core domain."
    });
    c.ParameterFilter<SwaggerNullableParameterFilter>();
});

// Cors Configuration cho Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("AdminApi/swagger.json", "AdminApi");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });
}

app.UseCors("AllowAngular");

app.UseStaticFiles();
app.UseHttpsRedirection();

// 3KHAI BÁO MIDDLEWARE AUTHENTICATION 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seeding data
app.MigrateDatabase();

app.Run();