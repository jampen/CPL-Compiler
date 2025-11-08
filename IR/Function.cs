namespace CPL.IR;

internal sealed class Function(string name, Block block, Type returnType) : Instruction(returnType)
{
    public string Name { get; } = name;
    public Block Block { get; } = block;
}