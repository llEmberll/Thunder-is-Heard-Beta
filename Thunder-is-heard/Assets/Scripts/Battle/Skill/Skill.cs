

public abstract class Skill : ISkill
{
    public string _name;
    public string Name { get { return _name; } }


    public bool _isActive;
    public bool IsActive { get { return _isActive; } } // Активный ли сейчас скил


    public string _targetType; 
    public string TargetType { get { return _targetType; } } // Unit | Build

    public string _targetUnitType;
    public string TargetUnitType { get { return _targetUnitType; } } // infantry | vehicle

    public string _targetUnitDoctrine;
    public string TargetUnitDoctrine { get { return _targetUnitDoctrine; } } // land | naval | air


    public int _cooldown;
    public int Cooldown { get { return _cooldown; } }


    public int _currentCooldown;
    public int CurrentCooldown { get { return _currentCooldown; } set { _currentCooldown = value; } }


    public Effect _effect;
    public Effect Effect { get { return _effect; } }


    public Skill(string name, bool isActive, string targetType, string targetUnitType, string targetUnitDoctrine, int cooldown, int currentCooldown, Effect effect)
    {
        _name = name;
        _isActive = isActive;
        _targetType = targetType;
        _targetUnitType = targetUnitType;
        _targetUnitDoctrine = targetUnitDoctrine;
        _cooldown = cooldown;
        _currentCooldown = currentCooldown;
        _effect = effect;
    }


    public abstract bool CanUse();
    public abstract void Use();

    public virtual bool IsTargetCompy(Entity target)
    {
        if (target.Type != _targetType) return false;
        if ((_targetUnitType != null || _targetUnitDoctrine != null) && target.Type != "Unit") return false;
        
        Unit unit = (Unit)target;
        if (_targetUnitType != null && unit._unitType != _targetUnitType) return false;
        if (_targetUnitDoctrine != null && unit._doctrine != _targetUnitDoctrine) return false;

        return true;
    }
}
