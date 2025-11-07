namespace CPL.IR;

internal class Block() : Instruction(new VoidType())
{
    public Dictionary<string, Variable> Variables { get; private set; } = new();
    public List<Instruction> Instructions { get; private set; } = new();

    public void DeclareVariable (Variable variable)
    {
        if (Variables.ContainsKey(variable.Name))
        {
            throw new VariableAlreadyDefinedException(variable.Name);
        }
        Variables [variable.Name] = variable;
    }
}