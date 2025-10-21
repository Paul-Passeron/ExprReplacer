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

    static IExpression FoldBinOp(IBinaryExpression bin)
    {
        var folded_l = Fold(bin.Left);
        var folded_r = Fold(bin.Right);
        if (folded_l is IConstantExpression lhs && folded_r is IConstantExpression rhs)
        {
            return bin.Sign switch
            {
                OperatorSign.Plus => new Constant(lhs.Value + rhs.Value),
                OperatorSign.Minus => new Constant(lhs.Value - rhs.Value),
                OperatorSign.Multiply => new Constant(lhs.Value * rhs.Value),
                OperatorSign.Divide => new Constant(lhs.Value / rhs.Value),
                _ => bin, // Unreachable
            }
        ;
        }

        return new BinaryOp(folded_l, folded_r, bin.Sign);
    }

    public static IExpression Fold(IExpression expression)
    {
        return expression switch
        {
            IConstantExpression c => c,
            IBinaryExpression bin => FoldBinOp(bin),
            IFunction fun => new Function(fun.Kind, Fold(fun.Argument)),
            _ => expression
        };
    }

    public static IExpression Optimize(IExpression expression, bool fold)
    {
        var start = fold ? Fold(expression) : expression;
        return OptAux(start, new HashSet<IExpression>());
    }


}
