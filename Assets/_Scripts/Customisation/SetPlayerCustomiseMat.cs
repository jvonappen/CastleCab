using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerCustomiseMat : MonoBehaviour
{
    [SerializeField] List<GameObject> m_backgrounds;
    [SerializeField] List<Material> m_playerCustomisationBGMats;

    private void Start()
    {
        int index = 0;
        if (GameManager.Instance.players.Count != 0) index = (GameManager.Instance.players.Count - 1) % m_playerCustomisationBGMats.Count;

        Material mat = m_playerCustomisationBGMats[index];

        foreach (GameObject bg in m_backgrounds)
        {
            bg.GetComponent<Image>().material = mat;
            if (bg.TryGetComponent(out ShaderRandomizer shaderRandomizer)) shaderRandomizer.SetMaterial(mat);
        }
    }
}
