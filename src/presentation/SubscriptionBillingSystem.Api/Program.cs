using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Repositories;
using SubscriptionBillingSystem.Infrastructure.Persistence;
using SubscriptionBillingSystem.Infrastructure.Persistence.Repositories;
using SubscriptionBillingSystem.Application.Customers.Commands;
using SubscriptionBillingSystem.Infrastructure.BackgroundJobs;
using Hangfire;
using SubscriptionBillingSystem.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("BillingSystemDb"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IUnitOfWork>(sp =>
    sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<ISubscriptionJobService, SubscriptionJobService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));

builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    jobManager.AddOrUpdate<ISubscriptionJobService>(
        "daily-subscription-check",
        service => service.ProcessExpiredSubscriptions(),
        Cron.Minutely);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Billing API");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();