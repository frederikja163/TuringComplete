using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringComplete;

public sealed class PrettyPrintVisitor : VisitorBase
{
    private readonly StringBuilder _output = new StringBuilder();

    public override void Visit(OrNode node)
    {
        Visit(node.Left);
        _output.Append("|");
        Visit(node.Right);
    }

    public override void Visit(AndNode node)
    {
        if (node.Left is OrNode)
        {
            _output.Append('(');
            Visit(node.Left);
            _output.Append(')');
        }
        else
        {
            Visit(node.Left);
        }

        _output.Append("&");

        if (node.Right is OrNode)
        {
            _output.Append('(');
            Visit(node.Right);
            _output.Append(')');
        }
        else
        {
            Visit(node.Right);
        }
    }


    public override void Visit(NotNode node)
    {
        _output.Append('!');
        if (node.Node is OrNode || node.Node is AndNode)
        {
            _output.Append('(');
            Visit(node.Node);
            _output.Append(')');
        }
        else
        {
            Visit(node.Node);
        }
    }

    public override void Visit(IdentifierNode node)
    {
        _output.Append(node.Identifier);
    }

    public override void Visit(ValueNode node)
    {
        _output.Append(node.Value ? '1' : '0');
    }

    public override string ToString()
    {
        return _output.ToString();
    }
}

internal sealed class ExplicitPrintVisitor : VisitorBase
{
    private readonly StringBuilder _output = new StringBuilder();

    public override void Visit(AndNode node)
    {
        Visit(node.Left);
        _output.Append("&");
        Visit(node.Right);
    }

    public override void Visit(OrNode node)
    {
        _output.Append('(');
        Visit(node.Left);
        _output.Append("|");
        Visit(node.Right);
        _output.Append(')');
    }

    public override void Visit(NotNode node)
    {
        _output.Append('!');
        Visit(node.Node);
    }

    public override void Visit(IdentifierNode node)
    {
        _output.Append(node.Identifier);
    }
    public override void Visit(ValueNode node)
    {
        _output.Append(node.Value ? '1' : '0');
    }

    public override string ToString()
    {
        return _output.ToString();
    }
}
