using System.Collections.Generic;
using Unity.VisualScripting;

public interface ITableItem
{
    public abstract Dictionary<string, object> GetFields();

    public abstract ITableItem Clone();
}
