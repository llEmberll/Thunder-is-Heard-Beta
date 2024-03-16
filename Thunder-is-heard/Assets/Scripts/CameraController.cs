using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isMovable = true;

    public float cameraHeight = 9f;
    public float focusOffset;
    
    public float acceleration = 0.3f;
    public float movementSpeed = 15f;
    public float maxSpeed = 0.5f;
    public float zoomSpeed = 1f;
    private Camera mainCamera;
    private Vector3 targetPosition;
    public float currentSpeed = 0f;

	public Vector2 sizeLimit = new Vector2(2.5f, 5f);

	public float screenWidth;
    public float screenHeight;

    public float minX, maxX, minZ, maxZ;

    public Map map;

    private void Start()
    {
        focusOffset = cameraHeight * 0.7f;
        
        mainCamera = Camera.main;
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        FocusOnPoint(map.centralCell.position);
        FindMovementThreshold();


        targetPosition = transform.position;
        

		screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void FocusOnPoint(Vector2Int point)
    {
        transform.position = new Vector3(point.x - focusOffset , cameraHeight, point.y - focusOffset);
    }

    public void FindMovementThreshold()
    {
        maxX = ((float)-2.5) / 5 * map.size.x;
        minX = (float)-6 / 5 * map.size.x;
        minZ = (float)-6 / 5 * map.size.y;
        maxZ = (float)-2.5 / 5 * map.size.y;
        //TODO рассчитать min и max координаты камеры в зависимости от размера игрового поля
    }

    private void Update()
    {

        // Move camera based on cursor position
        Vector3 cursorPosition = Input.mousePosition;
        Vector3 movement = Vector3.zero;

        bool needMove = false;
        
        if (cursorPosition.x < 0.1f * screenWidth)
        {
            movement += (Vector3.left/2 + Vector3.forward/2);
            needMove = true;
        }
        else if (cursorPosition.x > 0.9f * screenWidth)
        {
            movement += (Vector3.right/2 + Vector3.back/2);
            needMove = true;
        }
        if (cursorPosition.y < 0.1f * screenHeight)
        {
            movement += (Vector3.back/2 + Vector3.left/2);
            needMove = true;
        }
        else if (cursorPosition.y > 0.9f * screenHeight)
        {
            movement += (Vector3.forward/2 + Vector3.right/2);
            needMove = true;
        }

        if (needMove)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= (acceleration * Time.deltaTime) * 2;
        }

            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        
        movement.Normalize();
        targetPosition += movement * movementSpeed * currentSpeed * mainCamera.orthographicSize * Time.deltaTime;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
        // Zoom camera with mouse wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, sizeLimit.x, sizeLimit.y);
        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.030f);
    }
}
