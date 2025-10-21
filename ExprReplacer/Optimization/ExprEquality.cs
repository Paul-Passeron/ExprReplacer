namespace ExprReplacer.Optimization;

using ExprReplacer.Expressions;

public static class ExprEquality
{

    public static bool Equals(IExpression a, IExpression b)
    {
        // If they are the same instance then we are done
        if (ReferenceEquals(a, b)) return true;
        return a switch
        {
            IConstantExpression c => b is IConstantExpression c2 && c.Value == c2.Value,
            IVariableExpression v => b is IVariableExpression v2 && v.Name == v2.Name,
            IBinaryExpression bin =>
                // We are dealing with integers so + - * and / are commutative
                b is IBinaryExpression bin2 &&
                bin.Sign == bin2.Sign &&
                (
                    (Equals(bin.Left, bin2.Left) &&
                        Equals(bin.Right, bin2.Right))
                || Equals(bin.Right, bin2.Left) &&
                        Equals(bin.Left, bin2.Right)),
            // We can replace the function calls because we assume they have no side-effects
            IFunction f => b is IFunction f2 && f.Kind == f2.Kind && Equals(f.Argument, f2.Argument),
            _ => false
        };

    }

}
