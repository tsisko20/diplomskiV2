using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TerrainUtils;

public class MinimapClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Camera minimapCamera;   // kamera koja rendera minimapu
    public Camera mainCamera;      // glavna kamera
    public Transform camParent;

    public Vector3 cameraOffset = new Vector3(0, 0, -20);
    // offset kamere u odnosu na target (možeš prilagoditi: visina + udaljenost iza)

    public float moveSpeed = 5f;   // brzina pomaka

    private Vector3 targetLookAt;
    private Vector3 targetPosition;

    private int terrainMask;
    bool hasTarget = false;
    bool wasCameraControllOn = false;

    private void Start()
    {
        camParent = mainCamera.transform.parent;
        terrainMask = LayerMask.GetMask("Terrain");
    }

    void Update()
    {
        if (hasTarget)
        {
            // Smooth pozicija
            if (CameraControl.IsCameraControllOn())
            {
                wasCameraControllOn = true;
                CameraControl.ToogleCameraControl();
            }
            camParent.position = Vector3.Lerp(
                camParent.position,
                targetPosition,
                Time.deltaTime * moveSpeed
            );
        }
        if (Vector3.Distance(camParent.position, targetPosition) < 0.1f)
        {
            hasTarget = false;
            camParent.position = targetPosition;
            //camParent.LookAt(targetLookAt);
            if (wasCameraControllOn)
            {
                CameraControl.ToogleCameraControl();
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransform rect = GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Vector2 normalized = Rect.PointToNormalized(rect.rect, localPoint);

            Vector3 minimapScreenPoint = new Vector3(
                normalized.x * minimapCamera.pixelWidth,
                normalized.y * minimapCamera.pixelHeight,
                0f
            );

            Ray ray = minimapCamera.ScreenPointToRay(minimapScreenPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainMask))
            {
                // target = mjesto na koje si kliknuo
                Vector3 target = hit.point;

                // snimimo target poziciju i lookAt
                targetPosition = target + cameraOffset;
                targetLookAt = target;

                // ---- CLAMP da ne izade izvan granica CameraControla ----
                Vector3 lower = CameraControl.GetLowerLimits();
                Vector3 upper = CameraControl.GetUpperLimits();

                targetPosition.x = Mathf.Clamp(targetPosition.x, lower.x, upper.x);
                targetPosition.y = mainCamera.transform.position.y;
                targetPosition.z = Mathf.Clamp(targetPosition.z, lower.z, upper.z);
                hasTarget = true;
            }
        }
    }
}