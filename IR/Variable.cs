namespace CPL.IR;

internal sealed class Variable (string name, IR.Type type) : Value(type)
{
    public string Name { get; } = name;
}