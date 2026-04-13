using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float blackDuration = 2f;
    [SerializeField] private Image fadeImage;
    [SerializeField] private AudioClip transitionSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (fadeImage != null)
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
    }

    public void OnStartClicked()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        // fade to black over 3 seconds
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        // play sound effect
        audioSource.PlayOneShot(transitionSound);

        // wait 2 seconds in full black
        yield return new WaitForSeconds(blackDuration);

        SceneController.instance.LoadScene("TutorialScene");
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}