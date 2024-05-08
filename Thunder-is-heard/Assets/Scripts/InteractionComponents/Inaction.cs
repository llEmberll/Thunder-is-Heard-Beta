using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inaction : InteractionComponent
{
    public override void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem)
    {
        throw new System.NotImplementedException();
    }

    public override void Idle()
    {
        Debug.Log("Interact with Inaction while idle");
    }

    public override void Working()
    {
        throw new System.NotImplementedException();
    }

    public override void Finished()
    {
        throw new System.NotImplementedException();
    }
}
