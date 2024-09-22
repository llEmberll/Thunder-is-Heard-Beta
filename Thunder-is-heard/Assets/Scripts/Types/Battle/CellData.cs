

[System.Serializable]
public class CellData
{
    public string _type;
    public Bector2Int _position;
    public bool _isOccypy;

    public CellData(string cellType, Bector2Int cellPosition, bool isOccypy = false)
    {
        _type = cellType;
        _position = cellPosition;
        this._isOccypy = isOccypy;
    }

    public CellData Clone()
    {
        return new CellData(_type, _position, _isOccypy);
    }
}
