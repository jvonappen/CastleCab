using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    Animator animator;
    private string currentAnimation;

    //Animations
     const string NPC_ATTENTION = "Attention";
     const string NPC_DANCE = "Dance";
     const string NPC_FLAP = "Flap";
     const string NPC_GRANNY = "Granny";
     const string NPC_IDLE = "Idle";
     const string NPC_SIREN = "Siren";
     const string NPC_WALK = "Walk";
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    public void ChangeAnimation(string newAnimation)
    {
        //prevents interupting the animation
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

}
