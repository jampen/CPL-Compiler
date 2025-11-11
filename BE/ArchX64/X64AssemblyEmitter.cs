namespace CPL.BE;

internal sealed class X64AssemblyEmitter(List<X64Instruction> instructions)
{
    protected List<X64Instruction> Instructions { get; } = instructions;

    public List<string> Assembly { get; } = new();

    public void Emit()
    {
        foreach (var instruction in Instructions)
        {
            Emit(instruction);
        }
    }

    private void Emit(X64Instruction instruction)
    {
        switch (instruction.Mnemonic)
        {
            case X64Mnemonic.Mov: Mov(instruction);  break;
            case X64Mnemonic.MovZX: Mov(instruction);  break;
        }
    }

    private void Mov(X64Instruction instruction)
    {
        string opcode = instruction.Mnemonic.ToString();
        var destination = instruction.Operands[0];
        var source = instruction.Operands[1];

        if (destination is X64RegisterOperand destinationRegister)
        {
            if (source is X64RegisterOperand sourceRegister)
            {
                Assembly.Add($"\t{opcode} {destinationRegister.Register}, {sourceRegister.Register}");
            }

            if (source is X64ImmediateOperand sourceImmediate)
            {
                Assembly.Add($"\t{opcode} {destinationRegister.Register}, {sourceImmediate.Size} {sourceImmediate.Value}");
            }

            if (source is X64StackOperand sourceStack)
            {
                Assembly.Add($"\t{opcode} {destinationRegister.Register}, {sourceStack.Size} [rbp-{sourceStack.StackOffset}]");
            }
        }

        if (destination is X64StackOperand destinationStack)
        {
            string location = $"{destinationStack.Size} [rbp-{destinationStack.StackOffset}]";

            if (source is X64RegisterOperand sourceRegister)
            {
                Assembly.Add($"\t{opcode} {location}, {sourceRegister.Register}");
            }

            if (source is X64ImmediateOperand sourceImmediate)
            {
                Assembly.Add($"\t{opcode} {location}, {sourceImmediate.Size} {sourceImmediate.Value}");
            }

            if (source is X64StackOperand sourceStack)
            {
                Assembly.Add($"\t{opcode} {location}, {sourceStack.Size} [rbp-{sourceStack.StackOffset}]");
            }
        }
    }
}