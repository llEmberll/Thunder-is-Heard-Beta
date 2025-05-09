using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesPanel : Panel
{
    public TMP_Text baseName;

    public TMP_Text rubText;
    public TMP_Text framesText;

    public Slider expSlider;
    public TMP_Text expText;
    public Image rankIcon;

    public Slider oilSlider;
    public TMP_Text oilText;

    public Slider steelSlider;
    public TMP_Text steelText;

    public Slider staffSlider;
    public TMP_Text staffText;

    public void Start()
    {
        EventMaster.current.ResourcesChanged += UpdateAll;
        EventMaster.current.ChangedBaseName += UpdateBaseName;
    }

    public void UpdateAll(ResourcesData readings)
    {
        rubText.text = readings.rub.ToString();
        framesText.text = readings.frames.ToString();


        Vector2Int expRange = FindRangeExpByExp(readings.exp);
        readings.maxExp = expRange.y;
        expSlider.minValue = expRange.x;
        expSlider.maxValue = readings.maxExp;
        expSlider.value = readings.exp;
        expText.text = readings.exp.ToString() + "/" + readings.maxExp.ToString();

        Sprite[] sprites = Resources.LoadAll<Sprite>(Config.resources["ranks"]);
        int rank = Config.ranksByExp[readings.maxExp];
        Sprite rankSprite = sprites[rank];
        rankIcon.sprite = rankSprite;


        oilSlider.maxValue = readings.maxOil;
        oilSlider.value = readings.oil;
        oilText.text = readings.oil.ToString() + "/" + readings.maxOil.ToString();

        steelSlider.maxValue = readings.maxSteel;
        steelSlider.value = readings.steel;
        steelText.text = readings.steel.ToString() + "/" + readings.maxSteel.ToString();

        staffSlider.maxValue = readings.maxStaff;
        staffSlider.value = readings.staff;
        staffText.text = readings.staff.ToString() + "/" + readings.maxStaff.ToString();
    }

    public void UpdateBaseName(string value)
    {
        baseName.text = value;
    }

    public Vector2Int FindRangeExpByExp(int exp)
    {
        int prevMax = 0;
        int max = 0;

        foreach (var keyValuePair in Config.ranksByExp) 
        {
            prevMax = max;
            max = keyValuePair.Key;
            if (exp < keyValuePair.Key)
            {
                break;
            }
        }

        return new Vector2Int(prevMax, max);
    }
}
