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
                new Constant(7),
                OperatorSign.Multiply
            ),
            OperatorSign.Plus
        );

        var rhs = new Function(FunctionKind.Cos, new BinaryOp(
            new Variable("x"),
            new BinaryOp(
                new Constant(4),
                new Constant(10),
                OperatorSign.Plus
            ),
            OperatorSign.Plus
        ));

        var example = new BinaryOp(
            lhs,
            rhs,
            OperatorSign.Plus
        );
        var optimized_non_folded = Optimizer.Optimize(example, false);
        var optimized_folded = Optimizer.Optimize(example, true);

        Console.WriteLine("Expression: x + 2 * 7 + cos(x + 4 + 10)\n");

        Console.WriteLine("==========================");
        Console.WriteLine("Unoptimized:");
        Console.WriteLine("==========================");
        ExprPrinter.PrintAsRTL(example);
        Console.WriteLine("\n==========================");
        Console.WriteLine("Optimized (Not Folded):");
        Console.WriteLine("==========================");
        ExprPrinter.PrintAsRTL(optimized_non_folded);
        Console.WriteLine("\n==========================");
        Console.WriteLine("Optimized (Folded):");
        Console.WriteLine("==========================");
        ExprPrinter.PrintAsRTL(optimized_folded);
    }
}
