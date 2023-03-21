using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : AnimatorHandler
{
    PlayerInputScript input;
    public Renderer render;
    public float hitFlashDuration;
    private int lastHealth = 9999;

    private void Start()
    {
        input = GetComponentInParent<PlayerInputScript>();
    }

    private void Update()
    {
        anim.SetBool("isMoving", input._moveVector.magnitude > 0.3f ? true : false);
    }

    public void HitEffect(int amount)
    {
        Debug.Log(lastHealth + "  " + amount);
        if (amount >= lastHealth) return;

        lastHealth = amount;
        StartCoroutine(hitRoutine(hitFlashDuration));
    }
    IEnumerator hitRoutine(float duration)
    {
        float t = 0;
        while(t < duration)
        {
            t += Time.deltaTime;
            render.material.SetFloat("_ColorOverride", t / duration);
            yield return null;
        }


    }

}
