namespace CPL.AST;

internal class Identifier(string name) : INode
{
    public string Name { get; } = name;

    public IR.Value CodeGen(IR.Context context)
    {
        var variable = context.FindVariable(Name);
        if (variable == null)
            throw new Exception($"Variable '{Name}' not found.");
        return variable;
    }
}