namespace CPL.AST;

internal class Assign(INode left, INode expression) : INode
{
    public INode Left { get; } = left;
    public INode Expression { get; } = expression;

    public IR.Value CodeGen(IR.Context context)
    {
        Left.CodeGen(context);
        Expression.CodeGen(context);
        throw new NotImplementedException();
    }
}