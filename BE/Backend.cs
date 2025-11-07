namespace CPL.BE;

internal abstract class Backend (IAllocator allocator)
{
    protected Dictionary<IR.Value, MemoryLocation> MemoryLocations = new();
    protected IAllocator Allocator { get; } = allocator ?? throw new ArgumentNullException(nameof(allocator));


    // Generates assembly for the target machine
    public abstract void Generate(IR.Context context);
    protected static int Align16(int value) => (value + 15) & ~0xF;

    protected int CalculateStackSize(IR.Instruction instruction)
    {
        int size = 0;
        if (instruction is IR.Block block)
        {
            foreach (var variable in block.Variables.Values)
            {
                size += Allocator.SizeOfType(variable.Type);
            }
            foreach (var ins in block.Instructions)
            {
                size += CalculateStackSize(ins);
            }
        }
        return size;
    }
}