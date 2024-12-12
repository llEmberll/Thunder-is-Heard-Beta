using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity : Interactable, IDamageable
{
    public string coreId;
    public string childId;
    public string name;

    public Vector2Int originalSize;
    public Vector2Int currentSize;
    public List<Vector2Int> occypiedPoses;
    public int rotation;
    public Vector2Int center;

    public string _doctrine;

    public abstract override string Type { get; }
    public string CoreId { get { return coreId; } }
    public string ChildId { get { return childId; } }

    public Transform model;
    public Map map;

    public StateMachine stateMachine = new StateMachine();
    public SceneState sceneState;

    public string side;

    public int maxHealth, currentHealth, damage, distance, mobility;

    public IAnimator animator;


    public virtual void Awake()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
    }

    public virtual void Start()
    {
        sceneState = GameObject.FindWithTag("State").GetComponent<SceneState>();

        stateMachine.Initialize(sceneState.GetCurrentState());

        //OnChangeStateEvent
        EventMaster.current.StateChanged += OnChangeState;

        animator = GetComponentInChildren<IAnimator>();
        
    }

    public void SetName(string value)
    {
        name = value;
    }

    public void SetAttributes(int maxHealthValue, int currentHealthValue, int damageValue, int distanceValue, int mobilityValue)
    {
        maxHealth = maxHealthValue;
        currentHealth = currentHealthValue;
        damage = damageValue;
        distance = distanceValue;
        mobility = mobilityValue;
    }

    public void SetDoctrine(string value)
    {
        _doctrine = value;
    }

    public void SetSide(string value)
    {
        side = value;
    }

    public virtual void GetDamage(int damage)
    {
        Debug.Log("Obj damaged!");

        if (damage >= currentHealth)
        {
            currentHealth = 0;
            OnDestroy();
        }
        else
        {
            currentHealth -= damage;
            EventMaster.current.OnObjectDamaged(this);
        }
    }

    public override void OnFocus()
    {
        
    }

    public override void OnDefocus()
    {

    }

    public override void OnClick()
    {

    }

    public override void OnChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void SetOriginalSize(Vector2Int newSize)
    {
        originalSize = newSize;
        currentSize = originalSize;
    }

    public void SetRotation(int newRotation)
    {
        rotation = newRotation;
        currentSize = GetCorrectSizeByRotation(originalSize, rotation);
    }

    public void SetModel(Transform newModel)
    {
        model = newModel;
        SetRotation(GetDeterminedRotationByModel(model));
    }

    public void SetOccypation(List<Vector2Int> position)
    {
        occypiedPoses = position;
        center = CalculateCenter(occypiedPoses);
    }

    public static int GetDeterminedRotationByModel(Transform modelForCheck)
    {
        return (int)modelForCheck.eulerAngles.y;
    }

    public static Vector2Int GetCorrectSizeByRotation(Vector2Int size, int currentRotation)
    {
        if (size.x == size.y)
        {
            return size;
        }
        if (currentRotation == 90 || currentRotation == 270)
        {
            size = GetSwappedSize(size);
        }

        return size;
    }

    public static Vector2Int GetSwappedSize(Vector2Int currentSize)
    {
        currentSize.x = currentSize.y + currentSize.x;
        currentSize.y = currentSize.x - currentSize.y;
        currentSize.x -= currentSize.y;

        return currentSize;
    }

    public static Vector2Int CalculateCenter(List<Vector2Int> positions)
    {
        if (positions.Count == 1) 
        { 
            return positions[0];
        }

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (Vector2Int pose in positions)
        {
            if (pose.x < minX)
                minX = pose.x;
            if (pose.y < minY)
                minY = pose.y;
            if (pose.x > maxX)
                maxX = pose.x;
            if (pose.y > maxY)
                maxY = pose.y;
        }

        float differenceX = (Math.Abs(maxX + minX)) / 2;
        int centerX = (int)Math.Floor(differenceX);

        float differenceY = (Math.Abs(maxY + minY)) / 2;
        int centerY = (int)Math.Floor(differenceY);

        return new Vector2Int(centerX, centerY);
    }

    public virtual void Die()
    {
        animator.Death();
    }

    public virtual void OnDestroy()
    {
        Debug.Log("On destroy");

        map.Free(occypiedPoses);
        EventMaster.current.OnObjectDestroy(this);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Entity other = (Entity)obj;
        return CoreId == other.CoreId && ChildId == other.ChildId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + CoreId.GetHashCode();
            hash = hash * 23 + ChildId.GetHashCode();
            return hash;
        }
    }
}