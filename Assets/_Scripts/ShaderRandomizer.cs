using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderRandomizer : MonoBehaviour
{
    [SerializeField] Material m_material;
    [SerializeField] float m_timeUntilRandomize = 1;
    [SerializeField] float m_randomRangeX = 1, m_randomRangeY = 1;

    float m_pulse;

    private void Update()
    {
        if (m_pulse < 1) m_pulse += Time.deltaTime / m_timeUntilRandomize;
        else
        {
            m_pulse = 0;
            Randomize();
        }

        m_material.SetFloat("_Pulse", m_pulse);
    }

    void Randomize()
    {
        m_material.SetVector("_RandomTileOffset", new Vector4(Random.Range(-m_randomRangeX, m_randomRangeX), Random.Range(-m_randomRangeY, m_randomRangeY), 0, 0));
    }
}
