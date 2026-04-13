using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private float delay = 5f;

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(delay);
        SceneController.instance.NextLevel();
    }
}