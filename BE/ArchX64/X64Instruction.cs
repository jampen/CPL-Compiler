using static CPL.BE.X64Allocator;

namespace CPL.BE;

internal enum X64Mnemonic
{
    Mov,
    MovZX,

    Push,
    Pop,

    Cmp,
    Test,

    // Set address to comparison result
    SetL,
    SetLE,
    SetE,
    SetG,
    SetGE,
    SetZ,

    // Jumps:
    Label,
    Jmp,
    JZ,
    JL,
    JG,
    JGE,
    JNE,

    // Math
    Add,
    Sub,
    Mul,
    IDiv,
}


internal sealed class X64Instruction(X64Mnemonic mnemonic, List<X64Operand> operands)
{
    public X64Mnemonic Mnemonic { get; } = mnemonic;
    public List<X64Operand> Operands { get; } = operands;
}

internal abstract class X64Operand;

internal sealed class X64LabelOperand(string name) : X64Operand
{
    public string Name { get; } = name;
}

internal sealed class X64RegisterOperand(Register register) : X64Operand
{
    public Register Register { get; } = register;
}

internal sealed class X64StackOperand(int stackOffset, RegisterSize size) : X64Operand
{
    public int StackOffset { get; } = stackOffset;
    public RegisterSize Size { get; } = size;
}

internal sealed class X64ImmediateOperand(string value, RegisterSize size) : X64Operand
{
    public string Value { get; } = value;
    public RegisterSize Size { get; } = size;
}