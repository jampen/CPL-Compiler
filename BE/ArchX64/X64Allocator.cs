namespace CPL.BE;

internal class X64Allocator : IAllocator
{
    public MemoryLocation Allocate(IR.Variable variable)
    {
        /// TODO:
        return new StackLocation(0);
    }

    public int SizeOfType(IR.Type type)
    {
        if (type is IR.IntType intType)
        {
            return intType.BitWidth / 8;
        }

        if (type is IR.PointerType)
        {
            // On x64 pointers are a QWORD
            return 8;
        }

        if (type is IR.ArrayType arrayType)
        {
            return SizeOfType(arrayType.ElementType) * arrayType.Size;
        }

        if (type is IR.VoidType)
        {
            return 0;
        }

        throw new NotImplementedException(nameof(type));
    }

    public string TranslateMemoryLocation(MemoryLocation location)
    {
        if (location is StackLocation stackLocation)
        {
            return $"[rbp-{stackLocation.StackOffset}]";
        }
        else if (location is RegisterLocation reg)
        {
            return reg.Name;
        }

        throw new NotImplementedException(nameof(location));
    }

}