using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringComplete;

public static class Parser
{
    public static IAstNode Parse(string str) => Parse(new TokenStream(str));

    internal static IAstNode Parse(TokenStream stream) => new RootNode(ParseOr(stream));

    private static IAstNode ParseOr(TokenStream stream)
    {
        IAstNode node = ParseAnd(stream);
        if (stream.Accept(TokenType.Or))
        {
            return new OrNode(node, ParseOr(stream));
        }
        return node;
    }

    private static IAstNode ParseAnd(TokenStream stream)
    {
        IAstNode node = ParseNot(stream);
        if (stream.Accept(TokenType.And))
        {
            return new AndNode(node, ParseAnd(stream));
        }
        return node;
    }

    private static IAstNode ParseNot(TokenStream stream)
    {
        if (stream.Accept(TokenType.Not))
        {
            return new NotNode(ParseNot(stream));
        }
        return ParseExpression(stream);
    }

    private static IAstNode ParseExpression(TokenStream stream)
    {
        if (stream.Accept(TokenType.LeftParenthesis))
        {
            IAstNode retVal = ParseOr(stream);
            if (stream.Accept(TokenType.RightParenthesis))
            {
                return retVal;
            }
            throw new Exception("Expected ')'");
        }
        else if (stream.Accept(TokenType.Identifier, out Token token))
        {
            return new IdentifierNode(token.Match);
        }
        else if (stream.Accept(TokenType.True))
        {
            return new ValueNode(true);
        }
        else if (stream.Accept(TokenType.False))
        {
            return new ValueNode(false);
        }
        throw new Exception("Expected 'identifier'");
    }
}
