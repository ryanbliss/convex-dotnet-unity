namespace Convex.Client.Infrastructure.Middleware;

/// <summary>
/// Executes the convex request operation.
/// Used by middleware to inspect and transform requests.
/// </summary>
/// <remarks>
/// Executes the convex request operation.
/// </remarks>
/// <param name="functionName">The function name.</param>
/// <param name="method">The request method (query, mutation, or action).</param>
/// <param name="args">Optional arguments.</param>
/// <param name="cancellationToken">Optional cancellation token.</param>
public sealed class ConvexRequest{
private string functionName;
private string method;
private object? args;
private CancellationToken cancellationToken;
public ConvexRequest(string functionName, string method, object? args = null, CancellationToken cancellationToken = default)
{
    this.FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
    this.Method = method ?? throw new ArgumentNullException(nameof(method));
    this.Args = args;
    this.CancellationToken = cancellationToken;
    this.functionName = functionName;
    this.method = method;
    this.args = args;
    this.cancellationToken = cancellationToken;
}    /// <summary>
    /// Gets or sets the argument null exception.
    /// </summary>
    public string FunctionName { get; set; } 
    /// <summary>
    /// Gets or sets the argument null exception.
    /// </summary>
    public string Method { get; set; } 
    /// <summary>
    /// Gets or sets the args.
    /// Null if no arguments are provided.
    /// </summary>
    public object? Args { get; set; } 
    /// <summary>
    /// Gets the string.
    /// Middleware can store custom data here for passing information between middleware layers.
    /// </summary>
    public Dictionary<string, object?> Metadata { get; } = new global::System.Collections.Generic.Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Gets or sets the cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; set; } 
    /// <summary>
    /// Executes the clone operation.
    /// Useful for middleware that needs to modify the request without affecting the original.
    /// </summary>
    public ConvexRequest Clone()
    {
        var clone = new ConvexRequest(FunctionName, Method, Args, CancellationToken)
        {
            Timeout = Timeout
        };

        foreach (var kvp in Metadata)
        {
            clone.Metadata[kvp.Key] = kvp.Value;
        }

        return clone;
    }
}
