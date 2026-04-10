using UnityEngine;

public class BlueStar : MonoBehaviour
{
    [SerializeField] public GameObject connectedBox;
    [SerializeField] public BlueStar pairedStar;

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            CheckPair();
        }
    }

    private void CheckPair()
    {
        if (pairedStar != null && pairedStar.isActivated)
            connectedBox.SetActive(true);
    }
}