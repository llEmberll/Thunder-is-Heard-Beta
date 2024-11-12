

[System.Serializable]
public class SkillOnBattle
{
    public string coreId;
    public int cooldown;
    public bool isActive;

    public SkillOnBattle() { }

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
}
