class ExprReplacer
{
    interface IExpression;

    interface IConstantExpression : IExpression { int Value { get; } }
    interface IVariableExpression : IExpression { string Name { get; } }
    interface IBinaryExpression : IExpression { IExpression Left { get; } IExpression Right { get; } OperatorSign Sign { get; } }
    interface IFunction : IExpression { FunctionKind Kind { get; } IExpression Argument { get; } }

    enum FunctionKind { Sin, Cos, Max }
    enum OperatorSign { Plus, Minus, Multiply, Divide }

    record Constant(int Value) : IConstantExpression;
    record Variable(string Name) : IVariableExpression;
    record BinaryOp(IExpression Left, IExpression Right, OperatorSign Sign) : IBinaryExpression;
    record Function(FunctionKind Kind, IExpression Argument) : IFunction;

    // Creating new expressions
    static IExpression CreateExample()
    {
        // Creates: (x + 5) * 2
        var x = new Variable("x");
        var five = new Constant(5);
        var xPlus5 = new BinaryOp(x, five, OperatorSign.Plus);
        var two = new Constant(2);
        var result = new BinaryOp(xPlus5, two, OperatorSign.Multiply);

        return result;
    }

    static bool ExprEq(IExpression a, IExpression b)
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
                    (ExprEq(bin.Left, bin2.Left) &&
                        ExprEq(bin.Right, bin2.Right))
                || ExprEq(bin.Right, bin2.Left) &&
                        ExprEq(bin.Left, bin2.Right)),
            // We can replace the function calls because we assume they have no side-effects
            IFunction f => b is IFunction f2 && f.Kind == f2.Kind && ExprEq(f.Argument, f2.Argument),
            _ => false
        };

    }

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
            if (ExprEq(e, expression))
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

    static IExpression Optimize(IExpression expression)
    {
        return OptAux(expression, new HashSet<IExpression>());
    }


    static void Main()
    {
        var lhs = new BinaryOp(
            new Variable("x"),
            new BinaryOp(
                new Constant(2),
                new Variable("y"),
                OperatorSign.Multiply
            ),
            OperatorSign.Plus
        );

        var rhs = new Function(FunctionKind.Cos, new BinaryOp(
            new Variable("x"),
            new BinaryOp(
                new Constant(2),
                new Variable("y"),
                OperatorSign.Multiply
            ),
            OperatorSign.Plus
        ));

        var example = new BinaryOp(
            lhs,
            rhs,
            OperatorSign.Plus
        );
        var optimized = Optimize(example);

        Console.WriteLine(example);
        Console.WriteLine(optimized);
    }

}
