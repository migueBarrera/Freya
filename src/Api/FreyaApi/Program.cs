var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Necesitamos ignorar las referencias circulares debido al tringulo compañia/clinicas/empleados, se soluciona ,creo, pasando al modelo completo de response/request
builder.Services.AddControllers()
    .AddJsonOptions(opt => {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.AddSwagger();
builder.ConfigureSerilog();

builder.Services.AddRazorPages();
builder.Services.AddDataBaseConfiguration(builder.Configuration);

builder.Services.AddTokenAuthentication(builder.Configuration);
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);

builder.AddServices();
var app = builder.Build();

//#if DEBUG
//app.UseSwagger();
//app.UseSwaggerUI();
//#endif

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

# if !DEBUG
app.UseHttpsRedirection();
#endif

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
