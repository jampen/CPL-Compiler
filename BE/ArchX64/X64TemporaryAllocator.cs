namespace CPL.BE;

internal sealed class X64TemporaryAllocator (X64InstructionSelector selector) : IDisposable
{
    private X64InstructionSelector Selector { get; } = selector;
    private readonly Stack<MemoryLocation> cleanup = new();

    public RegisterLocation Store(IR.Value value)
    {
        var allocator = Selector.Allocator;

        var source = allocator.IsAllocated(value) ?
            Selector.MemoryLocationToOperand(allocator.GetMemoryLocation(value))
            : Selector.ValueToOperand(value);

        var temp = New(value);
        Selector.Instructions.Add(new(X64Mnemonic.Mov, [Selector.MemoryLocationToOperand(temp), source]));
        return temp;
    }

    public RegisterLocation New(IR.Value value)
    {
        var temp = Selector.Allocator.AllocateTemporary(value);
        cleanup.Push(temp);
        return temp;
    }

    public void Dispose()
    {
        while (cleanup.Count > 0)
        {
            var temp = cleanup.Pop();
            Selector.Allocator.Free(temp);
        }
    }
}