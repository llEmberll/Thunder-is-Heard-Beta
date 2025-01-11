using UnityEngine.UI;

public class MainBasePanel : Panel
{
    public Image toBuildModeOption;

    public void Start()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ToggledOffBuildMode += Show;
    }

    public override void Show()
    {
        base.Show();
        toBuildModeOption.gameObject.SetActive(true);
    }
}
