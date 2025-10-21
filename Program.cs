using ExprReplacer.Expressions;
using ExprReplacer.Printing;
using ExprReplacer.Optimization;

class Program
{
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
        var optimized = Optimizer.Optimize(example);

        Console.WriteLine("Expression: x + 2 * y + cos(x + 2 * y)\n");

        Console.WriteLine("==========================");
        Console.WriteLine("Unoptimized:");
        Console.WriteLine("==========================");
        ExprPrinter.PrintAsRTL(example);
        Console.WriteLine("\n==========================");
        Console.WriteLine("Optimized:");
        Console.WriteLine("==========================");
        ExprPrinter.PrintAsRTL(optimized);
    }
}
