using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPuddleFunctionality : PuddleFunctionality
{
    void Start()
    {
        elementType = InteractionArgs.ElementType.wind;
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
