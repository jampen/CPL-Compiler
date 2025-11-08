namespace CPL.IR;

internal abstract class Type;

internal sealed class VoidType : Type;

internal sealed class IntType(int bitWidth) : Type
{
    public int BitWidth { get; } = bitWidth;
}

internal sealed class BooleanType() : Type;

internal sealed class PointerType(Type baseType) : Type
{
    public Type BaseType { get; } = baseType;
}

internal sealed class ArrayType(Type elementType, int size) : Type
{
    public Type ElementType { get; } = elementType;
    public int Size {  get; } = size;
};