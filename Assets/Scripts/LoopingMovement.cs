using UnityEngine;

public class LoopMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = startPos.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}