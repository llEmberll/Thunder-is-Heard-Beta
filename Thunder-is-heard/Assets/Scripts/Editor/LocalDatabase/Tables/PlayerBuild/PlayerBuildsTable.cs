using UnityEngine;


[CreateAssetMenu(menuName = "player build table", fileName = "player builds")]
public class PlayerBuildsTable : Table<BuildData>
{
    public override string name
    {
        get
        {
            return "PlayerBuild";
        }
    }
}