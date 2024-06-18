using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UnityEvent onPlayScene;

    public void PlayScene(string _sceneName)
    {
        GameManager.Instance.OpenCustomization();
        GameManager.Instance.GetComponent<SceneToLoad>().SetSceneToLoad(_sceneName);

        TimerManager.RunAfterTime(() =>
        {
            gameObject.SetActive(false);
            onPlayScene?.Invoke();
        }, 0.05f);
    }

    public void SkipCustomization(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
