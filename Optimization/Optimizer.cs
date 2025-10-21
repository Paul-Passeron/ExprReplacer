namespace ExprReplacer.Optimization;

using ExprReplacer.Expressions;

public static class Optimizer
{

    static IExpression OptimizeBinOp(IBinaryExpression bin, HashSet<IExpression> set)
    {
        var optimized_l = OptAux(bin.Left, set);
        var optimized_r = OptAux(bin.Right, set);
        return new BinaryOp(optimized_l, optimized_r, bin.Sign);
    }


    static IExpression OptAux(IExpression expression, HashSet<IExpression> set)
    {
        IExpression? present = null;

        foreach (var e in set)
        {
            if (ExprEquality.ExprEq(e, expression))
            {
                present = e;
                break;
            }
        }

        IExpression expr;
        if (present != null)
        {
            expr = present;
        }
        else
        {
            expr = expression switch
            {
                IConstantExpression c => c,
                IBinaryExpression bin => OptimizeBinOp(bin, set),
                IFunction fun => new Function(fun.Kind, OptAux(fun.Argument, set)),
                _ => expression
            }
            ;
        }
        set.Add(expr);
        return expr;
    }

    public static IExpression Optimize(IExpression expression)
    {
        return OptAux(expression, new HashSet<IExpression>());
    }


}
