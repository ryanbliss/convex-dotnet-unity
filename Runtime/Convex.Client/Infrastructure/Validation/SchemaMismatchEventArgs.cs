namespace Convex.Client.Infrastructure.Validation;

/// <summary>
/// Event arguments for schema mismatch events.
/// </summary>
/// <remarks>
/// Executes the schema mismatch event args operation.
/// </remarks>
public sealed class SchemaMismatchEventArgs : EventArgs
{
private string functionName;
private string expectedType;
private string actualType;
private IReadOnlyList<string> validationErrors;
private object? actualValue;
public SchemaMismatchEventArgs(string functionName, string expectedType, string actualType, IReadOnlyList<string> validationErrors, object? actualValue = null)
{
    this.FunctionName = functionName;
    this.ExpectedType = expectedType;
    this.ActualType = actualType;
    this.ValidationErrors = validationErrors;
    this.ActualValue = actualValue;
    this.functionName = functionName;
    this.expectedType = expectedType;
    this.actualType = actualType;
    this.validationErrors = validationErrors;
    this.actualValue = actualValue;
}    /// <summary>
    /// Gets the function name.
    /// </summary>
    public string FunctionName { get; } 
    /// <summary>
    /// Gets the expected type.
    /// </summary>
    public string ExpectedType { get; } 
    /// <summary>
    /// Gets the actual type.
    /// </summary>
    public string ActualType { get; } 
    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; } 
    /// <summary>
    /// Gets the actual value.
    /// </summary>
    public object? ActualValue { get; } }
