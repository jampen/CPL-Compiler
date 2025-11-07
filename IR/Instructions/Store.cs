namespace CPL.IR;

internal class Store(Value destination, Value source) : Instruction(source.Type)
{
    public Value Destination { get; } = destination;
    public Value Source { get; } = source;
}