using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AggroBubble : MonoBehaviour
{

    public Action Entered;
    public float radius;
    SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius= radius;
        coll.isTrigger= true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Entered?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,0,0.2f);
        Gizmos.DrawSphere(transform.position, radius);
        coll = GetComponent<SphereCollider>();
        coll.radius= radius;
    }
}
