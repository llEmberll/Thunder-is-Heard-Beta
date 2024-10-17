

public class EvaluateCellData : CellData
{
    // ��������� ���������� ��� ��������� A*
    public int gScore; // ��������� �������� � ���� ������
    public int hScore; // ������ ��������� �������� �� ���� ������ � �������
    public int fScore; // ����� ������
    public EvaluateCellData parent; // ������������ ������

    public EvaluateCellData(string cellType, Bector2Int cellPosition, bool isOccypy = false) : base(cellType, cellPosition, isOccypy)
    {
    }

    public EvaluateCellData(CellData cell) : base(cell._type, cell._position, cell._isOccypy)
    {
    }
}
