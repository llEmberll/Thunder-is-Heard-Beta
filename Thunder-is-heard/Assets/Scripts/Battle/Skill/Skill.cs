using UnityEngine;


public abstract class Skill : ISkill
{
    public string _coreId;
    public string CoreId { get { return _coreId; } }


    public string _childId;
    public string ChildId { get { return _childId; } }


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


    public int _currentCooldown = 0;
    public int CurrentCooldown { get { return _currentCooldown; } set { _currentCooldown = value; } }


    public UnitsOnFight unitsOnFight;
    public UnitsOnFight UnitsManager { get { return unitsOnFight; } }

    public BuildsOnFight buildsOnFight;
    public BuildsOnFight BuildsManager { get { return buildsOnFight; } }


    public Skill(SkillOnBattle skillOnBattleData)
    {
        Configure(skillOnBattleData);
        Init();
    }

    public Skill(SkillCacheItem skillData)
    {
        Configure(skillData);
        Init();
    }


    public Skill() 
    {
        Init();
    }

    public Skill(string coreId, string childId, string name, bool isActive, string targetType, string targetUnitType, string targetUnitDoctrine, int cooldown, int currentCooldown)
    {
        _coreId = coreId;
        _childId = childId;
        _name = name;
        _isActive = isActive;
        _targetType = targetType;
        _targetUnitType = targetUnitType;
        _targetUnitDoctrine = targetUnitDoctrine;
        _cooldown = cooldown;
        _currentCooldown = currentCooldown;

        Init();
    }


    public void Configure(SkillCacheItem skillData)
    {
        _coreId = skillData.GetCoreId();
        _name = skillData.GetName();
        _targetType = skillData.GetTargetType();
        _targetUnitType = skillData.GetTargetUnitType();
        _targetUnitDoctrine = skillData.GetTargetUnitDoctrine();
        _cooldown = skillData.GetCooldown();
    }

    public void Configure(SkillOnBattle skillOnBattleData)
    {
        _coreId = skillOnBattleData.coreId;
        _childId = skillOnBattleData.childId;
        _isActive = skillOnBattleData.isActive;
        _currentCooldown = skillOnBattleData.cooldown;

        SkillCacheTable skillsTable = Cache.LoadByType<SkillCacheTable>();
        CacheItem cacheItem = skillsTable.GetById(_coreId);
        if (cacheItem == null) return;

        SkillCacheItem coreSkillData = new SkillCacheItem(cacheItem.Fields);
        _name = coreSkillData.GetName();

        _targetType = coreSkillData.GetTargetType();
        _targetUnitType = coreSkillData.GetTargetUnitType();
        _targetUnitDoctrine = coreSkillData.GetTargetUnitDoctrine();
        _cooldown = coreSkillData.GetCooldown();
    }


    public void Init()
    {
        InitManagers();
    }

    public void InitManagers()
    {
        unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
        buildsOnFight = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();

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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Skill other = (Skill)obj;
        return CoreId == other.CoreId && ChildId == other.ChildId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + CoreId.GetHashCode();
            hash = hash * 23 + ChildId.GetHashCode();
            return hash;
        }
    }
}
