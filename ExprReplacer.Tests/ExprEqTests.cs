using Xunit;
using ExprReplacer.Expressions;
using ExprReplacer.Optimization;

namespace ExprReplacer.Tests;

public class ExprEqualityTests
{
    [Fact]
    public void ConstantEquality_Works()
    {
        var a = new Constant(5);
        var b = new Constant(5);
        var c = new Constant(6);

        Assert.True(ExprEquality.Equals(a, b));
        Assert.False(ExprEquality.Equals(a, c));
    }

    [Fact]
    public void VariableEquality_Works()
    {
        var x1 = new Variable("x");
        var x2 = new Variable("x");
        var y = new Variable("y");

        Assert.True(ExprEquality.Equals(x1, x2));
        Assert.False(ExprEquality.Equals(x1, y));
    }

    [Fact]
    public void BinaryEquality_IsCommutative()
    {
        var a = new Variable("a");
        var b = new Variable("b");

        var expr1 = new BinaryOp(a, b, OperatorSign.Plus);
        var expr2 = new BinaryOp(b, a, OperatorSign.Plus);

        Assert.True(ExprEquality.Equals(expr1, expr2));
    }

    [Fact]
    public void FunctionEquality_Works()
    {
        var f1 = new Function(FunctionKind.Sin, new Variable("x"));
        var f2 = new Function(FunctionKind.Sin, new Variable("x"));
        var f3 = new Function(FunctionKind.Cos, new Variable("x"));

        Assert.True(ExprEquality.Equals(f1, f2));
        Assert.False(ExprEquality.Equals(f1, f3));
    }
}
