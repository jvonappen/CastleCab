using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetCosmeticsOnEnable : MonoBehaviour
{
    [SerializeField] PlayerCustomization m_playerBase;

    private void OnEnable()
    {
        if (m_playerBase.playerInput)
        {
            HorseColourSelector horseSelector = gameObject.GetComponentInChildren<HorseColourSelector>();
            horseSelector.Init();
            horseSelector.GetComponent<SkinSelector>().Init();

            PlayerData data = GameManager.Instance.GetPlayerData(m_playerBase.playerInput.gameObject);

            foreach (ModelSelector modelSelector in gameObject.GetComponentsInChildren<ModelSelector>())
            {
                modelSelector.Init();

                ModelCustomization foundItem = data.modelCustomizations.FirstOrDefault(item => item.typeIndex == modelSelector.m_typeIndex);

                modelSelector.PreviewObjectByIndex(foundItem.index);
                modelSelector.SelectObject();

                if (foundItem.mat.mainDye.colour != null) modelSelector.colourSelector.SetDye("Main", foundItem.mat.mainDye);
                if (foundItem.mat.secondaryDye.colour != null) modelSelector.colourSelector.SetDye("Secondary", foundItem.mat.secondaryDye);
                if (foundItem.mat.tertiaryDye.colour != null) modelSelector.colourSelector.SetDye("Tertiary", foundItem.mat.tertiaryDye);
            }

            horseSelector.SetDyes(data.horseMat);
        }
    }
}
