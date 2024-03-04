

using System.Collections.Generic;

public interface ICacheItem
{
    public Dictionary<string, object> Fields { get; }
    public CacheItem Clone();
    public object GetField(string fieldName);
    public void SetField(string fieldName, object fieldValue);
}
