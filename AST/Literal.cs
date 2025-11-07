namespace CPL.AST;

internal enum LiteralType
{
    Char,
    Number,
    String
}

internal class Literal(string value, LiteralType type) : INode 
{
    public string Value { get; } = value;
    public LiteralType Type { get; } = type;

    public IR.Value CodeGen(IR.Context context)
    {
        return Type switch
        {
            LiteralType.Char   => new IR.Constant(Value, new IR.IntType(8)),
            LiteralType.Number => new IR.Constant(Value, new IR.IntType(32)),
            LiteralType.String => new IR.StringConstant(Value),
        };
    }
}