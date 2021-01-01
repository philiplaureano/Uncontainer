using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Uncontainer;


class Program
{
    static void Main(string[] args)
    {
        return;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseLightInject()
            .ConfigureServices(ConfigureServices);
}