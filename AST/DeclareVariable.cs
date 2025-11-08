namespace CPL.AST;

internal class DeclareVariable(string name, INode initializer, Type type) : INode
{
    public string Name { get; } = name;
    public INode Initializer { get; } = initializer;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public IR.Value CodeGen(IR.Context context)
    {
        IR.Value value = null;

        if (Initializer != null)
        {
            value = Initializer.CodeGen(context);
        }

        var destination = context.DeclareVariable(name: Name, type: Type);

        if (Initializer == null)
        {
            return destination;
        }

        if (Initializer is Identifier identifier)
        {
            var source = context.FindVariable(identifier.Name);
            context.Emit(new IR.Store(destination, source));
        }
        else
        {
            context.Emit(new IR.Load(destination, value));
        }

        return destination;
    }
}
