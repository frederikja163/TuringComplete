
using TuringComplete;

internal static class Program
{
    internal static void Main(string[] args)
    {
        //string? str = Console.ReadLine();
        var tree = Parser.Parse("(A&!B)|(!A&B)");
        var visitor = new PrettyPrintVisitor();
        visitor.Visit(tree);
        Console.WriteLine(visitor.ToString());
    }
}