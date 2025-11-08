namespace CPL.BE;

internal sealed class X64InstructionSelector(X64Allocator Allocator)
{
    private X64Allocator allocator { get; } = Allocator;

    public List<X64Instruction> Instructions { get; } = new();

    public void Emit(IR.Instruction instruction)
    {
        switch (instruction)
        {
            case IR.Function function: Function(function); break;
            case IR.Block block: Block(block); break;
            case IR.DeclareVariable declareVariable: DeclareVariable(declareVariable); break;
            case IR.Store store: Store(store); break;
            case IR.Load load: Load(load); break;
            case IR.Comparison comparison: Comparison(comparison); break;
            case IR.IfStatement ifStatement: IfStatement(ifStatement); break;
        }
    }

    private void Function(IR.Function function)
    {
        function.Block.Instructions.ForEach(Emit);
    }

    private void Block(IR.Block block)
    {
        Instructions.Add(new X64Instruction($"{block.Name}:"));
        block.Instructions.ForEach(Emit);
    }

    private void DeclareVariable(IR.DeclareVariable declareVariable)
    {
        allocator.Allocate(declareVariable.Variable);
    }

    private void Store(IR.Store store)
    {
        var sourceLocation = allocator.GetOrAllocate(store.Source);
        var destinationLocation = allocator.GetMemoryLocation(store.Destination);
        Instructions.Add(new X64Instruction("mov", destinationLocation.X64Rep(), sourceLocation.X64Rep()));
    }

    private void Load(IR.Load load)
    {
        var destinationLocation = allocator.GetMemoryLocation(load.Destination);
        bool isSourceSmaller = load.Source.Type.X64Size() < load.Destination.Type.X64Size();

        if (load.Source is IR.Constant constant)
        {
            var constantRegSize = X64Allocator.TypeToRegisterSize(constant.Type);
            Instructions.Add(new X64Instruction(isSourceSmaller ? "movzx" : "mov", destinationLocation.X64Rep(), $"{constantRegSize.ToString()} {constant.Value}"));
        }
        else
        {
            var sourceLocation = allocator.GetMemoryLocation(load.Source);
            Instructions.Add(new X64Instruction(isSourceSmaller ? "movzx" : "mov", destinationLocation.X64Rep(), sourceLocation.X64Rep()));
        }
    }

    private void Comparison(IR.Comparison comparison)
    {
        var leftLocation = allocator.GetOrAllocate(comparison.Left);
        var rightLocation = allocator.GetOrAllocate(comparison.Right);
        int leftSize = comparison.Left.Type.X64Size();
        int rightSize = comparison.Right.Type.X64Size();
        int largestSize = Math.Max(leftSize, rightSize);
        var leftOperand = allocator.Promote(leftLocation, largestSize);
        var rightOperand = allocator.Promote(rightLocation, largestSize);
        Instructions.Add(new X64Instruction("cmp", rightOperand.X64Rep(), leftOperand.X64Rep()));
    }

    private void IfStatement(IR.IfStatement ifStatement)
    {
        bool hasElse = ifStatement.ElseBlock != null;
        string doneLabel = $"{ifStatement.ThenBlock.Name}.Done";
        string jumpIfFalseLabel = hasElse ? ifStatement.ElseBlock.Name : doneLabel;

        if (ifStatement.Condition is IR.Comparison comparison)
        {
            Emit(comparison);

            string jumpIfFalse = comparison.ComparisonType switch
            {
                AST.ComparisonType.LessThan => "jge",
                AST.ComparisonType.LessThanOrEqual => "jg",
                AST.ComparisonType.Equal => "jne",
                AST.ComparisonType.NotEqual => "je",
                AST.ComparisonType.GreaterThan => "jle",
                AST.ComparisonType.GreaterThanOrEqual => "jl",
                _ => throw new InvalidOperationException()
            };
            Instructions.Add(new X64Instruction($"{jumpIfFalse} {jumpIfFalseLabel}"));
        }
        else
        {
            throw new NotImplementedException(ifStatement.Condition.GetType().Name);
        }

        Block(ifStatement.ThenBlock);

        if (hasElse)
        {
            Instructions.Add(new X64Instruction($"jmp {doneLabel}"));
            Block(ifStatement.ElseBlock);
        }

        Instructions.Add(new X64Instruction($"{doneLabel}:"));
    }
}