using System.Collections.Generic;
using UnityEngine;

public class BuildsOnBase : ItemList, IObjectsOnScene
{
    public Map map;

    public override void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<Map>();
        base.Start();
    }

    public override void FillContent()
    {
        base.FillContent();

        PlayerBuildsTable playerBuildsTable = (PlayerBuildsTable)LocalDatabase.GetTableByName("PlayerBuild");
        foreach (var item in playerBuildsTable.Items)
        {
            MappingBuild(item.GetFields());
        }
    }

    private void MappingBuild(Dictionary<string, object> data)
    {
        string name = (string)data["name"];
        string modelPath = (string)data["modelPath"];
        Vector2Int[] position = (Vector2Int[])data["position"];
        int rotation = (int)data["rotation"];
        int sizeX = (int)data["sizeByX"];
        int sizeY = (int)data["sizeByY"];

        GameObject buildObj = CreateBuildObject(position, name, this.transform);
        GameObject buildModel = CreateBuildModel(modelPath, rotation, buildObj.transform);

        OccypyCells(position);
        SetModelOffsetByRotation(buildModel.transform, sizeX, sizeY, rotation);
    }

    private GameObject CreateBuildModel(string modelPath, int rotation, Transform parent)
    {
        GameObject buildModelPrefab = Resources.Load<GameObject>(modelPath);
        return Instantiate(
            buildModelPrefab, buildModelPrefab.transform.position + parent.transform.position,
            Quaternion.Euler(new Vector3(0, rotation, 0)),
            parent
            );
    }

    private GameObject CreateBuildObject(Vector2Int[] position, string name, Transform parent)
    {
        var buildPrefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        GameObject obj = Instantiate(
            buildPrefab,
            new Vector3(position[0].x, 0, position[0].y),
            Quaternion.identity,
            parent
            );
        obj.name = name;
        return obj;
    }

    private void OccypyCells(Vector2Int[] position)
    {
        foreach (Vector2Int pos in position)
        {
            var cell = map.cells[pos];
            cell.Occupy();
        }
    }

    public void SetModelOffsetByRotation(Transform model, int sizeX, int sizeY, int rotation)
    {
        if (rotation == 0 || rotation == 360 || rotation == 180) return;

        Vector2Int size = GetSwappedSize(sizeX, sizeY);
        sizeX = size.x; sizeY = size.y;

        float sizeDiff = ((float)sizeX - (float)sizeY) / 2;

        Vector3 offset = new Vector3(sizeDiff, 0, -1 * sizeDiff);

        model.position += offset;
    }

    public Vector2Int GetSwappedSize(int x, int y)
    {
        x = y + x;
        y = x - y;
        x -= y;

        return new Vector2Int(x, y);
    }
}
