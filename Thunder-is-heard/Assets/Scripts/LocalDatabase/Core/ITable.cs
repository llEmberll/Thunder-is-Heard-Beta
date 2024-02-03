using System.Collections.Generic;

public interface ITable
{
    public abstract string Name { get; set; }

    public abstract List<ITableItem> Items { get; set; }

    public void AddElement();
    public void RemoveElement();

    public void GetNext();

    public void GetPrev();

    public void ClearAll();

    public ITableItem this[int index] { get; set; }

    public void DefaultItemList();

    public ITableItem DefaultItem();
}
