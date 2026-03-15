namespace Cstewart.Portfolio.DataModel;

public abstract class Pair<T>
{
    public T Offensive { get; set; } = default!;
    public T Defensive { get; set; } = default!;
}

public class IntPair : Pair<int> {}

public class StringPair : Pair<string> {}