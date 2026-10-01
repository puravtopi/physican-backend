using Npgsql;
using PhysicianBooking.Services.Implementation;
using PhysicianBooking.Services.Interface;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


// 1. Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://physicianbookingapi20260728210051-amdrd7dpdegsguft.westcentralus-01.azurewebsites.net") // No trailing slashes!
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


// Add services to the container.
builder.Services.AddControllers();

// Database
builder.Services.AddScoped<IDbConnection>(x =>
    new NpgsqlConnection(
        builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddScoped<PlaceRepository>();

// HTTP clients / services
builder.Services.AddHttpClient<GooglePlacesService>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();
builder.Services.AddScoped<ILeadsRepository, LeadsRepository>();
builder.Services.AddScoped<IInquiryRepository, InquiryRepository>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// (previous custom AddOpenApi removed and replaced with Swashbuckle)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhysicianBooking API V1");
        // c.RoutePrefix = string.Empty; // Uncomment to serve Swagger UI at app root (/)
    });
}
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
