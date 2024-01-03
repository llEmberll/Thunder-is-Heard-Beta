using System.Collections.Generic;

public interface ITableItem
{
    public abstract Dictionary<string, object> GetFields();
}
