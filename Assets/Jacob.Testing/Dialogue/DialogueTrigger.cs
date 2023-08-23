using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private InterfaceManager ui;
    [SerializeField] private VillagerScript currentVillager;
    //private MovementInput movement;
    public CinemachineTargetGroup targetGroup;

    [Space]

    [Header("Post Processing")]
    public Volume dialogueDof;

    void Start()
    {
        ui = InterfaceManager.instance;
       // movement = GetComponent<MovementInput>();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.IsPressed() && !ui.inDialogue && currentVillager != null)
        {
            targetGroup.m_Targets[1].target = currentVillager.transform;
            //movement.active = false;
            ui.SetCharNameAndColor();
            ui.inDialogue = true;
            ui.CameraChange(true);
            ui.ClearText();
            ui.FadeUI(true, .2f, .65f);
            currentVillager.TurnToPlayer(transform.position);

            Debug.Log("SpaceBar");
        }

        //if (Keyboard.current.spaceKey.IsPressed())
        //    {
        //    Debug.Log("Poop");
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Villager"))
        {
            currentVillager = other.GetComponent<VillagerScript>();
            ui.currentVillager = currentVillager;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Villager"))
        {
            currentVillager = null;
            ui.currentVillager = currentVillager;
        }
    }

}
