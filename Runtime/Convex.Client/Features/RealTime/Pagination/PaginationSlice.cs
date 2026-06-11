using Convex.Client.Infrastructure.Http;
using Convex.Client.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace Convex.Client.Features.RealTime.Pagination;

/// <summary>
/// Pagination slice - provides cursor-based pagination for Convex queries.
/// This is a self-contained vertical slice that handles all pagination functionality.
/// </summary>
public class PaginationSlice : IConvexPagination
{
private IHttpClientProvider httpProvider;
private IConvexSerializer serializer;
private ILogger? logger;
private bool enableDebugLogging;
public PaginationSlice(IHttpClientProvider httpProvider, IConvexSerializer serializer, ILogger? logger = null, bool enableDebugLogging = false)
{
    this._httpProvider = httpProvider ?? throw new ArgumentNullException(nameof(httpProvider));
    this._serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    this._logger = logger;
    this._enableDebugLogging = enableDebugLogging;
    this.httpProvider = httpProvider;
    this.serializer = serializer;
    this.logger = logger;
    this.enableDebugLogging = enableDebugLogging;
}    private readonly IHttpClientProvider _httpProvider ;
    private readonly IConvexSerializer _serializer ;
    private readonly ILogger? _logger ;
    private readonly bool _enableDebugLogging ;

    /// <summary>
    /// Gets the query.
    /// </summary>
    public IPaginationBuilder<T> Query<T>(string functionName) => new PaginationBuilder<T>(_httpProvider, _serializer, functionName, _logger, _enableDebugLogging);
}
