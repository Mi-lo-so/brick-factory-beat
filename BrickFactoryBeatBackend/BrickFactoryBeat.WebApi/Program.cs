using System.Reflection;
using BrickFactoryBeat.Application.Services;
using BrickFactoryBeat.Infrastructure.DependencyInjection;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    // ignore circular references for the sake of the SQL db
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});*/ //use jsonIgnore instead
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BrickFactoryBeatAPI", Version = "v1" });

    // Include XML comments (enable GenerateDocumentationFile in csproj)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IEquipmentService, EquipmentService>();


// Enable CORS so the REACT app can access the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();