using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneToLoad : MonoBehaviour
{
    string m_sceneToLoad;
    public string sceneToLoad { get { return m_sceneToLoad; } }
    public void SetSceneToLoad(string _sceneToLoad) => m_sceneToLoad = _sceneToLoad;
}
