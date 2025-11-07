namespace CPL.IR;

internal abstract class Value(Type type)
{
    public Type Type { get; } = type;
}