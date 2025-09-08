using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance { get; private set; }
    [SerializeField] private Vector3 lowerLimits;
    [SerializeField] private Vector3 upperLimits;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float movementSpeed = 200f;
    [SerializeField] private float panBorderThickness = 10f;
    private bool shouldMoveCamera = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        HandleToggleInput();
        if (shouldMoveCamera)
        {
            HandleCameraMovement();
        }
    }

    private void HandleToggleInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            shouldMoveCamera = !shouldMoveCamera;
        }
    }
    private void HandleCameraMovement()
    {
        Vector3 newPosition = transform.position;
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            newPosition.z += movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            newPosition.z -= movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            newPosition.x += movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            newPosition.x -= movementSpeed * Time.deltaTime;
        }
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        newPosition.y -= scrollAmount * scrollSpeed * 100f * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, lowerLimits.y, upperLimits.y);
        newPosition.x = Mathf.Clamp(newPosition.x, lowerLimits.x, upperLimits.x);
        newPosition.z = Mathf.Clamp(newPosition.z, lowerLimits.z, upperLimits.z);
        transform.position = newPosition;
    }
    public static void ToogleCameraControl()
    {
        instance.shouldMoveCamera = !instance.shouldMoveCamera;
    }
    public static bool IsCameraControllOn() => instance.shouldMoveCamera;
    public static Vector3 GetLowerLimits() => instance.lowerLimits;
    public static Vector3 GetUpperLimits() => instance.upperLimits;
}
