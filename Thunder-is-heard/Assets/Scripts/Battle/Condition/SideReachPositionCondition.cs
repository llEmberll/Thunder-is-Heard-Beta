using UnityEngine;


public class SideReachPositionCondition : BasicCondition
{
    public RectangleBector2Int _positionRectangle;
    public string _side;

    public UnitsOnFight _unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();

    public SideReachPositionCondition(RectangleBector2Int positionRectange, string side)
    {
        _positionRectangle = positionRectange;
        _side = side;
    }

    public SideReachPositionCondition(Bector2Int start, Bector2Int end, string side)
    {
        _positionRectangle = new RectangleBector2Int(start, end);
        _side = side;
    }



    public override bool IsComply()
    {
        foreach (Entity unit in _unitsOnFight.items.Values)
        {
            if (unit.side == _side && _positionRectangle.Contains(new Bector2Int(unit.center))) return true;
        }

        return false;
    }
}
