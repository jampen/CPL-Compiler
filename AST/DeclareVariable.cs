namespace CPL.AST;

internal class DeclareVariable(string name, INode initializer, Type type) : INode
{
    public string Name { get; } = name;
    public INode Initializer { get; } = initializer;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public IR.Value CodeGen(IR.Context context)
    {
        IR.Value? initialValue = null;

        if (Initializer != null)
        {
            initialValue = Initializer.CodeGen(context);
        }

        var variable = context.DeclareVariable(Name, Type);

        if (initialValue != null)
        {
            context.Emit(new IR.Store(variable, initialValue));
        }

        return variable;
    }
}
