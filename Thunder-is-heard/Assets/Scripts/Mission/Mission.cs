using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Mission : Item
{
    public string type = "Mission";

    public override string Type { get { return type;  } }

    public override void Awake()
    {
        type = "Mission";

        TmpName = transform.Find("Text").GetComponent<TMP_Text>();

        TmpName.text = objName;
    }

    public override void Interact()
    {
        Load();
    }

    public void Load()
    {
        var mission = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
        
        DontDestroyOnLoad(mission.gameObject);
        
        SceneManager.LoadScene("Fight");

    }
}
