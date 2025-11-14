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
main.Statements.Add(new AST.DeclareVariable("a", 
    new AST.Comparison(
        IntLiteral("10", 32),
        IntLiteral("20", 32),
        AST.ComparisonType.LessThan)
    , int32));


main.Statements.Add(new AST.DeclareVariable("b",
    new AST.Comparison(
        new AST.Identifier("a"),
        IntLiteral("500", 32),
        AST.ComparisonType.LessThan)
    , int32));


IR.Context context = new();
main.CodeGen(context);

BE.X64Backend x64 = new();
x64.Generate(context);

