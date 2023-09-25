using UnityEngine;


[CreateAssetMenu(menuName = "player resource table", fileName = "player resources")]
public class PlayerResourcesTable : Table<PlayerResourceData>
{
    public override string name
    {
        get
        {
            return "PlayerResource";
        }
    }
}