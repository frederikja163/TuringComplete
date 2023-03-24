using TuringComplete;

IAstNode node = Parser.Parse("1&X&0|1");
new SimplifyVisitor().Visit(node);
var prettyPrinter = new PrettyPrintVisitor();
prettyPrinter.Visit(node);
Console.WriteLine(prettyPrinter.ToString());