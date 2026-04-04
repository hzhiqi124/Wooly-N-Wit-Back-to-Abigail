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

    void LateUpdate()
    {
        Vector3 centerPoint = (sheep01.position + sheep02.position) / 2;
        Vector3 targetPosition = centerPoint + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}