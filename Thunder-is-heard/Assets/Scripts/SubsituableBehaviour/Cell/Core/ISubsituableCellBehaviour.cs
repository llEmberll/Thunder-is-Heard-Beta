

public interface ISubsituableCellBehaviour
{
    public void Init(Cell conductor);

    public void OnClick(Cell conductor);

    public void OnFocus(Cell conductor);

    public void OnDefocus(Cell conductor);
}
