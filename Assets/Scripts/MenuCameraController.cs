using UnityEngine;
using System.Collections;
using ChristinaCreatesGames.Animations;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float minY;
    [SerializeField] private float slowDownDistance = 3f;
    [SerializeField] private GameObject titleUI;
    [SerializeField] private GameObject titleUI02;
    [SerializeField] private GameObject buttonsUI;
    [SerializeField] private GameObject buttonsUI02;
    [SerializeField] private float titleDelay = 5f;
    [SerializeField] private float fadeDuration = 1f;

    private float timer = 0f;
    private bool titleActivated = false;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup titleCanvasGroup02;
    private CanvasGroup buttonsCanvasGroup;
    private CanvasGroup buttonsCanvasGroup02;

    private float buttonsFadeTimer = 0f;
    private bool buttonsFading = false;

    void Start()
    {
        titleUI.SetActive(true);
        titleUI02.SetActive(true);
        buttonsUI.SetActive(true);
        buttonsUI02.SetActive(true);

        titleCanvasGroup = titleUI.GetComponent<CanvasGroup>();
        titleCanvasGroup.alpha = 0f;

        titleCanvasGroup02 = titleUI02.GetComponent<CanvasGroup>();
        titleCanvasGroup02.alpha = 0f;

        buttonsCanvasGroup = buttonsUI.GetComponent<CanvasGroup>();
        buttonsCanvasGroup.alpha = 0f;

        buttonsCanvasGroup02 = buttonsUI02.GetComponent<CanvasGroup>();
        buttonsCanvasGroup02.alpha = 0f;
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
                titleUI.GetComponent<SquashAndStretch>().PlaySquashAndStretch();
                titleUI02.GetComponent<SquashAndStretch>().PlaySquashAndStretch();
                StartCoroutine(FadeIn(titleCanvasGroup, 0f));
                StartCoroutine(FadeIn(titleCanvasGroup02, 0f));
                Invoke("StartButtonsFade", 2f);
            }
        }

        if (buttonsFading)
        {
            buttonsFadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, buttonsFadeTimer / fadeDuration);
            buttonsCanvasGroup.alpha = alpha;
            buttonsCanvasGroup02.alpha = alpha;
            if (buttonsFadeTimer >= fadeDuration)
            {
                buttonsCanvasGroup.alpha = 1f;
                buttonsCanvasGroup02.alpha = 1f;
                buttonsFading = false;
            }
        }
    }

    void StartButtonsFade()
    {
        buttonsFading = true;
        buttonsFadeTimer = 0f;
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
}