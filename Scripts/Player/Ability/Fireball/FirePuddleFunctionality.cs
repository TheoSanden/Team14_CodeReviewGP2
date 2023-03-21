using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePuddleFunctionality : PuddleFunctionality
{
    void Start()
    {
        elementType = InteractionArgs.ElementType.fire;
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
