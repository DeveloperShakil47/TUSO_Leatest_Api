namespace TUSO.Api.BGService
{
    public class IncidentBackgroundService : BackgroundService
    {
        public bool IsEnabled { get; set; }
        public TimeSpan Second { get; set; } = TimeSpan.FromSeconds(1);
        public int MilliSecond { get; set; }
        private readonly ILogger<IncidentBackgroundService> _logger;
        private readonly IServiceScopeFactory _factory;
        private int _executionCount = 0;

        public IncidentBackgroundService(ILogger<IncidentBackgroundService> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)

        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromHours(12));
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        IncidentService incidentService = asyncScope.ServiceProvider.GetRequiredService<IncidentService>();
                        await incidentService.DoSomethingAsync(MilliSecond);
                        _executionCount++;
                        _logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");
                    }
                    else
                    {
                        _logger.LogInformation("Email Not Sending");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }

    
}