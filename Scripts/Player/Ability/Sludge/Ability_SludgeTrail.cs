using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Ability_SludgeTrail : TrailAbility
{
    [SerializeField] private GameObject _sludgePool;
    [SerializeField] private FloatVariable _sludgeDuration;
    [SerializeField] private FloatVariable _sludgeInterval;
    [SerializeField] private FloatVariable _cooldownDuration;
    public float _sludgeTimer;
    private bool _sludging = false;

    [SerializeField] private AudioSource sludgeTrail;
    [SerializeField] private AudioClip sludge;

    public UnityEvent<float, float> CooldownEvent;
    public UnityEvent WasOnCD;

private Transform _tf;

    private void Awake()
    {
        _tf = this.GameObject().transform;
    }

    private void Update()
    {
        float currentT = CooldownTimer();
        CooldownEvent?.Invoke(currentT, _cooldownDuration.Value);
        SludgeTimer();
    }


    public override void UseAbility()
    {
        if (_onCD)
        {
            WasOnCD?.Invoke();
        }
        if (_sludgePool != null && !_onCD)
        {
            sludgeTrail.PlayOneShot(sludge);

            _cdTimer = _cooldownDuration.Value;
            _sludgeTimer = _sludgeDuration.Value;
            _onCD = true;
            _sludging = true;
            StartCoroutine(Sludging());
        }
    }

    private void SludgeTimer()
    {
        
        if (_sludging)
        {
            _sludgeTimer -= Time.deltaTime;
            if (_sludgeTimer <= 0)
            {
                StopCoroutine(Sludging());
                _sludging = false;
                _sludgeTimer = _sludgeDuration.Value;
            }
        }
    }
    IEnumerator Sludging()
    {
        while (_sludging)
        {
            Vector3 rayOrigin = _tf.position - _tf.forward * 0.5f + Vector3.up * 0.5f;
            Vector3 rayDir = -Vector3.up;
            StickToSurface(_sludgePool, rayOrigin, rayDir);

            yield return new WaitForSeconds(_sludgeInterval.Value);
        }
    }

}
