using CPL.IR;

namespace CPL.AST;

internal sealed class Block (List<INode> statements) : INode
{
    private static int blockID = 0;

    internal List<INode> Statements { get; } = statements;

    public Value CodeGen(Context context)
    {
        var block = context.EnterBlock($"L{blockID++}");
        foreach (var statement in Statements)
        {
            statement.CodeGen(context);
        }
        context.ExitBlock();
        return block;
    }
}