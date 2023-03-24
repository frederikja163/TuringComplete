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

    // Annulment.
    [TestCase("A&0", "0")]
    [TestCase("0&A", "0")]
    [TestCase("A|1", "1")]
    [TestCase("1|A", "1")]

    // Identity.
    [TestCase("A&1", "A")]
    [TestCase("1&A", "A")]
    [TestCase("A|0", "A")]
    [TestCase("0|A", "A")]

    // Idempotent.
    [TestCase("A&A", "A")]
    [TestCase("A|A", "A")]

    // Complement.
    [TestCase("A&!A", "0")]
    [TestCase("!A&A", "0")]
    [TestCase("A|!A", "1")]
    [TestCase("!A|A", "1")]

    // Double negation.
    [TestCase("!!A", "A")]
    public void SimplifyTest(string input, string expected)
    {
        IAstNode node = Parser.Parse(input);
        new SimplifyVisitor().Visit(node);
        PrettyPrintVisitor visitor = new PrettyPrintVisitor();
        visitor.Visit(node);
        Assert.That(visitor.ToString(), Is.EqualTo(expected));
    }
}
