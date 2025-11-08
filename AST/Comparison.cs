namespace CPL.AST;

internal sealed class Comparison (INode left, INode right, ComparisonType comparisonType) : INode
{
    public INode Left { get; } = left;
    public INode Right { get; } = right;
    public ComparisonType ComparisonType { get; } = comparisonType;
        
    public IR.Value CodeGen(IR.Context context)
    {
        var leftValue = Left.CodeGen(context);
        var rightValue = Right.CodeGen(context);
        var storage = new IR.TemporaryVariable(new IR.BooleanType());
        var comparison = new IR.Comparison(storage, leftValue, rightValue, ComparisonType);
        return comparison;
    }
}

internal enum ComparisonType
{
    LessThan,
    LessThanOrEqual,
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    And,
    Or,
    Xor
}