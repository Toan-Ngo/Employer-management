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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HRMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//
builder.Services.AddScoped<DataSeeder>();
// Business services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



// Identity configuration
builder.Services.Configure<JwtTokenSetting>(builder.Configuration.GetSection("JwtTokenSetting"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services
.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<HRMSContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
    c.SwaggerDoc("AdminAPI", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API for Administrators",
        Description = "API for CMS core domain. This domain keeps track of campaigms,..."
    });
    c.ParameterFilter<SwaggerNullableParameterFilter>();
});
// dùng để connect tới fronend angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("AdminAPI/swagger.json", "AdminAPI");
        c.DisplayOperationId();
        c.DisplayRequestDuration(); 
    });
}


app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// Seeding data
app.MigrateDatabase();

app.Run();
