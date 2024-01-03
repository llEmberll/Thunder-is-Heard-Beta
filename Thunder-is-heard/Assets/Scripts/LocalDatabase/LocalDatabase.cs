using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;



[CreateAssetMenu(menuName = "LocalDatabase", fileName = "database")]
public class LocalDatabase : ScriptableObject
{
    public static List<Object> GetTables() {
        Object[] tables = Resources.LoadAll<Object>(Config.localDataBase["tablesPath"]);
        return (tables).ToList();
    }

    public static ITable GetTableByName(string name)
    {
         foreach (ITable table in GetTables()) 
        { 
            if (table.Name == name) return table;
        }

        return null;
    }

    public static Dictionary<string, object> GetFieldsByTableAndTableItemIndex(ITable table, int tableItemIndex)
    {
        try
        {
            return table.Name switch
            {
                "Build" => ((BuildsTable)table).Items[tableItemIndex].GetFields(),
                "PlayerBuild" => ((BuildsTable)table).Items[tableItemIndex].GetFields(),
                "Unit" => ((UnitsTable)table).Items[tableItemIndex].GetFields(),
                "PlayerUnit" => ((UnitsTable)table).Items[tableItemIndex].GetFields(),
                "PlayerCell" => ((BaseCellsTable)table).Items[tableItemIndex].GetFields(),
                "PlayerResource" => ((PlayerResourcesTable)table).Items[tableItemIndex].GetFields()
            };
        }
        catch 
        { 
            Debug.Log("Can't find fields. Table " + table.Name + ", item index " +  tableItemIndex);
            return null;
        }
    }
}