using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject finishPointWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finishPointWall.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
