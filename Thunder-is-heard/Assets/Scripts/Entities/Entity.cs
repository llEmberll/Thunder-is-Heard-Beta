using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Interactable
{
    public Vector2Int size;
    public List<Vector2Int> occypiedPoses;
    public int rotation;
    public Vector2Int center;
    public abstract override string EntityType { get; }


    public void Awake()
    {
        SetRotation();
        UpdateoccypiedPoses();
    }

    public void SetRotation()
    {
        rotation = (int)transform.eulerAngles.y;
    }

    public virtual void UpdateoccypiedPoses()
    {
        Vector2Int rootPose = center = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        if (size.x < 2 && size.y < 2)
        {
            occypiedPoses = new List<Vector2Int> { center };
            return;
        }

        CorrectSizeByRotation();

        Vector2Int directionForFillByX = FindDirectionForFillOcypyByX();
        Vector2Int directionForFillByY = FindDirectionForFillOcypyByY();

        int maxX = rootPose.x + (directionForFillByX.x * size.x);
        int maxY = rootPose.y + (directionForFillByY.y * size.y);

        center = new Vector2Int(((rootPose.x + (maxX - directionForFillByX.x))) / 2, ((rootPose.y + (maxY - directionForFillByY.y))) / 2);

        FillOccypation(rootPose, directionForFillByX, directionForFillByY, new Vector2Int(maxX, maxY));
    }

    public void CorrectSizeByRotation()
    {
        if (rotation == 90 || rotation == 270)
        {
            int oldSizeX = size.x;
            size.x = size.y;
            size.y = oldSizeX;
        }
    }

    private Vector2Int FindDirectionForFillOcypyByX()
    {
        if (transform.forward.x != 0) return new Vector2Int((int)transform.forward.x, (int)transform.forward.z);
        if (transform.right.x != 0) return new Vector2Int((int)transform.right.x, (int)transform.right.z);
        return new Vector2Int(0, 0);
    }

    private Vector2Int FindDirectionForFillOcypyByY()
    {
        if (transform.right.z != 0) return new Vector2Int((int)transform.right.x, (int)transform.right.z);
        if (transform.forward.z != 0) return new Vector2Int((int)transform.forward.x, (int)transform.forward.z);
        return new Vector2Int(0, 0);
    }

    private void FillOccypation(Vector2Int rootPose, Vector2Int directionForFillByX, Vector2Int directionForFillByY, Vector2Int max)
    {
        Vector2Int currentPose = rootPose;
        occypiedPoses = new List<Vector2Int>();

        for (int x = rootPose.x; ;)
        {
            x = currentPose.x;
            if (x == max.x)
            {
                break;
            }
            for (int y = rootPose.y; ;)
            {
                y = currentPose.y;
                if (y == max.y)
                {
                    break;
                }
                occypiedPoses.Add(currentPose);
                currentPose = currentPose + directionForFillByY;
            }
            currentPose = currentPose + directionForFillByX;
        }
    }
}