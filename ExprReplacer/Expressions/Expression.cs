namespace ExprReplacer.Expressions;

public interface IExpression;

public interface IConstantExpression : IExpression
{
    int Value { get; }
}

public interface IVariableExpression : IExpression
{
    string Name { get; }
}

public interface IBinaryExpression : IExpression
{
    IExpression Left { get; }
    IExpression Right { get; }
    OperatorSign Sign { get; }
}

public interface IFunction : IExpression
{
    FunctionKind Kind { get; }
    IExpression Argument { get; }
}

public enum FunctionKind { Sin, Cos, Max }
public enum OperatorSign { Plus, Minus, Multiply, Divide }

public record Constant(int Value) : IConstantExpression;

public record Variable(string Name) : IVariableExpression;

public record BinaryOp(
    IExpression Left,
    IExpression Right,
    OperatorSign Sign
) : IBinaryExpression;

public record Function(
    FunctionKind Kind,
    IExpression Argument
) : IFunction;
