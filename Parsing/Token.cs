namespace CPL.Parsing;

internal record Token(string Text, TokenType Type);

public enum TokenType
{
    Identifier,

    // Literals
    Number,
    String,

    // Keywords
    Function,
    Return,
    If,
    Else,
    While,

    // Data types
    Byte,
    Int,
    Short,
    Long,

    // Punctuation
    LeftCurlyBracket,
    RightCurlyBracket,
    LeftParen,
    RightParen,
    LeftSquareBracket,
    RightSquareBracket,
    Colon,

    // Comparison
    Equals,
    EqualsEquals,
    Not,
    NotEquals,
    LessThan,
    LessThanEquals,
    GreaterThan,
    GreaterThanEquals,

    // Math
    Plus,
    Minus,
    Star,
    Slash,
}
