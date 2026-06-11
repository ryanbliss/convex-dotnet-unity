using Convex.Client.Infrastructure.Http;
using Convex.Client.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace Convex.Client.Features.RealTime.Pagination;

/// <summary>
/// Fluent builder for creating paginated queries.
/// </summary>
internal sealed class PaginationBuilder<T>: IPaginationBuilder<T>
{
private IHttpClientProvider httpProvider;
private IConvexSerializer serializer;
private string functionName;
private ILogger? logger;
private bool enableDebugLogging;
public PaginationBuilder(IHttpClientProvider httpProvider, IConvexSerializer serializer, string functionName, ILogger? logger = null, bool enableDebugLogging = false)
{
    this._httpProvider = httpProvider;
    this._serializer = serializer;
    this._functionName = functionName;
    this._logger = logger;
    this._enableDebugLogging = enableDebugLogging;
    this.httpProvider = httpProvider;
    this.serializer = serializer;
    this.functionName = functionName;
    this.logger = logger;
    this.enableDebugLogging = enableDebugLogging;
}    private readonly IHttpClientProvider _httpProvider ;
    private readonly IConvexSerializer _serializer ;
    private readonly string _functionName ;
    private readonly ILogger? _logger ;
    private readonly bool _enableDebugLogging ;

    private int _pageSize = 20;
    private object? _args;

    public IPaginationBuilder<T> WithPageSize(int pageSize)
    {
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0");

        _pageSize = pageSize;
        return this;
    }

    public IPaginationBuilder<T> WithArgs<TArgs>(TArgs args) where TArgs : notnull
    {
        _args = args;
        return this;
    }

    public IPaginationBuilder<T> WithArgs<TArgs>(Action<TArgs> configure) where TArgs : class, new()
    {
        var argsInstance = new TArgs();
        configure(argsInstance);
        _args = argsInstance;
        return this;
    }

    public IPaginator<T> Build() => new Paginator<T>(_httpProvider, _serializer, _functionName, _pageSize, _args, _logger, _enableDebugLogging);
}
