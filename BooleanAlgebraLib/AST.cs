#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Xml.Linq;

namespace TuringComplete;

public interface IAstNode : IEquatable<IAstNode>
{
    IAstNode? Parent { get; internal set; }

    void Swap(IAstNode oldNode, IAstNode newNode);

}

public class RootNode : IAstNode
{
    private IAstNode _node;

    public RootNode(IAstNode node)
    {
        Node = node;
    }

    public IAstNode Node
    {
        get => _node;
        private set
        {
            _node = value;
            _node.Parent = this;
        }
    }

    public IAstNode? Parent { get => null; set => throw new Exception("Root node cannot have a parent."); }

    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        if (oldNode == Node)
        {

            Node = newNode;
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is RootNode rootNode && rootNode.Node.Equals(Node);
    }
}

public class OrNode : IAstNode
{
    private IAstNode _left;
    private IAstNode _right;

    public OrNode(IAstNode left, IAstNode right)
    {
        Left = left;
        Right = right;
    }

    public IAstNode Left
    {
        get => _left;
        private set
        {
            _left = value;
            _left.Parent = this;
        }
    }
    public IAstNode Right
    {
        get => _right;
        private set
        {
            _right = value;
            _right.Parent = this;
        }
    }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }
    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        if (Left == oldNode)
        {
            Left = newNode;
        }
        else if (Right == oldNode)
        {
            Right = newNode;
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is OrNode orNode && orNode.Left.Equals(Left) && orNode.Right.Equals(Right);
    }
}

public class AndNode : IAstNode
{
    private IAstNode _left;
    private IAstNode _right;

    public AndNode(IAstNode left, IAstNode right)
    {
        Left = left;
        Right = right;
    }

    public IAstNode Left
    {
        get => _left;
        private set
        {
            _left = value;
            _left.Parent = this;
        }
    }
    public IAstNode Right
    {
        get => _right;
        private set
        {
            _right = value;
            _right.Parent = this;
        }
    }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }

    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        if (Left == oldNode)
        {
            Left = newNode;
        }
        else if (Right == oldNode)
        {
            Right = newNode;
        }
    }
    public bool Equals(IAstNode? other)
    {
        return other is AndNode andNode && andNode.Left.Equals(Left) && andNode.Right.Equals(Right);
    }
}

public class NotNode : IAstNode
{
    private IAstNode _node;

    public NotNode(IAstNode node)
    {
        Node = node;
    }


    public IAstNode Node
    {
        get => _node;
        private set
        {
            _node = value;
            _node.Parent = this;
        }
    }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }

    public IAstNode Parent { get; internal set; }

    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        if (oldNode == Node)
        {
            Node = newNode;
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is NotNode notNode && notNode.Node.Equals(Node);
    }
}

public class IdentifierNode : IAstNode
{
    public IdentifierNode(char identifier)
    {
        Identifier = identifier;
    }

    public char Identifier { get; }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }

    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        
    }

    public bool Equals(IAstNode? other)
    {
        return other is IdentifierNode identifierNode && identifierNode.Identifier.Equals(Identifier);
    }
}

public class ValueNode : IAstNode
{
    public ValueNode(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }

    public void Swap(IAstNode oldNode, IAstNode newNode)
    {
        
    }

    public bool Equals(IAstNode? other)
    {
        return other is ValueNode valueNode && valueNode.Value.Equals(Value);
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.