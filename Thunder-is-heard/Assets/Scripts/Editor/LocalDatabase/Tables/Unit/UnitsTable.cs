using UnityEngine;



[CreateAssetMenu(menuName = "unit table", fileName = "units")]
[System.Serializable]
public class UnitsTable : Table<UnitData>
{
    public override string name
    {
        get
        {
            return "Unit";
        }
    }
}