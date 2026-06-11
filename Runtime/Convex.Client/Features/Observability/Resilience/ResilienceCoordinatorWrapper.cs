using Convex.Client.Infrastructure.Resilience;
using Microsoft.Extensions.Logging;

namespace Convex.Client.Features.Observability.Resilience;

/// <summary>
/// Internal wrapper around ResilienceCoordinator for resilience pattern execution.
/// Combines retry and circuit breaker policies for robust operation execution.
/// </summary>
internal sealed class ResilienceCoordinatorWrapper{
private ILogger? logger;
private bool enableDebugLogging;
public ResilienceCoordinatorWrapper(ILogger? logger = null, bool enableDebugLogging = false)
{
    this._coordinator = new ResilienceCoordinator(null, null, logger, enableDebugLogging);
    this.logger = logger;
    this.enableDebugLogging = enableDebugLogging;
}    private readonly ResilienceCoordinator _coordinator ;

    public RetryPolicy? RetryPolicy
    {
        get => _coordinator.RetryPolicy;
        set => _coordinator.RetryPolicy = value;
    }

    public ICircuitBreakerPolicy? CircuitBreakerPolicy
    {
        get => _coordinator.CircuitBreakerPolicy;
        set => _coordinator.CircuitBreakerPolicy = value;
    }

    public Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        => _coordinator.ExecuteAsync(operation, cancellationToken);

    public Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        => _coordinator.ExecuteAsync(operation, cancellationToken);
}
