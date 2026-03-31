using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private int playersInside = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersInside++;
            if (playersInside >= 2)
            //go to next level
            SceneController.instance.NextLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playersInside--;
    }
}
