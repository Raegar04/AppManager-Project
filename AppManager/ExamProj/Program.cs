
using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using UI;
using UI.ConsoleManagers;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        var serviceProvider = DependencyRegistration.Register();

        using (var scope = serviceProvider.CreateScope())
        {
            var appManager = scope.ServiceProvider.GetService<UserConsoleManager>();
            appManager.StartProcessAsync().Wait();
        }
    }
}