using System.Collections.Generic;

public abstract class SomeTableItem : ITableItem
{
    public abstract Dictionary<string, object> GetFields();

    public abstract ITableItem Clone();
}
