using Api.Repositories;
using Api.Extensions.MiddlewareExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    // An in memory database works for the sake of the demo, but if this were not just a demo,
    // we would use a connection string to point to whatever database name we want to use.
    options.UseInMemoryDatabase("InMemoryDbName");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowLocalhost,
        policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

// Decided to put the Dependency Injection configurations in their own extension methods.
// As an application grows, and we introduce more and more dependencies, this can bloat the middleware.
// So, I thought housing those configurations in their own dedicated method would abstract that logic out, & have a more readable middleware pipeline.
builder.Services
    .AddRepositoryDependencies()
    .AddServiceDependencies();

var app = builder.Build();

// Since we are using an in memory database for this demo project, migrations are not necessary.
// A consequence of not using migrations is that we need to ensure the in memory database is created and seeded
// with the data specified in the OnModelCreating method of the DatabaseContext.
// This code will ensure our database is created, and will properly seed it.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
