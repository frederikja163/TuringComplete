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
        base.Visit(node);
    }

    public override void Visit(AndNode node)
    {
        AnnulmentLaw(node);
        IdentityLaw(node);
        IdempotentLaw(node);
        ComplementLaw(node);
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
        if (node.Left is ValueNode left && left.Value ||
            node.Right is ValueNode right && right.Value)
        {
            node.Parent.Swap(node, new ValueNode(true));
        }
    }
    // X&0=0
    public static void AnnulmentLaw(AndNode node)
    {
        if (node.Left is ValueNode left && !left.Value ||
            node.Right is ValueNode right && !right.Value)
        {
            node.Parent.Swap(node, new ValueNode(false));
        }
    }

    // X|0=X
    public static void IdentityLaw(OrNode node)
    {
        if (node.Left is ValueNode left && !left.Value)
        {
            node.Parent.Swap(node, node.Right);
        }
        else if (node.Right is ValueNode right && !right.Value)
        {
            node.Parent.Swap(node, node.Left);
        }
    }
    // X&1=X
    public static void IdentityLaw(AndNode node)
    {
        if (node.Left is ValueNode left && left.Value)
        {
            node.Parent.Swap(node, node.Right);
        }
        else if (node.Right is ValueNode right && right.Value)
        {
            node.Parent.Swap(node, node.Left);
        }
    }

    // X|X=X
    public static void IdempotentLaw(OrNode node)
    {
        if (node.Left.Equals(node.Right))
        {
            node.Parent.Swap(node, node.Left);
        }
    }
    // X&X=X
    public static void IdempotentLaw(AndNode node)
    {
        if (node.Left.Equals(node.Right))
        {
            node.Parent.Swap(node, node.Left);
        }
    }

    // X|!X=1
    public static void ComplementLaw(OrNode node)
    {
        if (node.Left is NotNode left && left.Node.Equals(node.Right) ||
            node.Right is NotNode right && right.Node.Equals(node.Left))
        {
            node.Parent.Swap(node, new ValueNode(true));
        }
    }

    // X&!X=0
    public static void ComplementLaw(AndNode node)
    {
        if (node.Left is NotNode left && left.Node.Equals(node.Right) ||
            node.Right is NotNode right && right.Node.Equals(node.Left))
        {
            node.Parent.Swap(node, new ValueNode(false));
        }
    }

    // !!X=X
    public static void DoubleNegationLaw(NotNode node)
    {
        if (node.Node is NotNode child)
        {
            node.Parent.Swap(node, child.Node);
        }
    }

    // Commutative Law.
    // X|Y=Y|X
    // X&Y=Y&X

    // Associative Law.
    // X&(Y&Z)=(X&Y)&Z=(X&Z)&Y=X&Z&Y 
    // X|(Y|Z)=(X|Y)|Z=(X|Z)|Y=X|Z|Y

    // Distributive Law.
    // X|(Y&Z)=(X|Y)&(X|Z)
    // X&(Y|Z)=X&Y|X&Z
}
