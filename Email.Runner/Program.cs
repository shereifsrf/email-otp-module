using EmailOTP.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Email.Runner;

using IHost host = CreateHostBuilder(args).Build();

var app = ActivatorUtilities.CreateInstance<App>(host.Services);
await app.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddTransient<ISendService, SendService>();
            services.AddTransient<App>();
        });