namespace CPL.BE;

internal sealed class X64InstructionSelector(X64Allocator Allocator) : InstructionSelector
{
    public X64Allocator Allocator { get; } = Allocator;
    public List<X64Instruction> Instructions { get; } = new();

    protected override void Function(IR.Function function) =>
        function.Block.Instructions.ForEach(Emit);

    protected override void Block(IR.Block block)
    {
        Instructions.Add(new(X64Mnemonic.Label, [new X64LabelOperand(block.Name)]));
        block.Instructions.ForEach(Emit);

        // Cleanup used locations
        foreach (var variable in block.Variables.Values)
        {
            Allocator.Free(variable);
        }
    }

    protected override void DeclareVariable(IR.DeclareVariable declareVariable) =>
        Allocator.Allocate(declareVariable.Variable);

    public X64Operand MemoryLocationToOperand(MemoryLocation location) =>
        location switch
        {
            StackLocation stackLocation => new X64StackOperand(stackLocation.StackOffset, Allocator.GetMemoryLocationSize(location)),
            RegisterLocation registerLocation => new X64RegisterOperand(Enum.Parse<X64Allocator.Register>(registerLocation.Name)),
            _ => throw new ArgumentException("invalid location")
        };

    public X64Operand ValueToOperand(IR.Value value) =>
        (value is IR.Constant constant) 
        ? new X64ImmediateOperand(constant.Value, X64Allocator.TypeToRegisterSize(constant.Type).Value)
        : MemoryLocationToOperand(Allocator.GetMemoryLocation(value));

    protected override void Store(IR.Store store)
    {
        X64Operand sourceOperand = ValueToOperand(store.Source);
        X64Operand destinationOperand = ValueToOperand(store.Destination);

        switch (sourceOperand, destinationOperand)
        {
            case (X64StackOperand stackSource, X64StackOperand stackDest):
                bool isSourceSmaller = store.Source.Type.X64Size() < store.Destination.Type.X64Size();
                X64Mnemonic mov = isSourceSmaller ? X64Mnemonic.MovZX : X64Mnemonic.Mov;
                var temporary = Allocator.AllocateTemporary(store.Source);
                var temporaryOperand = new X64RegisterOperand(Enum.Parse<X64Allocator.Register>(temporary.Name));
                Instructions.Add(new(mov, [temporaryOperand, sourceOperand]));
                Instructions.Add(new(mov, [destinationOperand, temporaryOperand]));
                Allocator.Free(temporary);
                break;

            case (X64RegisterOperand regSource, X64StackOperand stackDest):
                Instructions.Add(new(X64Mnemonic.Mov, [stackDest, regSource]));
                break;

            case (X64RegisterOperand regSource, X64RegisterOperand regDest):
                Instructions.Add(new(X64Mnemonic.MovZX, [regDest, regSource]));
                break;

            default:
                throw new Exception("Invalid store args");
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
        using X64TemporaryAllocator tempAllocator = new(this);
        var storage = tempAllocator.New(comparison.Storage);
        var storageOperand = ValueToOperand(comparison.Storage);

        RegisterLocation leftLocation = tempAllocator.Store(comparison.Left);
        RegisterLocation rightLocation = tempAllocator.Store(comparison.Right);

        Instructions.Add(new(X64Mnemonic.Cmp, [
            MemoryLocationToOperand(leftLocation),
            MemoryLocationToOperand(rightLocation)
         ]));


        var storeIns = comparison.ComparisonType switch
        {
            AST.ComparisonType.Equal => X64Mnemonic.SetE,
            AST.ComparisonType.NotEqual => X64Mnemonic.SetNE,
            AST.ComparisonType.LessThan => X64Mnemonic.SetL,
            AST.ComparisonType.GreaterThan => X64Mnemonic.SetG,
            AST.ComparisonType.LessThanOrEqual => X64Mnemonic.SetLE,
            AST.ComparisonType.GreaterThanOrEqual => X64Mnemonic.SetGE,
            _ => throw new ArgumentException(nameof(comparison.ComparisonType)),
        };

        Instructions.Add(new(storeIns, [storageOperand]));
    }

    protected override void IfStatement(IR.IfStatement ifStatement)
    {

    }
}