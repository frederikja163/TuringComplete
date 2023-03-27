using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringComplete;

namespace Tests;

internal class Visitor
{
    [TestCase("A", "A")]
    [TestCase("!A", "!A")]
    [TestCase("A|B", "A|B")]
    [TestCase("(A|B)", "A|B")]
    [TestCase("((A)|B)", "A|B")]
    [TestCase("(A&C)|B", "A&C|B")]
    [TestCase("A&(C|B)", "A&(C|B)")]
    [TestCase("!(A&C)", "!(A&C)")]
    [TestCase("A&f", "A&0")]
    public void PrettyPrintTest(string input, string expected)
    {
        IAstNode node = Parser.Parse(input);
        PrettyPrintVisitor visitor = new PrettyPrintVisitor();
        visitor.Visit(node);
        Assert.That(visitor.ToString(), Is.EqualTo(expected));
    }

    private static void SimplifyTest(string input, string expected)
    {
        IAstNode node = Parser.Parse(input);
        new SimplifyVisitor().Visit(node);
        PrettyPrintVisitor visitor = new PrettyPrintVisitor();
        visitor.Visit(node);
        Assert.That(visitor.ToString(), Is.EqualTo(expected));
    }

    [TestCase("A&0", "0")]
    [TestCase("0&A", "0")]
    [TestCase("A|1", "1")]
    [TestCase("1|A", "1")]
    public void Annulment(string input, string expected)
        => SimplifyTest(input, expected);

    [TestCase("A&1", "A")]
    [TestCase("1&A", "A")]
    [TestCase("A|0", "A")]
    [TestCase("0|A", "A")]
    public void Identity(string input, string expected)
        => SimplifyTest(input, expected);

    [TestCase("A&A", "A")]
    [TestCase("A|A", "A")]
    public void Idempotent(string input, string expected)
        => SimplifyTest(input, expected);

    [TestCase("A&!A", "0")]
    [TestCase("!A&A", "0")]
    [TestCase("A|!A", "1")]
    [TestCase("!A|A", "1")]
    public void Complement(string input, string expected)
        => SimplifyTest(input, expected);

    [TestCase("!!A", "A")]
    public void DoubleNegation(string input, string expected)
        => SimplifyTest(input, expected);

    [TestCase("A&(B&C)", "A&B&C")]
    [TestCase("A|(B|C)", "A|B|C")]
    public void AssociativeLaw(string input, string expected)
        => SimplifyTest(input, expected);
}
