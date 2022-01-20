//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Text.Json;
//using Microsoft.Extensions.Configuration;

//namespace App.WindowsService
//{

//    public sealed class WindowsBackgroundService : BackgroundService
//    {
//        private readonly provaServizio _provaServizio;
//        private readonly ILogger<WindowsBackgroundService> _logger;

//        public WindowsBackgroundService(
//            provaServizio provaServizio,
//            ILogger<WindowsBackgroundService> logger) =>
//            (_provaServizio, _logger) = (provaServizio, logger);

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            int cicle = 1;
//            //inizializzazione variabili:
//            provaServizio.EmailConfig emailConfig = ConfigReader();
//            while (!stoppingToken.IsCancellationRequested)
//            {                
//                cicle += 1;
//            }
//        }
//        static provaServizio.EmailConfig ConfigReader()
//        {
//            var config = new ConfigurationBuilder()
//                //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//                .AddJsonFile("appsettings.json").Build();

//            var section = config.GetSection(nameof(provaServizio.EmailConfig));
//            var emailConfig = section.Get<provaServizio.EmailConfig>();

//            return emailConfig;
//        }
//    }
//}