using EmployeeApp.Configs;
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
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}


