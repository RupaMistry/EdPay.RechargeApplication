var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(opt =>
{
    opt.AddDebug();
    opt.AddConsole();
}); 

// MassTransit service
SetupMassTransit();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// SwaggerUI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database services
var dbConnection = builder.Configuration.GetConnectionString("EdPayBankingAppDBConnection");
builder.Services.Setup<PaymentAppDBContext>(dbConnection);

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