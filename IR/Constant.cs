namespace CPL.IR;

internal class Constant(string value, Type type) : Value (type)
{
    public string Value { get; } = value;
}

internal class StringConstant(string value) : Value(type: new PointerType(new IntType(8)))
{
    public string Value { get; } = value;
}