using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuringComplete;

namespace BooleanAlgebraLib.Simplify;

public sealed class AnnulmentVisitor : VisitorBase
{
    public override void Visit(OrNode node)
    {
        base.Visit(node);
    }

    public override void Visit(AndNode node)
    {
        base.Visit(node);
    }
}
