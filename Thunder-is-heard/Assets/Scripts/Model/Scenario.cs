using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public List<Stage> stages;
    public int currentStage = 0;

    public void Next()
    {
        currentStage++;
    }
}
