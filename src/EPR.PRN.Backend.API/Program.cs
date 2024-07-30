namespace EPR.PRN.Backend
{
    using EPR.PRN.Backend.API;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
       {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}