using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShieldFunctionality : MonoBehaviour
{
    [SerializeField] private IntVariable _shieldPerSecond;
    [SerializeField] private IntVariable _healPerSecond;
    [SerializeField] private FloatVariable _areaOfEffect;
    [SerializeField] private FloatVariable _shieldTickRate;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _lifetime = 5;
    private float _count = 0;

    private bool _canApply = true;


    private void Update()
    {
        ApplyHealAndShield();
        SelfDespawn();
    }


    private void ApplyHealAndShield()
    {
        if (_canApply)
        {
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, _areaOfEffect.Value, _mask);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    ShieldHealth sHealth = col.GetComponent<ShieldHealth>();
                    sHealth.ModifyShield(_shieldPerSecond.Value);
                    sHealth.Apply(_healPerSecond.Value);
                }
            }
            _canApply = false;
            StartCoroutine(ShieldICD());
        }
    }
    
    IEnumerator ShieldICD()
    {
        yield return new WaitForSeconds(_shieldTickRate.Value);
        _canApply = true;
    }

    private void SelfDespawn()
    {
        _count += Time.deltaTime;
        if (_count >= _lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
