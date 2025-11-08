namespace Conector.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int MAX_PARALLEL_THREADS = 1;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            MAX_PARALLEL_THREADS = configuration.GetValue<int>("MaxParallelThreads", 1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                for (int i = 0; i < MAX_PARALLEL_THREADS; i++)
                {
                    _ = Task.Run(async () =>
                    {
                        // Simulate work
                        await Task.Delay(5000, stoppingToken);
                        if (_logger.IsEnabled(LogLevel.Information))
                        {
                            _logger.LogInformation("Thread {threadId} completed work at: {time}",
                                Thread.CurrentThread.ManagedThreadId, DateTimeOffset.Now);
                        }
                    }, stoppingToken);
                }
            }
        }
    }
}
