#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Collections;
using System.Xml.Linq;

namespace TuringComplete;

public interface IAstNode : IEquatable<IAstNode>
{
    IAstNode? Parent { get; internal set; }

    void Swap(IAstNode? oldNode, IAstNode? newNode);

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

    public void Swap(IAstNode? oldNode, IAstNode? newNode)
    {
        if (oldNode == Node && newNode is not null)
        {
            Node = newNode;
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is RootNode rootNode && rootNode.Node.Equals(Node);
    }
}

public class OrNode : IAstNode, IEnumerable<IAstNode>
{
    private readonly List<IAstNode> _children = new List<IAstNode>();

    public OrNode(List<IAstNode> children)
    {
        _children = children;
    }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }
    public void Swap(IAstNode? oldNode, IAstNode? newNode)
    {
        if (oldNode is not null && newNode is not null)
        {
            int index = _children.IndexOf(oldNode);
            _children[index] = newNode;
        }
        else if (oldNode is null && newNode is not null)
        {
            _children.Add(newNode);
        }
        else if (oldNode is not null && newNode is null)
        {
            _children.Remove(oldNode);
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is OrNode node && !node.Any(n => !_children.Contains(n));
    }

    public IEnumerator<IAstNode> GetEnumerator()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            yield return _children[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public class AndNode : IAstNode, IEnumerable<IAstNode>
{
    private readonly List<IAstNode> _children = new List<IAstNode>();

    public AndNode(List<IAstNode> children)
    {
        _children = children;
    }

    IAstNode? IAstNode.Parent
    {
        get => Parent;
        set => Parent = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IAstNode Parent { get; internal set; }

    public void Swap(IAstNode? oldNode, IAstNode? newNode)
    {
        if (oldNode is not null && newNode is not null)
        {
            int index = _children.IndexOf(oldNode);
            _children[index] = newNode;
        }
        else if (oldNode is null && newNode is not null)
        {
            _children.Add(newNode);
        }
        else if (oldNode is not null && newNode is null)
        {
            _children.Remove(oldNode);
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is AndNode node && !node.Any(n => !_children.Contains(n));
    }

    public IEnumerator<IAstNode> GetEnumerator()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            yield return _children[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class NotNode : IAstNode
{
    private IAstNode _node;

    public NotNode(IAstNode node)
    {
        Child = node;
    }


    public IAstNode Child
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

    public void Swap(IAstNode? oldNode, IAstNode? newNode)
    {
        if (oldNode == Child && newNode is not null)
        {
            Child = newNode;
        }
    }

    public bool Equals(IAstNode? other)
    {
        return other is NotNode notNode && notNode.Child.Equals(Child);
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

    public void Swap(IAstNode? oldNode, IAstNode? newNode)
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

    public void Swap(IAstNode? oldNode, IAstNode? newNode)
    {
        
    }

    public bool Equals(IAstNode? other)
    {
        return other is ValueNode valueNode && valueNode.Value.Equals(Value);
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.