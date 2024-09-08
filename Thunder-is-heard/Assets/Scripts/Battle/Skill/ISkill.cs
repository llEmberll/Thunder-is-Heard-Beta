

public interface ISkill
{
    public abstract string Name { get; }
    public abstract bool IsActive { get; }

    public abstract string TargetType { get; }

    public abstract int Cooldown { get; }
    public abstract int CurrentCooldown { get; set; }
    public abstract Effect Effect { get; }

    public abstract bool CanUse();
    public abstract void Use();
}
