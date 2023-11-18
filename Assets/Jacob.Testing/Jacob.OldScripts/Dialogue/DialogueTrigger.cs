using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    //[SerializeField] private InterfaceManager ui;

    //public CinemachineTargetGroup targetGroup;

    //[Header("Debug")]
    //[SerializeField] private VillagerScript currentVillager;
    //[SerializeField] private PlayerMovement playerMovement;


    //void Start()
    //{
    //    ui = InterfaceManager.instance;
    //}

    //void Update()
    //{
    //    if (Keyboard.current.enterKey.wasPressedThisFrame && !ui.inDialogue && currentVillager != null) //
    //    {
    //        targetGroup.m_Targets[1].target = currentVillager.transform;
    //        ui.SetCharNameAndColor();
    //        ui.inDialogue = true;
    //        ui.CameraChange(true);
    //        ui.ClearText();
    //        ui.FadeUI(true, .2f, .65f);

    //        playerMovement.freeze = true;
    //        playerMovement.enabled = false;
    //    }
    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Villager"))
    //    {
    //        currentVillager = other.GetComponent<VillagerScript>();
    //        ui.currentVillager = currentVillager;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Villager"))
    //    {
    //        currentVillager = null;
    //        ui.currentVillager = currentVillager;
    //    }
    //}

}
