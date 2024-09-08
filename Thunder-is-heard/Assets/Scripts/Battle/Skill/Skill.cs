

public abstract class Skill : ISkill
{
    public string _name;
    public string Name { get { return _name; } }


    public bool _isActive;
    public bool IsActive { get { return _isActive; } }


    public string _targetType;
    public string TargetType { get { return _targetType; } }


    public int _cooldown;
    public int Cooldown { get { return _cooldown; } }


    public int _currentCooldown;
    public int CurrentCooldown { get { return _currentCooldown; } set { _currentCooldown = value; } }


    public Effect _effect;
    public Effect Effect { get { return _effect; } }


    public Skill(string name, bool isActive, string targetType, int cooldown, int currentCooldown, Effect effect)
    {
        _name = name;
        _isActive = isActive;
        _targetType = targetType;
        _cooldown = cooldown;
        _currentCooldown = currentCooldown;
        _effect = effect;
    }


    public abstract bool CanUse();
    public abstract void Use();
}
