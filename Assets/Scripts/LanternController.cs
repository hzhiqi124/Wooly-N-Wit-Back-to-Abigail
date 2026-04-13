using UnityEngine;

public class LanternController : MonoBehaviour
{
    [Header("Scale Animation")]
    [SerializeField] private float minScaleY = 0.5f;
    [SerializeField] private float scaleSpeed = 2f;

    [Header("Linked Box")]
    [SerializeField] private Transform linkedBox;
    [SerializeField] private BoxCollider2D triggerZone;

    private Vector3 originalScale;
    private float originalX;
    private float linkedBoxOriginalX;

    void Start()
    {
        originalScale = transform.localScale;
        originalX = transform.position.x;
        if (linkedBox != null)
            linkedBoxOriginalX = linkedBox.position.x;
    }

    void Update()
    {
        // scale Y in a loop
        float scaleY = Mathf.Lerp(minScaleY, originalScale.y,
            (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f);
        transform.localScale = new Vector3(originalScale.x, scaleY, originalScale.z);

        // follow linked box X offset
        if (linkedBox != null)
        {
            float offsetX = linkedBox.position.x - linkedBoxOriginalX;
            transform.position = new Vector3(
                originalX + offsetX,
                transform.position.y,
                transform.position.z);
        }
    }
}