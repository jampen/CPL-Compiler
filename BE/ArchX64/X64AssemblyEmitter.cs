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
            case X64Mnemonic.Mov: Mov(instruction); break;
            case X64Mnemonic.MovZX: Mov(instruction); break;
        }
    }

    private void Mov(X64Instruction instruction)
    {
        string opcode = instruction.Mnemonic.ToString();

        string dest = instruction.Operands[0] switch
        {
            X64RegisterOperand reg => reg.Register.ToString(),
            X64StackOperand stack => $"{stack.Size} [rbp-{stack.StackOffset}]",
            _ => throw new ArgumentException("invalid destination argument")
        };

        string src = instruction.Operands[1] switch
        {
            X64RegisterOperand reg => reg.Register.ToString(),
            X64ImmediateOperand imm => $"{imm.Size} {imm.Value}",
            X64StackOperand stack => $"{stack.Size} [rbp-{stack.StackOffset}]",
            _ => throw new ArgumentException("invalid source argument")
        };

        Assembly.Add($"\t{opcode} {dest}, {src}");
    }
}
