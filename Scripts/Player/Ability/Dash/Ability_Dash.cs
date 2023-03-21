using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Ability_Dash : AbilityScript
{
    [SerializeField] private FloatVariable _dashLength;
    [SerializeField] private FloatVariable _dashCooldown;
    [SerializeField] private float _safetyRange = 1f;
    [Header("Spherecast Settings")]
    [SerializeField] private float _upwardOffset = 0.5f;
    [SerializeField] private float _backwardOffset = 0.1f;
    [SerializeField] private float _sphereCastRadius = 0.4f;
    [SerializeField] private LayerMask _mask;
    private Transform _tf;
    private Rigidbody _rb;

    [SerializeField] private AudioSource dash;
    [SerializeField] private AudioClip dashyeah;

    private void Awake()
    {
        if (!this.gameObject.TryGetComponent<Rigidbody>(out _rb))
        {
            Debug.Log("Missing Rigid Body");
        }
        _tf = this.gameObject.transform;
    }

    private void Update()
    {
        CooldownTimer();
    }

    public override void UseAbility()
    {
        if (!_onCD)
        {
            dash.PlayOneShot(dashyeah);

            // TryDash();
            
            Vector3 pushDir = (_tf.forward + _tf.up * 0.5f).normalized;
            _rb.AddForce(pushDir * _dashLength.Value, ForceMode.Impulse);

            _cdTimer = _dashCooldown.Value;
            _onCD = true;
            
            
        }

    }

    private void TryDash()
    {
        RaycastHit hit;
        Vector3 backOffset = -_tf.forward * _backwardOffset;
        Vector3 upOffset = _tf.up * _upwardOffset;
        Vector3 rayOrg = _tf.position + backOffset + upOffset;
        
        if (Physics.SphereCast(rayOrg, _sphereCastRadius, _tf.forward, out hit, _dashLength.Value, _mask))
        {
            Debug.Log(hit.transform.name);
            Vector3 destination = hit.point - _tf.forward * _safetyRange;
            destination.y = _tf.position.y;
            _tf.position = destination;
        }
        else
        {
            _tf.position += _tf.forward * _dashLength.Value;
        }
    }
}
