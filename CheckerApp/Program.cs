using CheckerApp.Services;

namespace CheckerApp
{
    internal class Program
    {


        static async Task Main(string[] args)
        {
            var app = new App();
            await app.Run();

            Environment.Exit(0);
        }

    }
}
