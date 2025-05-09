
public class MediaEventData
{
    public string _audioEventId = null;

    public Bector2Int _point = null;

    public MediaEventData() { }

    public MediaEventData(string audioEventId, Bector2Int point)
    {
        this._audioEventId = audioEventId;
        this._point = point;
    }
}
