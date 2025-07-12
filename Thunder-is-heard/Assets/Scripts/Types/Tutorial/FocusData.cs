using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class FocusData
{
    [JsonProperty("Type")]
    public string Type;

    [JsonProperty("Data")]
    public Dictionary<string, object> Data;

    // ������������� ���������
    /// <summary>
    /// {"Type": "...", "Data": {"lockCamera": true}
    /// </summary>

    // Example 1. ������������ ������ �� ����
    /// <summary>
    /// {"Type": "Button", "Data": {"tag": "ToShopButton", ...}}
    /// </summary>

    // Example 2. ������� ������ �� ��������� id
    /// <summary>
    /// {"Type": "Build", "Data": {"childId": "[some uuid4]", ...}}
    /// </summary>

    // Example 2. ����, ��������� ��� �����, �� �������
    /// <summary>
    /// {"Type": "Unit", "Data": {"side": "Empire", "underAttack": true}}
    /// </summary>

    // Example 2. ���� �� �������
    /// <summary>
    /// {"Type": "Unit", "Data": {"side": "Federation"}}
    /// </summary>

    // Example 2. ���� �� id
    /// <summary>
    /// {"Type": "Unit", "Data": {"childId": "[some uuid4]"}}
    /// </summary>

    // Example 3. ������ ������� ������ �� ������������� id
    /// <summary>
    /// {"Type": "Build", "Data": {"coreId": "[some uuid4]", ...}}
    /// </summary>

    // Example 4. ������� ������ �� ���� ������ � id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Shop", "itemId": "some uuid4", ...}}
    /// </summary>

    // Example 5. ������� ������ �� ���� ������ � ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Shop", "coreId": "some uuid4", ...}}
    /// </summary>

    // Example 6. ������� ������ �� ���� ������ � ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Contracts", "contractType": "Steel", ...}}
    /// </summary>

    // Example 6. ���� ��� ������� �� ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "Landing", "coreId": "some uuid4", ...}}
    /// </summary>

    // Example 7. ������� ������ �� ���� ������ � ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "itemId": "some uuid4", ...}}
    /// </summary>

    // Example 8. ������� ������ �� ���� ������ � ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "unitType": "Infantry", ...}}
    /// </summary>

    // Example 9. ������� ������ �� ���� ������ � ������������� id ��������
    /// <summary>
    /// {"Type": "UIItem", "Data": {"UIType": "UnitProductions", "unitId": "some uuid4", ...}}
    /// </summary>

    // Example 10. ������ ����������� � ����� �� ���� �����������
    /// <summary>
    /// {"Type": "ProductsNotification", "Data": {"type": "ResourceCollection", ...}}
    /// </summary>

    // Example 11. ������ ����������� � ����� �� ������������� id �������-���������
    /// <summary>
    /// {"Type": "ProductsNotification", "Data": {"sourceObjectCoreId": "8878498b-a4bc-4dc8-8f39-bc9e987a689f", ...}}
    /// </summary>

    // Example 12. ������� �� ����� � ����������
    /// <summary>
    /// {"Type": "Area", "Data": {"visible": true, "rectangle": new RectangleBector2Int...}}
    /// </summary>

    // Example 12. ������� �� ����� ��� ���������
    /// <summary>
    /// {"Type": "Area", "Data": {"visible": false, "rectangle": new RectangleBector2Int...}}
    /// </summary>

    .// ����������� ��� ����� ���������� ������� �� ����� � � �������



    public FocusData() { }

    public FocusData(string type, Dictionary<string, object> data)
    {
        Type = type;
        Data = data;
    }
}
