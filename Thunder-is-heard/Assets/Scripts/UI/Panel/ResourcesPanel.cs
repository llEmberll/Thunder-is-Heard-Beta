using TMPro;
using UnityEngine.UI;

public class ResourcesPanel : Panel
{
    public TMP_Text rubText;
    public TMP_Text framesText;

    public Slider expSlider;
    public TMP_Text expText;

    public Slider oilSlider;
    public TMP_Text oilText;

    public Slider steelSlider;
    public TMP_Text steelText;

    public Slider staffSlider;
    public TMP_Text staffText;

    public void UpdateAll(ResourcesData readings)
    {
        rubText.text = readings.rub.ToString();
        framesText.text = readings.frames.ToString();

        expSlider.maxValue = readings.maxExp;
        expSlider.value = readings.exp;
        expText.text = readings.exp.ToString() + "/" + readings.maxExp.ToString();

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
}
