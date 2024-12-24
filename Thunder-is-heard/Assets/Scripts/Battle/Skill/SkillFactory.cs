using System;
using System.Collections.Generic;


public static class SkillFactory
{
    public static Dictionary<string, Type> skillsByName = new Dictionary<string, Type>()
    {
        { "Атака в движении", typeof(MoveWithAttack) },
        { "Double damage to infantry", typeof(DoubleDamageToInfantry) },
    };

    public static Skill GetSkillById(string id)
    {
        SkillCacheTable skillsTable = Cache.LoadByType<SkillCacheTable>();
        CacheItem cacheItem = skillsTable.GetById(id);
        if (cacheItem == null)
        {
            return null;
        }

        SkillCacheItem skillData = new SkillCacheItem(cacheItem.Fields);
        string skillName = skillData.GetName();

        if (skillsByName.ContainsKey(skillName))
        {
            Type type = skillsByName[skillName];
            Skill skill = (Skill)Activator.CreateInstance(type);
            skill._coreId = id;
            return skill;
        }

        return null;
    }
}
