namespace CPL.IR;

internal class VariableAlreadyDefinedException(string Name)
    : Exception($"Variable '{Name}' is already defined.");

internal class FunctionAlreadyDefinedException(string Name)
    : Exception($"Function '{Name}' is already defined.");

internal class Context
{
    private Stack<Block> blockStack = new();
    public List<Block> Blocks { get; } = new();
    public Block CurrentBlock {  get {  return blockStack.Peek(); } }
    public Dictionary<string, Function> Functions { get; } = new();
    public Function? CurrentFunction { get; private set; } = null;

    public Block EnterBlock(string name)
    {
        var block = new Block();
        blockStack.Push(block);
        Blocks.Add(block);
        return block;
    }

    public Block ExitBlock()
    {
        var block = CurrentBlock;
        blockStack.Pop();
        if (CurrentFunction != null && block == CurrentFunction.Block)
        {
            CurrentFunction = null;
        }
        return block;
    }

    public Value DeclareVariable(string name, AST.Type type)
    {
        var irType = TypeConverter.Convert(type);
        var variable = new Variable(name, irType);
        CurrentBlock.DeclareVariable(variable);
        Emit(new DeclareVariable(variable));
        return variable;
    }

    public Function DeclareFunction(string name, AST.Type returnType)
    {
        if (Functions.ContainsKey(name))
        {
            throw new FunctionAlreadyDefinedException(name);
        }
        var irReturnType = TypeConverter.Convert(returnType);
        var block = EnterBlock(name);
        var function = new Function(name, block, irReturnType);
        CurrentFunction = function;
        Functions[name] = function;
        return function;
    } 

    public void Emit(Instruction instruction)
    {
        CurrentBlock.Instructions.Add(instruction);
    }

    public Variable FindVariable(string name)
    {
        foreach(var block in blockStack.Reverse())
        {
            if (block.Variables.TryGetValue(name, out Variable? value))
            {
                return value;
            }
        }
        return null;
    }
}