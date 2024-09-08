

public interface IEffect
{
    public abstract string Name { get; }
    public abstract string Type { get; }

    public abstract int Duration { get; set; }

    public abstract bool IsEnd();
    public abstract bool IsMustTic();
    public abstract void Tic();
}
