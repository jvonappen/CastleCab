using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyeCollection : MonoBehaviour
{
    [SerializeField] ColourSelector m_selector;
    [SerializeField] string m_dyeType;

    [SerializeField] GameObject m_buttonPrefab;
    [SerializeField] List<SO_Dye> m_dyes;

    private void Awake()
    {
        foreach (SO_Dye dye in m_dyes)
        {
            GameObject button = Instantiate(m_buttonPrefab);
            button.transform.SetParent(transform, false);

            DyeButton dyeButton = button.GetComponent<DyeButton>();
            dyeButton.Init(m_selector, m_dyeType, dye);
            //button.GetComponent<Button>().onClick.AddListener(dyeButton.SetDye);// += dyeButton.SetDye;// m_selector.SetDye(m_dyeType, dye);
        }
    }
}
