using CPL.IR;

namespace CPL.BE;

internal abstract class InstructionSelector
{
    public void Emit(IR.Instruction instruction)
    {
        switch (instruction)
        {
            case IR.Function function: Function(function); break;
            case IR.Block block: Block(block); break;
            case IR.DeclareVariable declareVariable: DeclareVariable(declareVariable); break;
            case IR.Store store: Store(store); break;
            case IR.Load load: Load(load); break;
            case IR.Comparison comparison: Comparison(comparison); break;
            case IR.IfStatement ifStatement: IfStatement(ifStatement); break;
        }
    }

    protected abstract void Function(IR.Function function);
    protected abstract void Block(IR.Block block);
    protected abstract void DeclareVariable(IR.DeclareVariable declareVariable);
    protected abstract void Store(IR.Store store);
    protected abstract void Load(IR.Load load);
    protected abstract void Comparison(IR.Comparison comparison);
    protected abstract void IfStatement(IR.IfStatement ifStatement);
}