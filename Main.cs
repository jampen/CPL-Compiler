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
main.Statements.Add(new AST.DeclareVariable("a", IntLiteral("10", 8), int8));
main.Statements.Add(new AST.DeclareVariable("b", IntLiteral("20", 16), int16));
main.Statements.Add(new AST.IfStatement(
    condition: new AST.Comparison(new AST.Identifier("a"), new AST.Identifier("b"), AST.ComparisonType.LessThan),
    thenBlock: new AST.Block(statements: [
        new AST.DeclareVariable("c", IntLiteral("30", 16), int16)
    ]),
    elseBlock:
    new AST.Block(statements: [
        new AST.DeclareVariable("c", IntLiteral("40", 16), int16)
    ])
    )
);

IR.Context context = new();
main.CodeGen(context);

BE.X64Backend x64 = new();
x64.Generate(context);