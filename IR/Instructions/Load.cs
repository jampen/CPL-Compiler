namespace CPL.IR;

internal class Load(Value destination, Value source) : Instruction(destination.Type)
{
    public Value Destination { get; } = destination;
    public Value Source { get; } = source;
}