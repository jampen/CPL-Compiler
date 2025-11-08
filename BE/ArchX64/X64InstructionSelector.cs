namespace CPL.BE;

internal class X64InstructionSelector(X64Allocator Allocator)
{
    private X64Allocator allocator { get; } = Allocator;

    public List<X64Instruction> Instructions { get; } = new();

    public void Emit(IR.Instruction instruction)
    {
        switch (instruction)
        {
            case IR.Block block: block.Instructions.ForEach(Emit); break;
            case IR.DeclareVariable declareVariable: DeclareVariable(declareVariable); break;
            case IR.Store store: Store(store); break;
            case IR.Load load: Load(load); break;
        }
    }

    private void DeclareVariable(IR.DeclareVariable declareVariable)
    {
        allocator.Allocate(declareVariable.Variable);
    }

    private void Store(IR.Store store)
    {
        var sourceLocation = allocator.Allocate(store.Source);
        var destinationLocation = allocator.GetMemoryLocation(store.Destination);
        Instructions.Add(new X64Instruction("; Store"));
        Instructions.Add(new X64Instruction("mov", destinationLocation.X64Rep(), sourceLocation.X64Rep()));
    }

    private void Load(IR.Load load)
    {
        var destinationLocation = allocator.GetMemoryLocation(load.Destination);
        bool isSourceSmaller = load.Source.Type.X64Size() < load.Destination.Type.X64Size();

        if (load.Source is IR.Constant constant)
        {
            var constantRegSize = X64Allocator.TypeToRegSize(constant.Type);
            Instructions.Add(new X64Instruction(isSourceSmaller ? "movzx" : "mov", destinationLocation.X64Rep(), $"{constantRegSize.ToString()} {constant.Value}"));
        }
        else
        {
            var sourceLocation = allocator.GetMemoryLocation(load.Source);
            Instructions.Add(new X64Instruction(isSourceSmaller ? "movzx" : "mov", destinationLocation.X64Rep(), sourceLocation.X64Rep()));
        }
    }
}