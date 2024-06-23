using Org.BouncyCastle.Asn1.Mozilla;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class CellData
{
    public string type;
    public Bector2Int position;

    public CellData(string cellType, Bector2Int cellPosition)
    {
        type = cellType;
        position = cellPosition;
    }

}
