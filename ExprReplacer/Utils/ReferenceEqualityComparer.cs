// Simple reference equality comparer (works for any reference type)
// (Reference equality != structural equality)
// ex: new Variable("x") !== new Variable("x")
// but new Variable("x") == new Variable("x")
sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    public bool Equals(T? x, T? y) => ReferenceEquals(x, y);
    public int GetHashCode(T obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
}
