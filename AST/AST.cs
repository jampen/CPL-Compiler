namespace CPL.AST;

internal interface INode
{
    public IR.Value CodeGen(IR.Context context);
}