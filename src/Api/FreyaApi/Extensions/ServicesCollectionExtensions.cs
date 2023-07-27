namespace FreyaApi.Extensions;

public static class ServicesCollectionExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        return builder;
    }
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen(c =>
        {
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.CustomSchemaIds(type => type.ToString());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });

        return builder;
    }
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<JwtSecurityTokenService>();
        builder.Services.UsePaymentsCore(builder.Configuration);
        builder.Services.UseFreyaRepository();
        builder.Services.ConfigureWasabiUploaderServices(builder.Configuration);
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<MultimediaService>();
        builder.Services.AddTransient<ClientControllerService>();
        builder.Services.AddTransient<ClinicControllerService>();
        builder.Services.AddTransient<TokenControllerService>();
        builder.Services.AddTransient<AppManagerControllerService>();
        builder.Services.AddTransient<FAQControllerService>();
        builder.Services.AddTransient<CompaniesControllerService>();
        builder.Services.AddTransient<MultimediaControllerService>();
        builder.Services.AddTransient<EmployeeControllerServices>();
        builder.Services.AddTransient<SubscriptionPlanControllerServices>();
        builder.Services.AddTransient<SubscriptionProductControllerServices>();
        builder.Services.AddTransient<SubscriptionPaymentControllerService>();
        builder.Services.AddTransient<PaymentsControllerService>();
        builder.Services.AddTransient<HelperControllerService>();
        builder.Services.AddTransient<StripeWebhookControllerService>();

        return builder;
    }
}
