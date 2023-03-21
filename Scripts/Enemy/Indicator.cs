using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Indicator : MonoBehaviour
{
    DecalProjector projector;
    private void Awake()
    {
        projector = GetComponent<DecalProjector>();
    }

    public void SetRadius(float radius)
    {
        projector.pivot = Vector3.forward * (radius / 2);
        projector.size = Vector3.one * radius;
    }
    

    public void Display(bool state) => gameObject.SetActive(state);
    
}
