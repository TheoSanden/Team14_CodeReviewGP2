using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationPuddleFunctionality : PuddleFunctionality
{
    void Start()
    {
        elementType = InteractionArgs.ElementType.radiation;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        HandleInteractions(other.transform);
        PuddleInterationDespawn(other.transform);
    }
    private void OnCollisionEnter(Collision col)
    {
        HandleInteractions(col.transform);
        PuddleInterationDespawn(col.transform);

    }
}
