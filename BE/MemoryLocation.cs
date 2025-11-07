namespace CPL.BE;

internal abstract class MemoryLocation;

internal sealed class StackLocation(int stackOffset) : MemoryLocation
{
    public int StackOffset { get; } = stackOffset;
}

internal sealed class RegisterLocation(string name) : MemoryLocation
{
    public string Name { get; } = name;
}