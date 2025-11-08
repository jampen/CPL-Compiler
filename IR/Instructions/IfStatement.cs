namespace CPL.IR;

internal class IfStatement(Value condition, Block thenBlock, Block elseBlock) : Instruction(new VoidType())
{
    public Value Condition { get; } = condition;
    public Block ThenBlock { get; } = thenBlock;
    public Block ElseBlock { get; } = elseBlock;
}