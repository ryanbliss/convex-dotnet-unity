using Convex.Client.Infrastructure.ErrorHandling;

namespace Convex.Client.Infrastructure.Validation;

/// <summary>
/// Exception thrown when schema validation fails.
/// </summary>
/// <remarks>
/// Executes the schema validation exception operation.
/// </remarks>
public sealed class SchemaValidationException : ConvexException
{
private string functionName;
private string expectedType;
private string actualType;
private IReadOnlyList<string> validationErrors;
public SchemaValidationException(string functionName, string expectedType, string actualType, IReadOnlyList<string> validationErrors) : base($"Schema validation failed for '{functionName}': Expected {expectedType}, got {actualType}")
{
    this.FunctionName = functionName;
    this.ExpectedType = expectedType;
    this.ActualType = actualType;
    this.ValidationErrors = validationErrors;
    this.functionName = functionName;
    this.expectedType = expectedType;
    this.actualType = actualType;
    this.validationErrors = validationErrors;
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
    /// Initializes a new instance of the <see cref="SchemaValidationException"/> class.
    /// </summary>
    public SchemaValidationException(
        string functionName,
        string expectedType,
        string actualType,
        string validationError)
        : this(functionName, expectedType, actualType, new[] { validationError })
    {
    }
}
