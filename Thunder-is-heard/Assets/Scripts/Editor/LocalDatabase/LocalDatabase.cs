using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[CreateAssetMenu(menuName = "local database", fileName = "database")]
public class LocalDatabase : ScriptableObject
{
    public List<ScriptableObject> GetTables() {
        return Resources.LoadAll<ScriptableObject>(Config.localDataBase["tablesPath"]).ToList();
    }
}