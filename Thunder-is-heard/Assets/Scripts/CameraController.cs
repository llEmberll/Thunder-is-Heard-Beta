using UnityEngine;


public class CameraController : MonoBehaviour
{
    public bool isUIPanel = false;

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
        EventMaster.current.CameraMovePermitToggled += OnCameraMovePermitToggle;
        EventMaster.current.UIPanelToggled += OnUIPanelToggle;
        EventMaster.current.CameraNeedFocusOnPosition += SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled += CancelFocus;
    }

    public void DisableListeners()
    {
        EventMaster.current.CameraMovePermitToggled -= OnCameraMovePermitToggle;
        EventMaster.current.UIPanelToggled -= OnUIPanelToggle;
        EventMaster.current.CameraNeedFocusOnPosition -= SetSoftFocusOnPoint;
        EventMaster.current.CameraFocusCanceled -= CancelFocus;
    }

    public void SetIsMovable(bool isMovementForbidden)
    {
        _isMovable = !isMovementForbidden;

    }

    public void OnUIPanelToggle(bool isOpen)
    {
        if (isOpen)
        {
            isUIPanel = true;
            SetIsMovable(true);
        }
        else
        {
            isUIPanel = false;
            SetIsMovable(false);
        }
    }

    public void OnCameraMovePermitToggle(bool isMovementForbidden)
    {
        if (isUIPanel) return;
        SetIsMovable(isMovementForbidden);
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
        haveFocus = false;
        if (isUIPanel) return;

        Debug.Log("focus canceled");

        SetIsMovable(false);
        
    }

    public void SoftMoveOnFocus()
    {
        Vector3 focusVector3 = new Vector3(focus.x -focusOffset, cameraHeight, focus.y - focusOffset);

        Vector3 direction = (focusVector3 - transform.position).normalized;

        float moveToFocusSpeed = 0.35f;

        direction.Normalize();
        cameraPosition += direction * Vector3.Distance(transform.position, focusVector3) * moveToFocusSpeed * mainCamera.orthographicSize * Time.deltaTime;

        Debug.Log("Move calculated: " + cameraPosition);

        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);

        Debug.Log("Camera moved");

        // Отправляем событие о движении камеры при фокусировке
        EventMaster.current.OnCameraMoved();

        if (Vector3.Distance(transform.position, focusVector3) < 0.1f)
        {
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
        //TODO ���������� min � max ���������� ������ � ����������� �� ������� �������� ����
    }

    public void UpdateLastMousePosition()
    {
        _lastMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void UpdateIsDraggingStatus()
    {
        // ��������� ������� ���
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            UpdateLastMousePosition();
        }

        // ��������� ���������� ���
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
        bool cameraMoved = false;
        
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
                cameraMoved = true;
            }

            if (delta.y != 0)
            {
                cameraPosition += new Vector3(-delta.y * totalMovementMultiplier, 0, -delta.y * totalMovementMultiplier);
                cameraMoved = true;
            }

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, minZ, maxZ);
            UpdateLastMousePosition();
        }

        // Zoom camera with mouse wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (zoomAmount != 0)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, sizeLimit.x, sizeLimit.y);
        }
        
        // Move camera to target position
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 0.030f);
        
        // Отправляем событие только если камера действительно двигалась
        if (cameraMoved)
        {
            EventMaster.current.OnCameraMoved();
        }
    }
}
