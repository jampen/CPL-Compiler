using CPL.IR;
namespace CPL.AST;

internal sealed class Function (string name, Type returnType) : INode
{
    public string Name { get; } = name;
    public Type ReturnType { get; } = returnType;
    public List<INode> Statements { get; } = new();

    public Value CodeGen(Context context)
    {
        var function = context.DeclareFunction(Name, ReturnType);
        foreach(INode statement in Statements)
        {
            statement.CodeGen(context);
        }
        context.ExitBlock();
        return function;
    }
}