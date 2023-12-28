//namespace TUSO.Api.BGService
//{
//    public class EmailBackgroundService : BackgroundService
//    {
//        public bool IsEnabled { get; set; }
//        public TimeSpan Second { get; set; } = TimeSpan.FromSeconds(1);
//        public int MilliSecond { get; set; }
//        private readonly ILogger<EmailBackgroundService> _logger;
//        private readonly IServiceScopeFactory _factory;
//        private int _executionCount = 0;

//        public EmailBackgroundService(ILogger<EmailBackgroundService> logger, IServiceScopeFactory factory)
//        {
//            _logger = logger;
//            _factory = factory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)

//        {
//            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(100));
//            while (
//                !stoppingToken.IsCancellationRequested &&
//                await timer.WaitForNextTickAsync(stoppingToken))
//            {
//                try
//                {
//                    if (IsEnabled)
//                    {
//                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
//                        DeviceEmailService deviceService = asyncScope.ServiceProvider.GetRequiredService<DeviceEmailService>();
//                        await deviceService.DoSomethingAsync(MilliSecond);
//                        _executionCount++;
//                        _logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");
//                    }
//                    else
//                    {
//                        _logger.LogInformation("Email Not Sending");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
//                }
//            }
//        }
//    }
//}