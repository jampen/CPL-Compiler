namespace CPL.AST;

internal class DeclareVariable(string name, INode initializer, Type type) : INode
{
    public string Name { get; } = name;
    public INode Initializer { get; } = initializer;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public IR.Value CodeGen(IR.Context context)
    {
        var variable = context.DeclareVariable(name: Name, type: Type);

        if (Initializer != null)
        {
            IR.Value value = Initializer.CodeGen(context);
            context.Emit(new IR.Store(variable, value));
        }
        return variable;
    }
}
