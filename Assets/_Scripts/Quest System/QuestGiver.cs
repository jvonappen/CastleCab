using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest quest;

    //ref player

    [Header("QuestWindow")]
    [SerializeField] private GameObject questWindow;
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI goldRewardText;
    [SerializeField] private TextMeshProUGUI itemRewardText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        questTitleText.text = quest.questTitle;
        questDescriptionText.text= quest.questDescription;
        goldRewardText.text = quest.goldReward.ToString();
        itemRewardText.text= quest.itemReward.ToString(); /// need to change to int description later
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        GameManager.Instance.quest = quest;
    }


}
