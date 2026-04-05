using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void OnStartClicked()
    {
        SceneController.instance.LoadScene("Level01Scene");
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