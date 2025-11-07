namespace CPL.IR;

internal class DeclareVariable(Variable variable) : Instruction(variable.Type)
{
    public Variable Variable { get; } = variable;
}