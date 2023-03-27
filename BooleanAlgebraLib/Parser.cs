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
        List<IAstNode> children = new List<IAstNode>() { ParseAnd(stream) };
        while (stream.Accept(TokenType.Or))
        {
            children.Add(ParseOr(stream));
        }
        if (children.Count == 1)
        {
            return children[0];
        }
        return new OrNode(children);
    }

    private static IAstNode ParseAnd(TokenStream stream)
    {
        List<IAstNode> children = new List<IAstNode>() { ParseNot(stream) };
        while (stream.Accept(TokenType.And))
        {
            children.Add(ParseAnd(stream));
        }
        if (children.Count == 1)
        {
            return children[0];
        }
        return new AndNode(children);
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
        else if (stream.Accept(TokenType.Identifier, out Token? token))
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
