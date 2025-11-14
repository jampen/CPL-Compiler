namespace CPL.BE;

internal sealed class X64Backend() : Backend(new X64Allocator())
{
    public override void Generate(IR.Context context)
    {

        WriteLine("section .text");

        foreach (var functionDefinition in context.Functions)
        {
            X64Allocator allocator = new();
            X64InstructionSelector selector = new(Allocator: allocator);
            IR.Function function = functionDefinition.Value;

            // Emit instructions
            function.Block.Instructions.ForEach(selector.Emit);
            var emitter = new X64AssemblyEmitter(selector.Instructions);

            // Emit Prologue
            WriteLine($"global {function.Name}");
            WriteLine($"{function.Name}:");
            WriteLine("\tPush RBP");
            WriteLine("\tMov RBP, RSP");
            WriteLine($"\tSub RSP, {IAllocator.Align16(allocator.StackSize)}");

            Stack<X64Allocator.Register> calleeRegisters = new (allocator.CalleeSavedRegisters.AsEnumerable());

            // Save callee registers
            foreach (X64Allocator.Register calleeRegister in allocator.CalleeSavedRegisters)
            {
                WriteLine($"\tPush {calleeRegister}");
            }

            emitter.Emit();
            foreach (var line in emitter.Assembly)
            {
                WriteLine(line);
            }

            // Pop callee registers
            while (calleeRegisters.Count > 0) { 
                X64Allocator.Register calleeRegister = calleeRegisters.Pop();
                WriteLine($"\tPop {calleeRegister}");
            }

            // Emit Epilogue
            WriteLine("\tMov RSP, RBP");
            WriteLine("\tPop RBP");
            WriteLine("\tRet");
        }
    }
    
    void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}