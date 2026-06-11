using System;

namespace Convex.Client.Attributes;

/// <summary>
/// Enumeration of Convex function types.
/// </summary>
public enum FunctionType
{
    /// <summary>
    /// Query function - read-only operations.
    /// </summary>
    Query,

    /// <summary>
    /// Mutation function - state-changing operations.
    /// </summary>
    Mutation,

    /// <summary>
    /// Action function - side-effect operations.
    /// </summary>
    Action
}

/// <summary>
/// Base attribute for marking Convex functions that should be generated.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvexFunctionAttribute"/> class.
/// </remarks>
/// <param name="functionName">The Convex function name.</param>
/// <param name="functionType">The function type.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ConvexFunctionAttribute : Attribute
{
private string functionName;
private FunctionType functionType;
public ConvexFunctionAttribute(string functionName, FunctionType functionType = FunctionType.Query)
{
    this.FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
    this.FunctionType = functionType;
    this.functionName = functionName;
    this.functionType = functionType;
}    /// <summary>
    /// Gets the argument null exception.
    /// </summary>
    public string FunctionName { get; } 
    /// <summary>
    /// Gets the function type.
    /// </summary>
    public FunctionType FunctionType { get; } }

/// <summary>
/// Attribute for marking Convex query functions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvexQueryAttribute"/> class.
/// </remarks>
/// <param name="functionName">The Convex function name.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ConvexQueryAttribute : ConvexFunctionAttribute
{
private string functionName;
public ConvexQueryAttribute(string functionName) : base(functionName, FunctionType.Query)
{
    this.functionName = functionName;
}}

/// <summary>
/// Attribute for marking Convex mutation functions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvexMutationAttribute"/> class.
/// </remarks>
/// <param name="functionName">The Convex function name.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ConvexMutationAttribute : ConvexFunctionAttribute
{
private string functionName;
public ConvexMutationAttribute(string functionName) : base(functionName, FunctionType.Mutation)
{
    this.functionName = functionName;
}}

/// <summary>
/// Attribute for marking Convex action functions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvexActionAttribute"/> class.
/// </remarks>
/// <param name="functionName">The Convex function name.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ConvexActionAttribute : ConvexFunctionAttribute
{
private string functionName;
public ConvexActionAttribute(string functionName) : base(functionName, FunctionType.Action)
{
    this.functionName = functionName;
}}

/// <summary>
/// Attribute for marking a class as a Convex table.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public class ConvexTableAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the table name.
    /// </summary>
    public string? TableName { get; set; }
}

/// <summary>
/// Attribute for marking a property as indexed in Convex.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConvexIndexAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the index name.
    /// </summary>
    public string? IndexName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether unique.
    /// </summary>
    public bool IsUnique { get; set; }
}

/// <summary>
/// Attribute for marking a property as searchable in Convex.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConvexSearchIndexAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the index name.
    /// </summary>
    public string? IndexName { get; set; }
}

/// <summary>
/// Attribute for marking a property as a foreign key reference to another table.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvexForeignKeyAttribute"/> class.
/// </remarks>
/// <param name="tableName">The name of the referenced table.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConvexForeignKeyAttribute : Attribute
{
private string tableName;
public ConvexForeignKeyAttribute(string tableName)
{
    this.TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
    this.tableName = tableName;
}    /// <summary>
    /// Gets the argument null exception.
    /// </summary>
    public string TableName { get; } }

/// <summary>
/// Attribute for specifying validation constraints on a property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConvexValidationAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the min.
    /// </summary>
    public double Min { get; set; } = double.MinValue;

    /// <summary>
    /// Gets or sets the max.
    /// </summary>
    public double Max { get; set; } = double.MaxValue;

    /// <summary>
    /// Gets or sets the min length.
    /// </summary>
    public int MinLength { get; set; } = 0;

    /// <summary>
    /// Gets or sets the max length.
    /// </summary>
    public int MaxLength { get; set; } = int.MaxValue;
}

/// <summary>
/// Attribute for excluding a property from the Convex schema.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConvexIgnoreAttribute : Attribute
{
}

