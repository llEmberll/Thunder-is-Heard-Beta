using System.Collections.Generic;

public interface ITable
{
    public abstract string Name { get; set; }

    public abstract List<SomeTableItem> Items { get; set; }

    public void AddElement();
    public void RemoveElement();

    public void GetNext();

    public void GetPrev();

    public void ClearAll();

    public SomeTableItem this[int index] { get; set; }

    public void DefaultItemList();

    public SomeTableItem DefaultItem();
}
