namespace ExprReplacer.Printing;

using ExprReplacer.Expressions;

public static class ExprPrinter
{

    public static void PrintAsRTL(IExpression expression)
    {
        // comparer that compares by reference (pointer), not by structural equality
        var refComparer = new ReferenceEqualityComparer<IExpression>();
        int counter = 0;
        var map = new Dictionary<IExpression, string>(refComparer);

        string PrintExpr(IExpression expr)
        {
            // Only consider identical *instances* equal
            if (map.TryGetValue(expr, out var existing))
                return existing;

            var name = $"expr_{counter++}";

            // Important: compute children first so their expr_N lines appear before parent
            switch (expr)
            {
                case IConstantExpression c:
                    Console.WriteLine($"{name}: {c.Value}");
                    break;

                case IVariableExpression v:
                    Console.WriteLine($"{name}: {v.Name}");
                    break;

                case IBinaryExpression bin:
                    {
                        var leftName = PrintExpr(bin.Left);
                        var rightName = PrintExpr(bin.Right);
                        var op = bin.Sign switch
                        {
                            OperatorSign.Plus => "+",
                            OperatorSign.Minus => "-",
                            OperatorSign.Multiply => "*",
                            OperatorSign.Divide => "/",
                            _ => "?"
                        };
                        Console.WriteLine($"{name}: {leftName} {op} {rightName}");
                        break;
                    }

                case IFunction fun:
                    {
                        var argName = PrintExpr(fun.Argument);
                        var fname = fun.Kind switch
                        {
                            FunctionKind.Sin => "sin",
                            FunctionKind.Cos => "cos",
                            FunctionKind.Max => "max",
                            _ => "func"
                        };
                        Console.WriteLine($"{name}: {fname}({argName})");
                        break;
                    }

                default:
                    Console.WriteLine($"{name}: <unknown>");
                    break;
            }

            map[expr] = name;
            return name;
        }

        var result = PrintExpr(expression);
        Console.WriteLine($"result: {result}");
    }
}
