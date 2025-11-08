namespace CPL.AST;

internal sealed class IfStatement(INode condition, INode thenBlock, INode elseBlock) : INode
{
    internal INode Condition { get; } = condition;
    internal INode ThenBlock { get; } = thenBlock;
    internal INode ElseBlock { get; } = elseBlock;

    public IR.Value CodeGen(IR.Context context)
    {
        IR.Value condition = Condition.CodeGen(context);
        IR.Block thenBlockValue = (IR.Block)ThenBlock.CodeGen(context);
        IR.Block elseBlockValue = (IR.Block)ElseBlock?.CodeGen(context);
        var ifStatement = new IR.IfStatement(condition, thenBlockValue, elseBlockValue);
        context.Emit(ifStatement);
        return ifStatement;
    }
}