using System.Collections.Generic;

public class SomeTableItem : ITableItem
{
    public virtual Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>();
    }
}
