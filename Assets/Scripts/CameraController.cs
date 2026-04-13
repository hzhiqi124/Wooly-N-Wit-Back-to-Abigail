using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform sheep01;
    [SerializeField] private Transform sheep02;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset;

    [Header("Camera Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Zoom Settings")]
    [SerializeField] private float defaultZ = -10f;
    [SerializeField] private float maxZoomOut = -20f;
    [SerializeField] private float zoomThreshold = 5f;
    [SerializeField] private float zoomSpeed = 2f;

    void LateUpdate()
    {
        Vector3 centerPoint = (sheep01.position + sheep02.position) / 2;
        Vector3 targetPosition = centerPoint + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // calculate distance between sheep
        float distance = Vector2.Distance(sheep01.position, sheep02.position);

        // zoom out if sheep are far apart
        float targetZ = defaultZ;
        if (distance > zoomThreshold)
            targetZ = Mathf.Lerp(defaultZ, maxZoomOut, (distance - zoomThreshold) / zoomThreshold);

        targetZ = Mathf.Clamp(targetZ, maxZoomOut, defaultZ);
        targetPosition.z = Mathf.Lerp(transform.position.z, targetZ, zoomSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
