using System.Text.Json;
using EmployeeApp.Configs;
using EmployeeApp.Logger;
using EmployeeApp.Repositories;
using EmployeeApp.Repositories.RepositoriesImpl;
using EmployeeApp.Services;
using EmployeeApp.Services.ServicesImpl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

{




    builder.Services.AddDbContext<EmployeeDbContext>(
       options => options.UseSqlServer("Server=DESKTOP-U6G6LIM\\SQLVUONG;Database=EmployeeDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;"));


    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

    builder.Services.Configure<MvcOptions>(options =>
    {
        options.ModelMetadataDetailsProviders.Add(
            new SystemTextJsonValidationMetadataProvider());
    });

    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
    builder.Services.AddScoped<ICompanyService, CompanyService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost4200",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

    builder.Logging.AddDbLogger(options =>
{
    builder.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
});
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
        // Thêm định dạng ngày tháng năm vào TimestampFormat
    });
});


}



WebApplication app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.Logger.LogInformation("Adding Routes");
    app.MapGet("/", () => "Hello World!");
    app.Logger.LogInformation("Starting the app");
    app.UseCors("AllowLocalhost4200");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}


