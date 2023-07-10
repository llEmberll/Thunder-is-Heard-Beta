using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour
{
    public Map map;

    public Scenario scenario;
    public void Load()
    {
        var mission = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
        
        DontDestroyOnLoad(mission.gameObject);
        
        SceneManager.LoadScene("Fight");

    }
}
