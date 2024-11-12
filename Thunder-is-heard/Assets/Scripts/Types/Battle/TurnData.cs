using System.Collections.Generic;


public class TurnData
{
    public string _activeUnitIdOnBattle = null;
    public string _targetIdOnBattle = null;
    public List<Bector2Int> _route = null;

    public TurnData() { }

    public TurnData(string activeUnitIdOnBattle, List<Bector2Int> route, string targetIdOnBattle = null)
    {
        _activeUnitIdOnBattle = activeUnitIdOnBattle;
        _targetIdOnBattle = targetIdOnBattle;
        _route = route;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        TurnData other = (TurnData)obj;


        if (_route == null && other._route == null)
        {
            return _activeUnitIdOnBattle == other._activeUnitIdOnBattle && _targetIdOnBattle == other._targetIdOnBattle;
        }
        else
        {
            if (_route == null || other._route == null) return false;
        }

        if (_route.Count != other._route.Count) return false;

        for (int i = 0; i < _route.Count; i++)
        {
            if (!_route[i].Equals(other._route[i]))
                return false;
        }
        return _activeUnitIdOnBattle == other._activeUnitIdOnBattle && _targetIdOnBattle == other._targetIdOnBattle;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            int activeUnitHash = _activeUnitIdOnBattle != null ? _activeUnitIdOnBattle.GetHashCode() : 0;
            int targetHash = _targetIdOnBattle != null ? _targetIdOnBattle.GetHashCode() : 0;
            hash = hash * 23 +  activeUnitHash;
            hash = hash * 23 + targetHash;
            if (_route == null) return hash;

            foreach (var cell in _route)
            {
                hash = hash * 31 + (cell != null ? cell.GetHashCode() : 0);
            }
            return hash;
        }
    }
}
