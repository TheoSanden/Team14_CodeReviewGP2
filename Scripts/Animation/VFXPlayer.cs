using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;

    private void Start()
    {
        if(vfx == null)
        vfx = GetComponent<VisualEffect>();
    }

    public void PlayVFX()
    {
        if(vfx != null)
        vfx.Play();
    }
}
