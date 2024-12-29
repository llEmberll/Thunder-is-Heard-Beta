using UnityEngine;


public class CameraController : MonoBehaviour
{
    public bool _isMovable = true;

    public Vector2 focus;
    public bool haveFocus = false;

    public float cameraHeight = 9f;
    public float focusOffset;
    
    public float acceleration = 0.3f;
    public float movementSpeed = 15f;
    public float maxSpeed = 0.5f;
    public float zoomSpeed = 1f;
    private Camera mainCamera;
    private Vector3 cameraPosition;
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
        map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        MoveOnPoint(map.centralCell.position);
        FindMovementThreshold();


        cameraPosition = transform.position;
        

		screenWidth = Screen.width;
        screenHeight = Screen.height;

        EnableListeners();
    }

    public void EnableListeners()
    {
        EventMaster.current.UIPanelToggled += SetIsMovable;
        EventMaster.current.CameraNeedFocusOnPosition += SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled += CancelFocus;
    }

    public void DisableListeners()
    {
        EventMaster.current.UIPanelToggled -= SetIsMovable;
        EventMaster.current.CameraNeedFocusOnPosition -= SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled -= CancelFocus;
    }

    public void SetIsMovable(bool isMovementForbidden)
    {
        _isMovable = !isMovementForbidden;
    }

    public void MoveOnPoint(Vector2Int point)
    {
        transform.position = new Vector3(point.x - focusOffset , cameraHeight, point.y - focusOffset);
    }

    public void SetSoftFocusOnPoint(Vector2Int point)
    {
        Debug.Log("Set focus on " + point);
        focus = point;
        haveFocus = true;
    }

    public void CancelFocus()
    {
        Debug.Log("focus canceled");
        haveFocus = false;
    }

    public void SoftMoveOnFocus()
    {
        Vector3 focusVector3 = new Vector3(focus.x -focusOffset, cameraHeight, focus.y - focusOffset);
        float speedForMoveToFocus = movementSpeed + Vector3.Distance(transform.position, focusVector3);

        Vector3 direction = (focusVector3 - transform.position).normalized;

        direction.Normalize();
        cameraPosition += direction * speedForMoveToFocus * currentSpeed * mainCamera.orthographicSize * Time.deltaTime;
        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);

        if (Vector3.Distance(transform.position, focusVector3) < 0.1f)
        {
            Debug.Log("focus достигнут!");
            Debug.Log("Позиция камеры - " + transform.position);

            transform.position = focusVector3;
            haveFocus = false;
        }
    }

    public void FindMovementThreshold()
    {
        maxX = transform.position.x + ((map.size.y + map.size.x) / 4);
        minX = transform.position.x - ((map.size.y + map.size.x) / 4);
        minZ = transform.position.z - ((map.size.y + map.size.x) / 4);
        maxZ = transform.position.z + ((map.size.y + map.size.x) / 4);
        //TODO рассчитать min и max координаты камеры в зависимости от размера игрового поля
    }

    private void Update()
    {
        if (haveFocus)
        {
            SoftMoveOnFocus();
            return;
        }

        if (!_isMovable) return;

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
        cameraPosition += movement * movementSpeed * currentSpeed * mainCamera.orthographicSize * Time.deltaTime;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, minZ, maxZ);
        // Zoom camera with mouse wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, sizeLimit.x, sizeLimit.y);
        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);
    }
}
