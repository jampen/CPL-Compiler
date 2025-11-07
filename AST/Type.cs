namespace CPL.AST;

internal enum Constness
{
    Const,
    Mutable
}

internal enum Signedness
{
    Signed,
    Unsigned
}

internal abstract class Type (string name, Constness constness)
{
    public Constness Constness { get; } = constness;
    public string Name { get; } = name;
}

internal abstract class PrimitiveType (string name, Constness constness) : Type(name, constness)
{
}

internal class IntType (int bitWidth, Signedness signedness, Constness constness)
    : PrimitiveType("int" + bitWidth.ToString(), constness)
{
    public int BitWidth { get; } = bitWidth;
    public Signedness Signedness { get; } = signedness;
}

internal sealed class VoidType : PrimitiveType
{
    public VoidType() : base("void", Constness.Mutable)
    {
    }
}

internal sealed class PointerType(Type baseType, Constness constness) : Type(baseType.Name + " ptr", constness)
{
    public Type BaseType { get; } = baseType;
}

internal sealed class ArrayType(Type elementType, int size, Constness constness) 
    : Type("[" + elementType.Name + " x " + size.ToString() + "]", constness)
{
    public Type ElementType { get; } = elementType;
    public int Size { get; } = size;
}