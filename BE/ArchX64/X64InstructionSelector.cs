namespace CPL.BE;

internal sealed class X64InstructionSelector(X64Allocator Allocator) : InstructionSelector
{
    private X64Allocator allocator { get; } = Allocator;
    public List<X64Instruction> Instructions { get; } = new();

    protected override void Function(IR.Function function)
    {
        function.Block.Instructions.ForEach(Emit);
    }

    protected override void Block(IR.Block block)
    {
        Instructions.Add(new(X64Mnemonic.Label, [new X64LabelOperand(block.Name)]));
        block.Instructions.ForEach(Emit);

        // Cleanup used locations
        foreach (var variable in block.Variables.Values)
        {
            allocator.Free(variable);
        }
    }

    protected override void DeclareVariable(IR.DeclareVariable declareVariable)
    {
        allocator.Allocate(declareVariable.Variable);
    }

    private X64Operand ValueToOperand(IR.Value value)
    {
        if (value is IR.Constant constant)
        {
            return new X64ImmediateOperand(constant.Value, X64Allocator.TypeToRegisterSize(constant.Type).Value);
        }

        return allocator.GetMemoryLocation(value) switch
        {
            StackLocation stackLocation => new X64StackOperand(stackLocation.StackOffset, allocator.GetValueSize(value)),
            RegisterLocation registerLocation => new X64RegisterOperand(Enum.Parse<X64Allocator.Register>(registerLocation.Name)),
            _ => throw new Exception()
        };
    }


    protected override void Store(IR.Store store)
    {
        bool isSourceSmaller = store.Source.Type.X64Size() < store.Destination.Type.X64Size();
        X64Mnemonic mov = isSourceSmaller ? X64Mnemonic.MovZX : X64Mnemonic.Mov;
        var sourceOperand = ValueToOperand(store.Source);
        var destinationOperand = ValueToOperand(store.Destination);

        if (sourceOperand is X64StackOperand sourceStackLoc && destinationOperand is X64StackOperand destinationStackLoc)
        {
            var temporary = allocator.AllocateTemporary(store.Source);
            var temporaryOperand = new X64RegisterOperand(Enum.Parse<X64Allocator.Register>(temporary.Name));
            Instructions.Add(new(mov, [temporaryOperand, sourceOperand]));
            Instructions.Add(new(mov, [destinationOperand, temporaryOperand]));
            allocator.Free(temporary);
        }
        else
        {
            Instructions.Add(new(mov, [destinationOperand, sourceOperand]));
        }
    }

    protected override void Load(IR.Load load)
    {
        var sourceOperand = ValueToOperand(load.Source);
        var destinationOperand = ValueToOperand(load.Destination);
        Instructions.Add(new(X64Mnemonic.Mov, [destinationOperand, sourceOperand]));
    }

    protected override void Comparison(IR.Comparison comparison)
    {
    }

    protected override void IfStatement(IR.IfStatement ifStatement)
    {
    }
}