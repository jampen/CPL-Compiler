namespace CPL.IR;

internal class Comparison (Value storage, Value left, Value right, AST.ComparisonType type) : Instruction(new BooleanType())
{
    /// <summary>
    /// The value containing the result of the comparison
    /// </summary>
    public Value Storage { get; } = storage;
    public Value Left { get; } = left;
    public Value Right { get; } = right;
    public AST.ComparisonType ComparisonType { get; } = type;
}

