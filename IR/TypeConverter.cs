namespace CPL.IR;

internal class UnknownTypeException (AST.Type type) : Exception ("Cannot convert AST type " + type.Name + " to a suitable IR type");

internal static class TypeConverter
{
    // Recursively converts AST types to IR types
    public static Type Convert(AST.Type type)
    {
        return type switch
        {
            AST.IntType intType => new IR.IntType(intType.BitWidth),
            AST.PointerType ptrType => new IR.PointerType(Convert(ptrType.BaseType)),
            AST.ArrayType arrayType => new IR.ArrayType(Convert(arrayType.ElementType), arrayType.Size),
            AST.VoidType voidType => new IR.VoidType(),
            _ => throw new UnknownTypeException(type)
        };
    }
}
