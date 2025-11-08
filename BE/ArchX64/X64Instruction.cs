namespace CPL.BE;

internal class X64Instruction(string mnemonic, string? operand1=null, string? operand2 = null)
{
    public string Mnemonic { get; } = mnemonic;
    public string? Operand1 { get; } = operand1;
    public string? Operand2 { get; } = operand2;

    public override string ToString()
    {
        if (Operand1 == null && Operand2 == null)
        {
            return $"\t{Mnemonic}";
        }
        else if (Operand2 == null)
        {
            return $"\t{Mnemonic} {Operand1}";
        }
        else
        {
            return $"\t{Mnemonic} {Operand1}, {Operand2}";
        }
    }
}