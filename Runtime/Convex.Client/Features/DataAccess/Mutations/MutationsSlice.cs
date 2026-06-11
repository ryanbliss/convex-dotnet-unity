using System.Collections.Concurrent;
using Convex.Client.Infrastructure.Builders;
using Convex.Client.Infrastructure.Http;
using Convex.Client.Infrastructure.Serialization;
using Convex.Client.Infrastructure.Caching;
using Convex.Client.Infrastructure.Internal.Threading;
using Convex.Client.Infrastructure.Telemetry;
using Microsoft.Extensions.Logging;

namespace Convex.Client.Features.DataAccess.Mutations;

/// <summary>
/// Mutations slice - provides write operations for Convex functions.
/// This is a self-contained vertical slice that handles all mutation-related functionality.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MutationsSlice"/> class.
/// </remarks>
/// <param name="httpProvider">The HTTP client provider for making requests.</param>
/// <param name="serializer">The serializer for handling Convex JSON format.</param>
/// <param name="reactiveCache">Optional reactive cache for optimistic updates with subscriber notifications.</param>
/// <param name="invalidateDependencies">Optional callback to invalidate dependent queries.</param>
/// <param name="syncContext">Optional synchronization context for UI thread marshalling.</param>
/// <param name="logger">Optional logger for debug logging.</param>
/// <param name="enableDebugLogging">Whether debug logging is enabled.</param>
public class MutationsSlice{
private IHttpClientProvider httpProvider;
private IConvexSerializer serializer;
private IReactiveCache? reactiveCache;
private Func<string, Task>? invalidateDependencies;
private SyncContextCapture? syncContext;
private ILogger? logger;
private bool enableDebugLogging;
public MutationsSlice(IHttpClientProvider httpProvider, IConvexSerializer serializer, IReactiveCache? reactiveCache = null, Func<string, Task>? invalidateDependencies = null, SyncContextCapture? syncContext = null, ILogger? logger = null, bool enableDebugLogging = false)
{
    this._httpProvider = httpProvider ?? throw new ArgumentNullException(nameof(httpProvider));
    this._serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    this._reactiveCache = reactiveCache;
    this._invalidateDependencies = invalidateDependencies;
    this._syncContext = syncContext;
    this._logger = logger;
    this._enableDebugLogging = enableDebugLogging;
    this.httpProvider = httpProvider;
    this.serializer = serializer;
    this.reactiveCache = reactiveCache;
    this.invalidateDependencies = invalidateDependencies;
    this.syncContext = syncContext;
    this.logger = logger;
    this.enableDebugLogging = enableDebugLogging;
}    private readonly IHttpClientProvider _httpProvider ;
    private readonly IConvexSerializer _serializer ;
    private readonly IReactiveCache? _reactiveCache ;
    private readonly Func<string, Task>? _invalidateDependencies ;
    private readonly SyncContextCapture? _syncContext ;
    private readonly ILogger? _logger ;
    private readonly bool _enableDebugLogging ;

    // Mutation queue for sequential execution (matches convex-js behavior)
    private readonly ConcurrentQueue<PendingMutation> _mutationQueue = new();
    private readonly SemaphoreSlim _queueProcessingLock = new(1, 1);

    /// <summary>
    /// Executes the mutate operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the mutation.</typeparam>
    /// <param name="functionName">The name of the Convex function to mutate.</param>
    /// <param name="middlewareExecutor">Optional middleware executor for intercepting requests.</param>
    /// <returns>A mutation builder for fluent configuration and execution.</returns>
    public IMutationBuilder<TResult> Mutate<TResult>(
        string functionName,
        Func<string, string, object?, TimeSpan?, CancellationToken, Task<TResult>>? middlewareExecutor = null)
    {
        if (string.IsNullOrWhiteSpace(functionName))
            throw new ArgumentException("Function name cannot be null or whitespace.", nameof(functionName));

        return new MutationBuilder<TResult>(
            _httpProvider,
            _serializer,
            functionName,
            _reactiveCache,
            _invalidateDependencies,
            middlewareExecutor,
            _syncContext,
            EnqueueMutationAsync,
            _logger,
            _enableDebugLogging);
    }

    /// <summary>
    /// Enqueues a mutation for sequential execution.
    /// Mutations are processed in order to ensure ordering guarantees.
    /// </summary>
    internal async Task<TResult> EnqueueMutationAsync<TResult>(
        Func<CancellationToken, Task<TResult>> executeMutation,
        CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<TResult>();
        var pendingMutation = new PendingMutation(
            async ct =>
            {
                try
                {
                    var result = await executeMutation(ct);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            },
            cancellationToken);

        _mutationQueue.Enqueue(pendingMutation);
        _ = ProcessMutationQueueAsync();

        return await tcs.Task;
    }

    /// <summary>
    /// Processes the mutation queue sequentially.
    /// Only one queue processing loop runs at a time.
    /// </summary>
    private async Task ProcessMutationQueueAsync()
    {
        // Try to acquire the lock for queue processing
        if (!await _queueProcessingLock.WaitAsync(0))
        {
            // Another thread is already processing the queue
            return;
        }

        try
        {
            while (_mutationQueue.TryDequeue(out var pendingMutation))
            {
                try
                {
                    await pendingMutation.Execute(pendingMutation.CancellationToken);
                }
                catch (Exception ex)
                {
                    // Errors are handled by the TaskCompletionSource in EnqueueMutationAsync
                    // Log the error for observability, then continue processing the next mutation
                    if (ConvexLoggerExtensions.IsDebugLoggingEnabled(_logger, _enableDebugLogging))
                    {
                        _logger!.LogDebug(ex, "Error processing mutation in queue: {ExceptionType}: {Message}", ex.GetType().Name, ex.Message);
                    }
                }
            }
        }
        finally
        {
            _ = _queueProcessingLock.Release();
        }
    }

    /// <summary>
    /// Executes the pending mutation operation.
    /// </summary>
    private sealed class PendingMutation    {
private Func<CancellationToken, Task> execute;
private CancellationToken cancellationToken;
public PendingMutation(Func<CancellationToken, Task> execute, CancellationToken cancellationToken)
{
    this.Execute = execute;
    this.CancellationToken = cancellationToken;
    this.execute = execute;
    this.cancellationToken = cancellationToken;
}        public Func<CancellationToken, Task> Execute { get; }         public CancellationToken CancellationToken { get; }     }
}
