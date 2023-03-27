using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringComplete;

public enum TokenType
{
    And,
    Not,
    Or,
    LeftParenthesis,
    RightParenthesis,
    Identifier,
    True,
    False,
}

public record Token (TokenType Type, char Match);

internal sealed class TokenStream : IEnumerator<Token>
{
    private IEnumerable<Token> _tokens;

    public TokenStream(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
    }

    public TokenStream(string str)
    {
        _tokens = Tokenizer.Tokenize(str);
    }

    public Token Current => _tokens.First();

    object IEnumerator.Current => _tokens.First();

    public void Dispose()
    {

    }

    public bool Peek([NotNullWhen(true)] out Token? token)
    {
        token = _tokens.Skip(1).FirstOrDefault();
        return _tokens.Count() > 1;
    }

    public bool Accept(TokenType type) => Accept(type, out Token _);

    public bool Accept(TokenType type, [NotNullWhen(true)] out Token? token)
    {
        if (!_tokens.Any())
        {
            token = null;
            return false;
        }

        token = Current;
        if (Current.Type == type)
        {
            MoveNext();
            return true;
        }
        return false;
    }

    public bool MoveNext()
    {
        _tokens = _tokens.Skip(1);
        return _tokens.Any();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}

internal static class Tokenizer
{
    [Pure]
    public static IEnumerable<Token> Tokenize(string str)
    {
        foreach (char c in str)
        {
            switch (c)
            {
                // And.
                case '*':
                case '&':
                    yield return new Token(TokenType.And, c);
                    break;
                // Or.
                case '+':
                case '|':
                    yield return new Token(TokenType.Or, c);
                    break;
                // Not.
                case '!':
                    yield return new Token(TokenType.Not, c);
                    break;
                // Left Parenthesis.
                case '(':
                    yield return new Token(TokenType.LeftParenthesis, c);
                    break;
                // Right Parenthesis.
                case ')':
                    yield return new Token(TokenType.RightParenthesis, c);
                    break;
                case 'T':
                case 't':
                case '1':
                    yield return new Token(TokenType.True, c);
                    break;
                case 'F':
                case 'f':
                case '0':
                    yield return new Token(TokenType.False, c);
                    break;
                default:
                    // Identifier.
                    if (char.IsLetter(c))
                    {
                        yield return new Token(TokenType.Identifier, c);
                        break;
                    }
                    // Ignore whitespace.
                    else if (char.IsWhiteSpace(c))
                    {
                        break;
                    }
                    throw new Exception($"{c} is an unrecognised token.");
            }
        }
    }
}
