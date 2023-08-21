using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable: MonoBehaviour
{
    public abstract string entityType { get; }

    public abstract void OnChangeState(State newState);


    protected abstract void OnMouseEnter();
    protected abstract void OnMouseExit();
    protected abstract void OnMouseDown();
}