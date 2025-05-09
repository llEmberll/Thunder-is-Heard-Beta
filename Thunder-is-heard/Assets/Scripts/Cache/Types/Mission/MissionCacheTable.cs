


[System.Serializable]
public class MissionCacheTable : CacheTable
{
    public string name = "Mission";

    public override string Name { get { return name; } }

    public MissionCacheItem FindMissionByName(string name)
    {
        foreach (var keyValuePair in this.Items)
        {
            MissionCacheItem currentItem = new MissionCacheItem(keyValuePair.Value.Fields);
            if (currentItem.GetName() == name)
            {
                return currentItem;
            }
        }

        return null;
    }
}
