using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayScene(string _sceneName)
    {
        GameManager.Instance.OpenCustomization();
        GameManager.Instance.GetComponent<SceneToLoad>().SetSceneToLoad(_sceneName);

        gameObject.SetActive(false);
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
