var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(opt =>
{
    opt.AddDebug();
    opt.AddConsole();
});

// MassTransit service
SetupMassTransit();

// PaymentAPI external HTTP service
AddPaymentApiClient();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// SwaggerUI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database services
var dbConnection = builder.Configuration.GetConnectionString("EdPayRechargeAppDBConnection");
builder.Services.Setup<RechargeAppDBContext>(dbConnection);

// Repository and ApplicationServices
builder.Services.SetupServices();

// Allow Cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", policy =>
{
    policy.WithHeaders("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Enable Cors
app.UseCors((opt) =>
{
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

app.Run();

//Configure and Setup MassTransit service.
void SetupMassTransit()
{
    ServiceSettings serviceSettings = new();
    builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection(nameof(ServiceSettings)));
    builder.Configuration.GetSection(nameof(ServiceSettings)).Bind(serviceSettings);

    RabbitMQSettings rabbitMQSettings = new();
    builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection(nameof(RabbitMQSettings)));
    builder.Configuration.GetSection(nameof(RabbitMQSettings)).Bind(rabbitMQSettings);

    builder.Services.AddMassTransitWithRabbitMq(serviceSettings, rabbitMQSettings); 
}

//Configure and Setup PaymentAPIClient service.
void AddPaymentApiClient()
{
    ApiServiceClients serviceClients = new();
    builder.Services.Configure<ApiServiceClients>(builder.Configuration.GetSection(nameof(ApiServiceClients)));
    builder.Configuration.GetSection(nameof(ApiServiceClients)).Bind(serviceClients);

    var random = new Random();

    using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
        .SetMinimumLevel(LogLevel.Trace).AddConsole());

    var logger = loggerFactory.CreateLogger<PaymentApiClient>();

    builder.Services.AddHttpClient<PaymentApiClient>(client =>
    {
        client.BaseAddress = new Uri(serviceClients.PaymentApiClientUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    })
    .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
        5,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        + TimeSpan.FromMilliseconds(random.Next(0, 1000)),
        onRetry: (outcome, timespan, retryAttempt) =>
        {
            logger.LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
        }
    ))
    .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
        3,
        TimeSpan.FromSeconds(15),
        onBreak: (outcome, timespan) =>
        {
            logger.LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds...");
        },
        onReset: () =>
        {
            logger.LogWarning($"Closing the circuit...");
        }
    ))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
}