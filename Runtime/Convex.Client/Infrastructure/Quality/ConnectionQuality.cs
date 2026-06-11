namespace Convex.Client.Infrastructure.Quality;

/// <summary>
/// Defines the connection quality values.
/// </summary>
public enum ConnectionQuality
{
    /// <summary>
    /// Connection quality cannot be determined (insufficient data).
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Excellent connection quality.
    /// Low latency (&lt;100ms), no packet loss, stable connection.
    /// </summary>
    Excellent = 1,

    /// <summary>
    /// Good connection quality.
    /// Moderate latency (100-300ms), minimal packet loss, stable connection.
    /// </summary>
    Good = 2,

    /// <summary>
    /// Fair connection quality.
    /// Higher latency (300-500ms), some packet loss, occasional issues.
    /// </summary>
    Fair = 3,

    /// <summary>
    /// Poor connection quality.
    /// High latency (500-1000ms), significant packet loss, frequent reconnections.
    /// </summary>
    Poor = 4,

    /// <summary>
    /// Terrible connection quality.
    /// Very high latency (&gt;1000ms), severe packet loss, constant reconnections.
    /// </summary>
    Terrible = 5
}

/// <summary>
/// Provides detailed information about the current connection quality.
/// </summary>
/// <remarks>
/// Executes the connection quality info operation.
/// </remarks>
public sealed class ConnectionQualityInfo{
private ConnectionQuality quality;
private string description;
private double? averageLatencyMs;
private double? latencyVarianceMs;
private double? packetLossRate;
private int reconnectionCount;
private int errorCount;
private TimeSpan? timeSinceLastMessage;
private double uptimePercentage;
private int qualityScore;
private IReadOnlyDictionary<string, object>? additionalData;
public ConnectionQualityInfo(ConnectionQuality quality, string description, double? averageLatencyMs = null, double? latencyVarianceMs = null, double? packetLossRate = null, int reconnectionCount = 0, int errorCount = 0, TimeSpan? timeSinceLastMessage = null, double uptimePercentage = 100.0, int qualityScore = 100, IReadOnlyDictionary<string, object>? additionalData = null)
{
    this.Quality = quality;
    this.Description = description;
    this.AverageLatencyMs = averageLatencyMs;
    this.LatencyVarianceMs = latencyVarianceMs;
    this.PacketLossRate = packetLossRate;
    this.ReconnectionCount = reconnectionCount;
    this.ErrorCount = errorCount;
    this.TimeSinceLastMessage = timeSinceLastMessage;
    this.UptimePercentage = uptimePercentage;
    this.QualityScore = qualityScore;
    this.AdditionalData = additionalData ?? new Dictionary<string, object>();
    this.quality = quality;
    this.description = description;
    this.averageLatencyMs = averageLatencyMs;
    this.latencyVarianceMs = latencyVarianceMs;
    this.packetLossRate = packetLossRate;
    this.reconnectionCount = reconnectionCount;
    this.errorCount = errorCount;
    this.timeSinceLastMessage = timeSinceLastMessage;
    this.uptimePercentage = uptimePercentage;
    this.qualityScore = qualityScore;
    this.additionalData = additionalData;
}    /// <summary>
    /// Gets the quality.
    /// </summary>
    public ConnectionQuality Quality { get; } 
    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description { get; } 
    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the average latency ms.
    /// Null if insufficient data.
    /// </summary>
    public double? AverageLatencyMs { get; } 
    /// <summary>
    /// Gets the latency variance ms.
    /// Higher values indicate unstable connection.
    /// Null if insufficient data.
    /// </summary>
    public double? LatencyVarianceMs { get; } 
    /// <summary>
    /// Gets the packet loss rate.
    /// Estimated from failed messages and reconnections.
    /// Null if insufficient data.
    /// </summary>
    public double? PacketLossRate { get; } 
    /// <summary>
    /// Gets the reconnection count.
    /// </summary>
    public int ReconnectionCount { get; } 
    /// <summary>
    /// Gets the error count.
    /// </summary>
    public int ErrorCount { get; } 
    /// <summary>
    /// Gets the time since last message.
    /// Null if no messages have been received.
    /// </summary>
    public TimeSpan? TimeSinceLastMessage { get; } 
    /// <summary>
    /// Gets the uptime percentage.
    /// 100% means always connected, 0% means always disconnected.
    /// </summary>
    public double UptimePercentage { get; } 
    /// <summary>
    /// Gets the quality score.
    /// Combines latency, packet loss, reconnections, and errors into a single metric.
    /// </summary>
    public int QualityScore { get; } 
    /// <summary>
    /// Gets the dictionary.
    /// </summary>
    public IReadOnlyDictionary<string, object> AdditionalData { get; } 
    /// <summary>
    /// Returns a detailed string representation of the connection quality.
    /// </summary>
    public override string ToString()
    {
        var parts = new List<string>
        {
            $"Quality: {Quality}",
            $"Score: {QualityScore}/100",
            $"Description: {Description}"
        };

        if (AverageLatencyMs.HasValue)
        {
            parts.Add($"Avg Latency: {AverageLatencyMs.Value:F2}ms");
        }

        if (LatencyVarianceMs.HasValue)
        {
            parts.Add($"Latency Variance: {LatencyVarianceMs.Value:F2}ms");
        }

        if (PacketLossRate.HasValue)
        {
            parts.Add($"Packet Loss: {PacketLossRate.Value:F2}%");
        }

        parts.Add($"Uptime: {UptimePercentage:F1}%");
        parts.Add($"Reconnections: {ReconnectionCount}");
        parts.Add($"Errors: {ErrorCount}");

        return string.Join(", ", parts);
    }
}
