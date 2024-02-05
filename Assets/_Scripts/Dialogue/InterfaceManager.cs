using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.InputSystem;

public class InterfaceManager : MonoBehaviour
{
    //public bool inDialogue;

    //public static InterfaceManager instance;

    //public CanvasGroup canvasGroup;
    //public TMP_Animated animatedText;

    //public Image nameBubble;
    //public TextMeshProUGUI nameTMP;

    //[HideInInspector]
    //public VillagerScript currentVillager;

    //private int dialogueIndex;

    //public bool canExit;
    //public bool nextDialogue;

    //[Space]

    //[Header("Cameras")]
    //public GameObject gameCam;
    //public GameObject dialogueCam;

    //[Header("Debug")]
    //[SerializeField] private PlayerMovement playerMovement;

    ////[Space]

    //public Volume dialogueDof;


    //private void Awake()
    //{
    //    instance = this;
    //}

    //private void Start()
    //{
    //    animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
    //}

    //private void Update()
    //{
    //    if (Keyboard.current.enterKey.wasPressedThisFrame && inDialogue)
    //    {
    //        if (canExit)
    //        {
    //            CameraChange(false);
    //            FadeUI(false, .2f, 0);
    //            DG.Tweening.Sequence s = DOTween.Sequence();
    //            s.AppendInterval(.8f);
    //            s.AppendCallback(() => ResetState());
    //        }

    //        if (nextDialogue)
    //        {
    //            animatedText.ReadText(currentVillager.dialogue.conversationBlock[dialogueIndex]);
    //        }
    //    }
    //}

    //public void FadeUI(bool show, float time, float delay)
    //{

    //    DG.Tweening.Sequence s = DOTween.Sequence();
    //    s.AppendInterval(delay);
    //    s.Append(canvasGroup.DOFade(show ? 1 : 0, time));
    //    if (show)
    //    {
    //        dialogueIndex = 0;
    //        s.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
    //        s.AppendCallback(() => animatedText.ReadText(currentVillager.dialogue.conversationBlock[0]));
    //    }
    //}

    //public void SetCharNameAndColor()
    //{
    //    nameTMP.text = currentVillager.data.villagerName;
    //    nameTMP.color = currentVillager.data.villagerNameColor;
    //    nameBubble.color = currentVillager.data.villagerColor;

    //}

    //public void CameraChange(bool dialogue)
    //{
    //    gameCam.SetActive(!dialogue);
    //    dialogueCam.SetActive(dialogue);

    //    //Depth of field modifier
    //    float dofWeight = dialogueCam.activeSelf ? 1 : 0;
    //    //DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
    //}

    //public void DialogueDOF(float x)
    //{
    //    dialogueDof.weight = x;
    //}

    //public void ClearText()
    //{
    //    animatedText.text = string.Empty;
    //}

    //public void ResetState()
    //{
    //    //currentVillager.Reset();
    //    inDialogue = false;
    //    canExit = false;

    //}

    //public void FinishDialogue()
    //{
    //    if (dialogueIndex < currentVillager.dialogue.conversationBlock.Count -1)
    //    {
    //        dialogueIndex++;
    //        nextDialogue = true;
    //        Debug.Log("nextDialogue TRUE");
    //    }
    //    else
    //    {
    //        nextDialogue = false;
    //        canExit = true;
    //        playerMovement.enabled = true;
    //        playerMovement.freeze = false;
    //    }
    //} 
}
