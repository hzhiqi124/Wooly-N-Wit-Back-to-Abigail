using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject finishPointWall;
    [SerializeField] private GameObject lightWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finishPointWall.SetActive(true);
            lightWall.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
