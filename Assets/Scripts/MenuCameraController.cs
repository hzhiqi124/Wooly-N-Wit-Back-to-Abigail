using UnityEngine;
using System.Collections;
using ChristinaCreatesGames.Animations;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float minY;
    [SerializeField] private float slowDownDistance = 3f;
    [SerializeField] private GameObject titleUI;
    [SerializeField] private GameObject buttonsUI;
    [SerializeField] private float titleDelay = 5f;
    [SerializeField] private float fadeDuration = 1f;

    private float timer = 0f;
    private bool titleActivated = false;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup buttonsCanvasGroup;

    void Start()
    {
        titleUI.SetActive(true);
        buttonsUI.SetActive(true);
        titleCanvasGroup = titleUI.GetComponent<CanvasGroup>();
        titleCanvasGroup.alpha = 0f;
        buttonsCanvasGroup = buttonsUI.GetComponent<CanvasGroup>();
        buttonsCanvasGroup.alpha = 0f;
    }


    void Update()
    {
        if (transform.position.y > minY)
        {
            float distanceToStop = transform.position.y - minY;
            float speed = scrollSpeed;

            if (distanceToStop < slowDownDistance)
                speed = scrollSpeed * (distanceToStop / slowDownDistance);

            Vector3 pos = transform.position;
            pos.y = Mathf.Max(pos.y - speed * Time.deltaTime, minY);
            transform.position = pos;
        }

        if (!titleActivated)
        {
            timer += Time.deltaTime;
            if (timer >= titleDelay)
            {
                titleActivated = true;
                titleUI.SetActive(true);
                buttonsUI.SetActive(true);
                titleUI.GetComponent<SquashAndStretch>().PlaySquashAndStretch();
                StartCoroutine(FadeIn(titleCanvasGroup, 0f));
                StartCoroutine(FadeIn(buttonsCanvasGroup, 2f));
            }
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(2f);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}