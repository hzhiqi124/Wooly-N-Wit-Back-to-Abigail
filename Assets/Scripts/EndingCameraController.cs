using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingCameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float targetX = 10f;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject blueCanvas;

    private bool reachedTarget = false;

    void Start()
    {
        blueCanvas.SetActive(false);
        fadeImage.color = Color.white;
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (transform.position.x < targetX && !reachedTarget)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (transform.position.x >= targetX)
            {
                reachedTarget = true;
                StartCoroutine(PauseAndFadeOut());
            }
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        fadeImage.color = new Color(1f, 1f, 1f, 0f);
    }

    IEnumerator PauseAndFadeOut()
    {
        yield return new WaitForSeconds(3f);

        blueCanvas.SetActive(true);
        CanvasGroup blueCanvasGroup = blueCanvas.GetComponent<CanvasGroup>();
        blueCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            blueCanvasGroup.alpha = alpha;
            yield return null;
        }
        blueCanvasGroup.alpha = 1f;

        SceneManager.LoadSceneAsync("MenuScene");
    }
}