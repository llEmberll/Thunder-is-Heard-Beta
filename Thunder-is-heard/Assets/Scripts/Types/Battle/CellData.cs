

[System.Serializable]
public class CellData
{
    public string _type;
    public Bector2Int _position;
    public bool _isOccypy;

    public CellData() { }

    public CellData(string cellType, Bector2Int cellPosition, bool isOccypy = false)
    {
        _type = cellType;
        _position = cellPosition;
        this._isOccypy = isOccypy;
    }

    public CellData(Cell cell)
    {
        _type = cell.Type;
        _position = new Bector2Int(cell.position);
        this._isOccypy = cell.occupied;
    }

    public CellData Clone()
    {
        return new CellData(_type, _position, _isOccypy);
    }
}
