using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReloadEndBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController playerController = animator.GetComponentInParent<PlayerController>();

        if(playerController != null)
        {
            playerController.ReloadComplete();
        }
    }
}
