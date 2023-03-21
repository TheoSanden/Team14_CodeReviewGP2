using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    protected Animator anim;
    //
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayTargetAnimation(string animation, bool interacting)
    {
        anim.SetBool("isInteracting", interacting);
        anim.Play(animation);
    }

    public bool GetIsInteracting() => anim.GetBool("isInteracting");

}
