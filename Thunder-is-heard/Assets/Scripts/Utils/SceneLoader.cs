using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadFight(FightSceneParameters parameters)
    {
        FightSceneLoader.parameters = parameters;

        SceneManager.LoadScene(Scenes.fight, LoadSceneMode.Single);
    }

    public static void LoadHome()
    {
        SceneManager.LoadScene(Scenes.home, LoadSceneMode.Single);
    }
}

public static class FightSceneLoader
{
    public static FightSceneParameters parameters;
}

public class FightSceneParameters
{
    public string _battleId;
    public bool _autoStartFight;

    public FightSceneParameters(string battleId, bool autoStartFight = false)
    {
        _battleId = battleId;
        _autoStartFight = autoStartFight;
    }
} 
