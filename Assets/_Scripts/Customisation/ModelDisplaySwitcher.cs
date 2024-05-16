using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDisplaySwitcher : MonoBehaviour
{
    [SerializeField] List<GameObject> m_models;

    public void DisplayModel(int _index)
    {
        for (int i = 0; i < m_models.Count; i++)
        {
            if (i == _index) m_models[i].SetActive(true);
            else m_models[i].SetActive(false);
        }
    }
}
