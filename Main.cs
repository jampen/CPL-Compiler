using AST = CPL.AST;
using IR = CPL.IR;
using BE = CPL.BE;

var void_t = new AST.VoidType();
var int32 = new AST.IntType(32, AST.Signedness.Signed, AST.Constness.Mutable);

static AST.INode Int32Literal(string value)
{
    return new AST.Literal(value, AST.LiteralType.Number);
}

var main = new AST.Function("main", void_t);
main.Statements.Add(new AST.DeclareVariable("x", Int32Literal("10"), int32));
main.Statements.Add(new AST.DeclareVariable("y", Int32Literal("20"), int32));

IR.Context context = new();
main.CodeGen(context);

BE.X64Backend x64 = new();
x64.Generate(context);