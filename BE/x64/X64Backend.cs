namespace CPL.BE;

internal sealed class X64Backend() : Backend(new X64Allocator())
{
    public override void Generate(IR.Context context)
    {
        WriteLine("section .text");
        foreach (var functionDefinition in context.Functions)
        {
            IR.Function function = functionDefinition.Value;
            WriteLine($"global {function.Name}");
            WriteLine($"{function.Name}:");
            // Prologue
            WriteLine("\tpush rbp");
            WriteLine("\tmov rbp, rsp");
            WriteLine($"\tsub rsp, {Align16(CalculateStackSize(function.Block))}");
            EmitInstruction(function.Block);
            // Epilogue
            WriteLine("\tmov rsp, rbp");
            WriteLine("\tpop rbp");
            WriteLine("\tret");
        }
    }


    private void WriteLine(string s)
    {
        Console.WriteLine(s);
    }

    private void Load(IR.Load load)
    {
        var destinationLocation = MemoryLocations[load.Destination];
        var sourceLocation = MemoryLocations[load.Source];

        var translatedDestination = Allocator.TranslateMemoryLocation(destinationLocation);
        var translatedSource = Allocator.TranslateMemoryLocation(sourceLocation);

        WriteLine($"\tmov {translatedDestination}, {translatedSource}");
        MemoryLocations[load.Destination] = destinationLocation;
    }

    private void Store(IR.Store store)
    {
        var destinationLocation = MemoryLocations[store.Destination];

        if (store.Source is IR.Constant constant)
        {
            string type = "unset";

            if (constant.Type is IR.IntType)
            {
                type = "dword";
            }

            WriteLine($"\tmov {type} {Allocator.TranslateMemoryLocation(destinationLocation)}, {constant.Value}");
        }
    }

    private void EmitInstruction(IR.Instruction ins)
    {
        switch (ins)
        {
            case IR.Block block:
                block.Instructions.ForEach(EmitInstruction);
                break;

            case IR.DeclareVariable declareVariable:
                MemoryLocations[declareVariable.Variable] = Allocator.Allocate(declareVariable.Variable);
                break;

            case IR.Store store: Store(store); break;
            case IR.Load load: Load(load); break;
        }
    }
}