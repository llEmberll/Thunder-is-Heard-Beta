using UnityEngine;


public class CameraController : MonoBehaviour
{
    public bool _isMovable = true;

    public bool _isDragging = false;
    public Vector2 _lastMousePosition;

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
    public float _aspectRatio;

    public float minX, maxX, minZ, maxZ;

    public Map map;

    public void Awake()
    {
        EnableListeners();
    }

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
        SetAspectRatio();
    }

    public void EnableListeners()
    {
        EventMaster.current.CameraMovePermitToggled += SetIsMovable;
        EventMaster.current.UIPanelToggled += SetIsMovable;
        EventMaster.current.CameraNeedFocusOnPosition += SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled += CancelFocus;
    }

    public void DisableListeners()
    {
        EventMaster.current.CameraMovePermitToggled -= SetIsMovable;
        EventMaster.current.UIPanelToggled -= SetIsMovable;
        EventMaster.current.CameraNeedFocusOnPosition -= SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled -= CancelFocus;
    }

    public void SetIsMovable(bool isMovementForbidden)
    {
        Debug.Log("set is movable camera. isMovementForbidden = "+ isMovementForbidden);

        _isMovable = !isMovementForbidden;

        Debug.Log("Can move = " +  _isMovable);
    }

    public void SetAspectRatio()
    {
        _aspectRatio = screenWidth / screenHeight;
    }

    public void MoveOnPoint(Vector2Int point)
    {
        transform.position = new Vector3(point.x - focusOffset , cameraHeight, point.y - focusOffset);
    }

    public void SetSoftFocusOnPoint(Vector2Int point, bool lockCamera)
    {
        Debug.Log("Set focus on " + point);
        focus = point;
        haveFocus = true;
        _isMovable = !lockCamera;
    }

    public void CancelFocus()
    {
        Debug.Log("focus canceled");
        haveFocus = false;
        _isMovable = true;
    }

    public void SoftMoveOnFocus()
    {
        Debug.Log("In SoftMove");

        Vector3 focusVector3 = new Vector3(focus.x -focusOffset, cameraHeight, focus.y - focusOffset);

        Vector3 direction = (focusVector3 - transform.position).normalized;

        float moveToFocusSpeed = 0.35f;

        direction.Normalize();
        cameraPosition += direction * Vector3.Distance(transform.position, focusVector3) * moveToFocusSpeed * mainCamera.orthographicSize * Time.deltaTime;

        Debug.Log("Move calculated: " + cameraPosition);

        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);

        Debug.Log("Camera moved");

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

    public void UpdateLastMousePosition()
    {
        _lastMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void UpdateIsDraggingStatus()
    {
        // Проверяем нажатие ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            UpdateLastMousePosition();
        }

        // Проверяем отпускание ЛКМ
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void Update()
    {
        if (haveFocus)
        {
            Debug.Log("Update: focus true");
            SoftMoveOnFocus();
            return;
        }

        if (!_isMovable) return;

        UpdateIsDraggingStatus();
        if (_isDragging)
        {
            Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _lastMousePosition;
            delta.y *= _aspectRatio;
            delta.Normalize();

            float speedForCamera = movementSpeed / 5;
            float totalMovementMultiplier = speedForCamera * mainCamera.orthographicSize * Time.deltaTime;

            if (delta.x != 0)
            {
                cameraPosition += new Vector3(-delta.x * totalMovementMultiplier, 0, delta.x * totalMovementMultiplier);
            }

            if (delta.y != 0)
            {
                cameraPosition += new Vector3(-delta.y * totalMovementMultiplier, 0, -delta.y * totalMovementMultiplier);
            }

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, minZ, maxZ);
            UpdateLastMousePosition();
        }

        // Zoom camera with mouse wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, sizeLimit.x, sizeLimit.y);
        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);
    }
}
