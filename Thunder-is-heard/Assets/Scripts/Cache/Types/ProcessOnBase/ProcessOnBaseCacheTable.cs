


[System.Serializable]
public class ProcessOnBaseCacheTable : CacheTable
{
    public string name = "ProcessOnBase";

    public override string Name { get { return name; } }

    public ProcessOnBaseCacheItem FindBySourceObjectId(string sourceObjectId)
    {
        foreach (var keyValuePair in this.Items)
        {
            ProcessOnBaseCacheItem currentItem = new ProcessOnBaseCacheItem(keyValuePair.Value.Fields);
            if (currentItem.GetObjectOnBaseId() == sourceObjectId)
            {
                return currentItem;
            }
        }

        return null;
    }
}
