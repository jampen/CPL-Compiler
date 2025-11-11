using AST = CPL.AST;
using IR = CPL.IR;
using BE = CPL.BE;

var void_t = new AST.VoidType();
var int8 = new AST.IntType(8, AST.Signedness.Signed, AST.Constness.Mutable);
var int16 = new AST.IntType(16, AST.Signedness.Signed, AST.Constness.Mutable);
var int32 = new AST.IntType(32, AST.Signedness.Signed, AST.Constness.Mutable);
var int64 = new AST.IntType(64, AST.Signedness.Signed, AST.Constness.Mutable);

static AST.INode IntLiteral(string value, int width)
{
    return new AST.Literal(value, AST.LiteralType.Number, width);
}

var main = new AST.Function("main", void_t);
main.Statements.Add(new AST.DeclareVariable("a", IntLiteral("10", 64), int64));
main.Statements.Add(new AST.DeclareVariable("b", new AST.Identifier("a"), int64));
main.Statements.Add(new AST.DeclareVariable("c", new AST.Identifier("b"), int64));
main.Statements.Add(new AST.DeclareVariable("d", new AST.Identifier("c"), int64));
main.Statements.Add(new AST.DeclareVariable("e", new AST.Identifier("d"), int64));
main.Statements.Add(new AST.DeclareVariable("f", new AST.Identifier("e"), int64));
main.Statements.Add(new AST.DeclareVariable("g", new AST.Identifier("f"), int64));
main.Statements.Add(new AST.DeclareVariable("h", new AST.Identifier("g"), int64));
main.Statements.Add(new AST.DeclareVariable("i", new AST.Identifier("h"), int64));
main.Statements.Add(new AST.DeclareVariable("j", new AST.Identifier("i"), int64));
main.Statements.Add(new AST.DeclareVariable("k", new AST.Identifier("j"), int64));
main.Statements.Add(new AST.DeclareVariable("l", new AST.Identifier("k"), int64));
main.Statements.Add(new AST.DeclareVariable("m", new AST.Identifier("l"), int64));
main.Statements.Add(new AST.DeclareVariable("n", new AST.Identifier("m"), int64));
main.Statements.Add(new AST.DeclareVariable("o", new AST.Identifier("n"), int64));
main.Statements.Add(new AST.DeclareVariable("p", new AST.Identifier("o"), int64));
main.Statements.Add(new AST.DeclareVariable("q", new AST.Identifier("p"), int64));
main.Statements.Add(new AST.DeclareVariable("r", IntLiteral("20", 64), int64));
main.Statements.Add(new AST.DeclareVariable("s", new AST.Identifier("r"), int64));

IR.Context context = new();
main.CodeGen(context);

BE.X64Backend x64 = new();
x64.Generate(context);