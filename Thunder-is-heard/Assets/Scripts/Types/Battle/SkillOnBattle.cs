

[System.Serializable]
public class SkillOnBattle
{
    public string coreId;
    public int cooldown;
    public bool isActive;

    public SkillOnBattle(string coreSkillId, int skillCooldown, bool isSkillActive)
    {
        coreId = coreSkillId;
        cooldown = skillCooldown;
        isActive = isSkillActive;
    }

}
