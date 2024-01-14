using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Mission : Item
{
    public override void Awake()
    {
        objectType = "Mission";

        TmpText = transform.Find("Text").GetComponent<TMP_Text>();

        TmpText.text = objectName;
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
