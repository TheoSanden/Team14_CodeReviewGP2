using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Wind : ProjectileAbility
{
    [SerializeField] private GameObject _windOrb;
    [SerializeField] private FloatVariable _force;
    [SerializeField] private FloatVariable _cooldownDuration;

    [SerializeField] private AudioSource windProjectile;
    [SerializeField] private AudioClip windsound;

    private Transform _tf;

    public GameObject indicator;

    bool isHolding;
    public UnityEvent WasOnCD;

private void Awake()
    {
        _tf = this.GameObject().transform;
    }

    private void Update()
    {
        CooldownTimer();
        if (isHolding) Indicator();
    }

    public override void ChargedUp()
    {
        if (_onCD)
        {
            WasOnCD?.Invoke();
        }
        if (_windOrb != null && !_onCD)
        {
            windProjectile.PlayOneShot(windsound);
            GameObject go = Instantiate(_windOrb, _tf.position + _tf.forward + Vector3.up * 0.5f, _tf.rotation);
            go.GetComponent<Rigidbody>().AddForce(_tf.forward * _force.Value, ForceMode.Impulse);
            _cdTimer = _cooldownDuration.Value;
            _onCD = true;

        }
        isHolding = false;
        indicator.SetActive(false);
    }
    public override void UseAbility()
    {
        isHolding = true;
        indicator.SetActive(true);

    }
    private void Indicator()
    {
        indicator.transform.forward = _tf.forward;
    }
}
