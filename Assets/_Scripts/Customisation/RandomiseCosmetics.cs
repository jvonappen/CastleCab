using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomiseCosmetics : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;
    [SerializeField] Transform m_modelBase;

    [SerializeField] SO_Cosmetics m_cosmeticsList;

    [SerializeField] ModelSelector m_hatSelector;
    [SerializeField] HorseColourSelector m_horseSelector;
    [SerializeField] ModelSelector m_cartSelector, m_wheelSelector;

    public void SetRandomCosmetics()
    {
        Transform model = m_modelBase.GetChild(0);
        foreach (Transform child in m_modelBase)
        {
            if (child.gameObject.activeSelf)
            {
                model = child;
                break;
            }
        }

        // Sets hat, cart, and wheels
        RandomiseSelector(m_hatSelector);
        RandomiseSelector(m_cartSelector);
        RandomiseSelector(m_wheelSelector);

        // Sets horse colours, pattern, and outfit
        SkinData randOutfit = m_cosmeticsList.m_outfits[Random.Range(0, m_cosmeticsList.m_outfits.Count)].m_data;
        Texture2D randPattern = m_cosmeticsList.m_patterns[Random.Range(0, m_cosmeticsList.m_patterns.Count)];
        m_horseSelector.SetDyes(new(GetRandomDye().GetDyeData(), GetRandomDye().GetDyeData(), GetRandomDye().GetDyeData(), GetRandomDye().GetDyeData(), GetRandomDye().GetDyeData(), GetRandomDye().GetDyeData(), randPattern, randOutfit));
        m_horseSelector.ConfirmPattern();
        m_horseSelector.skinSelector.ConfirmSkin();

        PlayerCustomization.StoreCustomisationsToPlayer(m_input, model);
        m_input.GetComponent<PlayerCustomization>().ApplyCosmeticsToPlayer();
    }

    void RandomiseSelector(ModelSelector _modelSelector)
    {
        _modelSelector.PreviewObjectByIndex(Random.Range(0, _modelSelector.GetCount()));
        _modelSelector.SelectObject();

        _modelSelector.colourSelector.SetMainDye(GetRandomDye());
        _modelSelector.colourSelector.SetSecondaryDye(GetRandomDye());
        _modelSelector.colourSelector.SetTertiaryDye(GetRandomDye());
    }

    SO_Dye GetRandomDye()
    {
        return m_cosmeticsList.m_dyes[Random.Range(0, m_cosmeticsList.m_dyes.Count - 1)];
    }
}
