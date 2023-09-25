using UnityEngine;



[CreateAssetMenu(menuName = "player unit table", fileName = "player units")]
[System.Serializable]
public class PlayerUnitsTable : Table<UnitData>
{
    public override string name
    {
        get
        {
            return "PlayerUnit";
        }
    }
}