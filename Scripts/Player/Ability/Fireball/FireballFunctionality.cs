using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Unity.VisualScripting;
using UnityEngine;

public class FireballFunctionality : Ability
{
    [SerializeField] private GameObject _firePuddle;
    [SerializeField] private IntVariable _fireballDamage;
    [SerializeField] private int _lifeTime = 5;
    private float _count = 0;

    void Start()
    {
        elementType = InteractionArgs.ElementType.fire;
    }
    private void Update()
    {
        _count += Time.deltaTime;
        if (_count > _lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Vector3 worldHitPoint = col.contacts[0].point;
            GameObject.Instantiate(_firePuddle, worldHitPoint, default);

        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Health>().Apply(-_fireballDamage.Value);
            HandleInteractions(col.transform);
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 2);
            foreach (Collider _col in colliders)
            {
                if (_col.CompareTag("Enemy"))
                {
                    HandleInteractions(_col.transform);
                }
            }
        }
        else
        {
            HandleInteractions(col.transform);
        }
        Destroy(this.gameObject);
    }


}
