using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringComplete;

public abstract class VisitorBase
{
    public void Visit(IAstNode node)
    {
        switch (node)
        {
            case RootNode rootNode:
                Visit(rootNode);
                break;
            case AndNode andNode:
                Visit(andNode);
                break;
            case OrNode orNode:
                Visit(orNode);
                break;
            case NotNode notNode:
                Visit(notNode);
                break;
            case IdentifierNode identifierNode:
                Visit(identifierNode);
                break;
            case ValueNode valueNode:
                Visit(valueNode);
                break;
            default:
                throw new Exception("New node type detected!");
        }
    }

    public virtual void Visit(RootNode node)
    {
        Visit(node.Node);
    }


    public virtual void Visit(AndNode node)
    {
        foreach (IAstNode child in node)
        {
            Visit(child);
        }
    }

    public virtual void Visit(OrNode node)
    {
        foreach (IAstNode child in node)
        {
            Visit(child);
        }
    }

    public virtual void Visit(NotNode node)
    {
        Visit(node.Child);
    }

    public virtual void Visit(IdentifierNode node)
    {

    }

    public virtual void Visit(ValueNode node)
    {

    }
}
