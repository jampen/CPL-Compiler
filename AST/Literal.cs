namespace CPL.AST;

internal enum LiteralType
{
    Char,
    Number,
    String
}

internal class Literal(string value, LiteralType type, int width) : INode 
{
    public string Value { get; } = value;
    public LiteralType Type { get; } = type;
    public int Width { get; } = width; // Bad

    public IR.Value CodeGen(IR.Context context)
    {
        // TODO: Use Context Platform to create IR types.
        return Type switch
        {
            LiteralType.Char   => new IR.Constant(Value, new IR.IntType(width)),
            LiteralType.Number => new IR.Constant(Value, new IR.IntType(width)),
            LiteralType.String => new IR.StringConstant(Value),
        };
    }
}