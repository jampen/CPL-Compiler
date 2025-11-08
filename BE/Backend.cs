namespace CPL.BE;

internal abstract class Backend (IAllocator allocator)
{
    protected Dictionary<IR.Value, MemoryLocation> MemoryLocations = new();
    protected IAllocator Allocator { get; } = allocator ?? throw new ArgumentNullException(nameof(allocator));

    // Generates instructions for the target machine
    public abstract void Generate(IR.Context context);
}