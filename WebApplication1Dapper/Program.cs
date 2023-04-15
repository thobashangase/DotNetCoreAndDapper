using WebApplication1Dapper.Services;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Add services to the container.
builder.Services.AddScoped<IPeopleService, PeopleService>();

builder.Services.AddControllersWithViews();

builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration.Enrich.FromLogContext().
        Enrich.WithMachineName().
        WriteTo.Console().
        WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostContext.Configuration.GetValue<string>("ElasticConfiguration:Uri")))
        {
            IndexFormat = $"{hostContext.Configuration.GetValue<string>("ApplicationName")}-logs-{hostContext.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1
        }).
        Enrich.WithProperty("Environment", hostContext.HostingEnvironment.EnvironmentName).
        ReadFrom.Configuration(hostContext.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
