using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Radiation : TrailAbility
{
    [SerializeField] private GameObject _radiationPool;
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private FloatVariable _force;
    [SerializeField] private FloatVariable _spewDuration;
    [SerializeField] private FloatVariable _spewInterval;
    [SerializeField] private FloatVariable _cooldownDuration;
    [SerializeField] private float _rayCastRange = 1.5f;

    [SerializeField] private AudioSource radiation;
    [SerializeField] private AudioClip acidTrail;

    private Transform _tf;
    public float _spewTimer;
    private bool _spewing = false;
    public UnityEvent<float, float> CooldownEvent;
    public UnityEvent WasOnCD;
private void Awake()
    {
        _playerRB = this.GameObject().GetComponent<Rigidbody>();
        _tf = this.GameObject().transform;
    }

    private void Update()
    {
        SpewTimer();
        float currentT = CooldownTimer();
        CooldownEvent?.Invoke(currentT, _cooldownDuration.Value);
    }

    private void FixedUpdate()
    {
        if (_spewing)
        {
            _playerRB.MovePosition(_tf.position + TryMove(-_tf.forward) * _force.Value * Time.fixedDeltaTime);
        }
    }

    public override void UseAbility()
    {
        if (_onCD)
        {
            WasOnCD?.Invoke();
        }
        if (!_onCD)
        {
            radiation.PlayOneShot(acidTrail);
            
            _cdTimer = _cooldownDuration.Value;
            _spewTimer = _spewDuration.Value;
            _onCD = true;
            _spewing = true;
            StartCoroutine(Spew());
        }
       
    }
    
    public override bool AllowMove()
    {
        if (_spewing)
            return false;
        return true;
    }
    
    private void SpewTimer()
    {
        if (_spewing)
        {
            _spewTimer -= Time.deltaTime;
            if (_spewTimer <= 0)
            {
                StopCoroutine(Spew());
                _spewing = false;
                _spewTimer = _spewDuration.Value;
            }
        }
    }
    
    private Vector3 TryMove(Vector3 movement)
    {
        Vector3 rayOrg = this.gameObject.transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrg, movement, _rayCastRange, _trailMask))
        {
            return Vector3.zero;
        }
        return movement;
    }
    
    IEnumerator Spew()
    {
        while (_spewing)
        {
            Vector3 rayOrigin = _tf.position + _tf.forward + Vector3.up * 2;
            Vector3 rayDir = -Vector3.up;
            StickToSurface(_radiationPool, rayOrigin, rayDir);
            
            yield return new WaitForSeconds(_spewInterval.Value);
        }
    }


    
    

}
