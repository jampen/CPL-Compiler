namespace CPL.BE;

internal static class X64TypeExtensions
{
    internal static int X64Size(this IR.Type type)
    {
        if (type is IR.IntType intType)
        {
            return (int)Math.Ceiling((float)(intType.BitWidth) / 8);
        }

        if (type is IR.PointerType)
        {
            // In x64 pointers are a QWORD
            return 8;
        }

        if (type is IR.ArrayType arrayType)
        {
            return arrayType.ElementType.X64Size() * arrayType.Size;
        }

        if (type is IR.VoidType)
        {
            return 1;
        }

        if (type is IR.BooleanType)
        {
            return 1;
        }

        throw new NotImplementedException(nameof(type));
    }

}