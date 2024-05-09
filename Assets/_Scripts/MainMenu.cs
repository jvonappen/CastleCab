using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayScene(string _sceneName)
    {
        GameManager.Instance.OpenCustomization();
        GameManager.Instance.GetComponent<SceneToLoad>().SetSceneToLoad(_sceneName);

        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
