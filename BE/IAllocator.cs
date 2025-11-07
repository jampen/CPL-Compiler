namespace CPL.BE;

internal interface IAllocator
{
    // Returns the size of the type on the target machine as bytes
    int SizeOfType(IR.Type type);

    // Allocates a memory location for the variable
    MemoryLocation Allocate(IR.Variable variable);

    // Translates the memory location into string form for assembly
    string TranslateMemoryLocation(MemoryLocation location);
}