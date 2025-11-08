namespace CPL.BE;

internal static class X64MemoryLocationExtensions
{
    internal static string X64Rep(this MemoryLocation location)
    {
        if (location is StackLocation stackLocation)
        {
            return stackLocation.X64Rep();
        }
        else if (location is RegisterLocation registerLocation)
        {
            return registerLocation.X64Rep();
        }
        throw new ArgumentException(nameof(location));
    }

    internal static string X64Rep(this StackLocation stackLocation)
    {
        return $"[rbp-{stackLocation.StackOffset}]";
    }

    internal static string X64Rep(this RegisterLocation registerLocation)
    {
        return registerLocation.Name;
    }
}