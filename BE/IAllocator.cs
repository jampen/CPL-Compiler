namespace CPL.BE;

internal interface IAllocator
{
    // Allocates a memory location for the value
    MemoryLocation Allocate(IR.Value value);
    
    // De allocate the memory location of the value
    void Free(IR.Value value);

    public static int Align16(int value) => (value + 15) & ~0xF;
}