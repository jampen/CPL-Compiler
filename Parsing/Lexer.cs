namespace CPL.Parsing;

internal class Lexer
{
    private string source;
    private int sourceLength = 0;
    private int lexemeBegin = 0;
    private int lexemeEnd = 0;

    public Lexer(string source)
    {
        if (source.Length == 0)
        {
            throw new ArgumentException("Empty source string");
        }
        this.source = source;
        sourceLength = source.Length - 1;
    }

    public List<Token> Tokenize()
    {
        lexemeBegin = 0;
        lexemeEnd = 0;

        while (!IsAtEnd())
        {
            char current = Current();
            lexemeBegin = lexemeEnd;
            Next();
        }

        return new();
    }

    private bool IsAtEnd() => lexemeEnd >= sourceLength;
    private char Current() => source[lexemeEnd];
    private char Next()
    {
        if (IsAtEnd()) return '\0';
        char ch = Current();
        ++lexemeEnd;
        return ch;
    }

}