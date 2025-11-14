namespace CPL.BE;

internal sealed class X64AssemblyEmitter(List<X64Instruction> instructions)
{
    public List<string> Assembly { get; } = new();
    private List<X64Instruction> instructions { get; } = instructions;

    public void Emit() =>
        instructions.ForEach(Emit);

    private void Emit(X64Instruction instruction)
    {
        switch (instruction.Mnemonic)
        {
            case X64Mnemonic.Mov: Mov(instruction); break;
            case X64Mnemonic.MovZX: Mov(instruction); break;
            case X64Mnemonic.Cmp:  Cmp(instruction); break;
            case X64Mnemonic.SetL: StoreCmpFlag(instruction); break;
            case X64Mnemonic.SetLE: StoreCmpFlag(instruction); break;
            case X64Mnemonic.SetE: StoreCmpFlag(instruction); break;
            case X64Mnemonic.SetNE: StoreCmpFlag(instruction); break;
            case X64Mnemonic.SetG: StoreCmpFlag(instruction); break;
            case X64Mnemonic.SetGE: StoreCmpFlag(instruction); break;
        }
    }

    private string OperandToString(X64Operand operand, bool omitSize = false)
    {
        return operand switch
        {
            X64RegisterOperand reg => reg.Register.ToString(),
            X64ImmediateOperand imm => $"{imm.Size} {imm.Value}",
            X64StackOperand stack => (omitSize ? string.Empty : $"{stack.Size} ") +  $"[RBP-{stack.StackOffset}]",
            _ => throw new ArgumentException("invalid operand type")
        };
    }

    private void Mov(X64Instruction instruction)
    {
        string opcode = instruction.Mnemonic.ToString();
        string dest = OperandToString(instruction.Operands[0], omitSize: instruction.Operands[1] is X64RegisterOperand);
        string src = OperandToString(instruction.Operands[1]);
        Assembly.Add($"\t{opcode} {dest}, {src}");
    }

    private void Cmp(X64Instruction instruction)
    {
        string opcode = instruction.Mnemonic.ToString();
        string right = OperandToString(instruction.Operands[0]);
        string left = OperandToString(instruction.Operands[1]);
        Assembly.Add($"\t{opcode} {left}, {right}");
    }

    private void StoreCmpFlag(X64Instruction instruction)
    {
        string opcode = instruction.Mnemonic.ToString();
        string dest = OperandToString(instruction.Operands[0]);
        Assembly.Add($"\t{opcode} {dest}");
    }

}
