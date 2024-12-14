

[System.Serializable]
public class SkillOnBattle
{
    public string coreId;
    public string childId;
    public int cooldown;
    public bool isActive;

    public SkillOnBattle() { }

    public SkillOnBattle(Skill skillComponent)
    {
        coreId = skillComponent.CoreId;
        cooldown = skillComponent.Cooldown;
        isActive = skillComponent.IsActive;
    }

    public SkillOnBattle(string coreSkillId, int skillCooldown, bool isSkillActive)
    {
        coreId = coreSkillId;
        cooldown = skillCooldown;
        isActive = isSkillActive;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SkillOnBattle other = (SkillOnBattle)obj;
        return coreId == other.coreId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + coreId.GetHashCode();
            return hash;
        }
    }

    public static string[] GetSkillIdsBySkillOnBattleDatas(SkillOnBattle[] datas)
    {
        if (datas == null || datas.Length < 1) return new string[] {};
        string[] ids = new string[datas.Length];

        int index = 0;
        foreach (SkillOnBattle data in datas)
        {
            ids[index] = data.coreId;
        }

        return ids;
    }
}
