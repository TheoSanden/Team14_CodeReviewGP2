using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpiderAnimatorController : MonoBehaviour
{
    public SpiderLeg[] SpiderLegs;
    public VFXPlayer[] vfx;

    public Action shootSkyProjectile;

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayTargetAnimation(string animation)
    {
        anim.Play(animation);
    }

    public void playVFX(int i)
    {
        vfx[i].PlayVFX();
    }
    public bool AllowedToMove()
    {
        foreach (SpiderLeg s in SpiderLegs)
        {
            if (s.isMoving)
                return false;
        }

        return true;
    }

    public void ShootSkyProjectile()
    {
        shootSkyProjectile?.Invoke();
    }
}
