using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringComplete;

// Laws based on: https://www.mi.mun.ca/users/cchaulk/misc/boolean.htm
public sealed class SimplifyVisitor : VisitorBase
{
    public override void Visit(OrNode node)
    {
        AnnulmentLaw(node);
        IdentityLaw(node);
        IdempotentLaw(node);
        ComplementLaw(node);
        AssociativeLaw(node);
        base.Visit(node);
    }

    public override void Visit(AndNode node)
    {
        AnnulmentLaw(node);
        IdentityLaw(node);
        IdempotentLaw(node);
        ComplementLaw(node);
        AssociativeLaw(node);
        base.Visit(node);
    }

    public override void Visit(NotNode node)
    {
        DoubleNegationLaw(node);
        base.Visit(node);
    }

    public override void Visit(IdentifierNode node)
    {
        base.Visit(node);
    }

    // X|1=1
    public static void AnnulmentLaw(OrNode node)
    {
        foreach (IAstNode child in node)
        {
            if (child is ValueNode valueNode && valueNode.Value)
            {
                node.Parent.Swap(node, new ValueNode(true));
                return;
            }
        }
    }
    // X&0=0
    public static void AnnulmentLaw(AndNode node)
    {
        foreach (IAstNode child in node)
        {
            if (child is ValueNode valueNode && !valueNode.Value)
            {
                node.Parent.Swap(node, new ValueNode(false));
                return;
            }
        }
    }

    // X|0=X
    public static void IdentityLaw(OrNode node)
    {
        foreach (IAstNode child in node)
        {
            if (child is ValueNode valueNode && !valueNode.Value)
            {
                node.Swap(child, null);
            }
        }
    }
    // X&1=X
    public static void IdentityLaw(AndNode node)
    {
        foreach (IAstNode child in node)
        {
            if (child is ValueNode valueNode && valueNode.Value)
            {
                node.Swap(child, null);
            }
        }
    }

    // X|X=X
    public static void IdempotentLaw(OrNode node)
    {
        List<IAstNode> seenNodes = new List<IAstNode>();
        foreach(IAstNode child in node)
        {
            if (seenNodes.Contains(child))
            {
                node.Swap(child, null);
            }
            else
            {
                seenNodes.Add(child);
            }
        }
    }
    // X&X=X
    public static void IdempotentLaw(AndNode node)
    {
        List<IAstNode> seenNodes = new List<IAstNode>();
        foreach (IAstNode child in node)
        {
            if (seenNodes.Contains(child))
            {
                node.Swap(child, null);
            }
            else
            {
                seenNodes.Add(child);
            }
        }
    }

    // X|!X=1
    public static void ComplementLaw(OrNode node)
    {
        foreach(NotNode notNode in node.OfType<NotNode>())
        {
            if (node.Any(n => n.Equals(notNode.Child)))
            {
                node.Parent.Swap(node, new ValueNode(true));
            }
        }
    }

    // X&!X=0
    public static void ComplementLaw(AndNode node)
    {
        foreach (NotNode notNode in node.OfType<NotNode>())
        {
            if (node.Any(n => n.Equals(notNode.Child)))
            {
                node.Parent.Swap(node, new ValueNode(false));
            }
        }
    }

    // !!X=X
    public static void DoubleNegationLaw(NotNode node)
    {
        if (node.Child is NotNode child)
        {
            node.Parent.Swap(node, child.Child);
        }
    }

    // Commutative Law.
    // X|Y=Y|X
    // X&Y=Y&X

    // Associative Law.
    // X&(Y&Z)=(X&Y)&Z=(X&Z)&Y=X&Z&Y
    public static void AssociativeLaw(AndNode node)
    {
        foreach (AndNode childNode in node.OfType<AndNode>())
        {
            AssociativeLaw(childNode);
            node.Swap(childNode, null);
            foreach (IAstNode childChildNode in childNode)
            {
                node.Swap(null, childChildNode);
            }
        }
    }

    // X|(Y|Z)=(X|Y)|Z=(X|Z)|Y=X|Z|Y
    public static void AssociativeLaw(OrNode node)
    {
        foreach (OrNode childNode in node.OfType<OrNode>())
        {
            AssociativeLaw(childNode);
            node.Swap(childNode, null);
            foreach (IAstNode childChildNode in childNode)
            {
                node.Swap(null, childChildNode);
            }
        }
    }

    // Distributive Law.
    // X|(Y&Z)=(X|Y)&(X|Z)
    // X&(Y|Z)=(X&Y)|(X&Z)
}
