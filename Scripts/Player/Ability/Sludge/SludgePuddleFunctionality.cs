using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SludgePuddleFunctionality : PuddleFunctionality
{
    void Start()
    {
        elementType = InteractionArgs.ElementType.sludge;
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
