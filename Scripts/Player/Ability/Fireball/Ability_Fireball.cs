using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Fireball : ProjectileAbility
{
    [SerializeField] private GameObject _fireballRB;
    [SerializeField] private FloatVariable _maxForce;
    [SerializeField] private FloatVariable _cooldownDuration;
    [SerializeField] private FloatVariable _chargeRate;
    [SerializeField] private float _baseForce = 7.1f;
    [SerializeField] Vector3 spawnPosition;
    private float _force;
    bool _chargingUp = false;
    private Transform _tf;

    [SerializeField] private Transform FireIndicator;
    [SerializeField] private LayerMask indicatorMask;
    private LineRenderer FireIndicatorLineRenderer;
    [SerializeField][Range(10, 100f)] private int linePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.1f;
    private float fireMass;

    public UnityEvent WasOnCD;

    #region Audio
    private AudioSource source;
    [SerializeField] private AudioClip projectileClip;
    #endregion

    private void Awake()
    {
        fireMass = _fireballRB.GetComponent<Rigidbody>().mass;
        _tf = this.GameObject().transform;
        _force = _baseForce;
        FireIndicatorLineRenderer = FireIndicator.GetComponent<LineRenderer>();

        if (!gameObject.TryGetComponent<AudioSource>(out source)) 
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetSpawnPosition(), 0.2f);
    }

    private void Update()
    {
        if (_chargingUp)
        {
            _force += _chargeRate.Value * Time.deltaTime;
            _force = Mathf.Clamp(_force, _baseForce, _maxForce.Value);
            Indicator();
        }
        CooldownTimer();
    }

    public override void ChargedUp()
    {
        if (_onCD)
        {
            WasOnCD?.Invoke();
        }
        if (_fireballRB != null && !_onCD && _chargingUp)
        {
            source.PlayOneShot(projectileClip);

            FireballFunctionality fireball = Instantiate(_fireballRB, GetSpawnPosition(), _tf.rotation).GetComponent<FireballFunctionality>();


            fireball.GetComponent<Rigidbody>().AddForce((_tf.forward + Vector3.up * 0.5f) * _force, ForceMode.Impulse);

            _cdTimer = _cooldownDuration.Value;
            _onCD = true;

        }
        _chargingUp = false;
        FireIndicator.gameObject.SetActive(false);
        FireIndicatorLineRenderer.enabled = false;

    }

    public override void UseAbility()
    {
        if (_onCD || _chargingUp)
        {
            return; 
        }

        _force = _baseForce;

        _chargingUp = true;
        FireIndicator.gameObject.SetActive(true);
        FireIndicatorLineRenderer.enabled = true;

        FireIndicator.position = _tf.position;
    }

    private void Indicator()
    {
        FireIndicatorLineRenderer.positionCount = Mathf.CeilToInt(MathF.Ceiling(linePoints / TimeBetweenPoints) + 1);
        Vector3 startPosition = GetSpawnPosition();
        Vector3 startVelocity = _force * (_tf.forward + Vector3.up * 0.5f) / fireMass;
        int i = 0;
        FireIndicatorLineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            FireIndicatorLineRenderer.SetPosition(i, point);

            Vector3 lastPosition = FireIndicatorLineRenderer.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, indicatorMask))
            {
                FireIndicatorLineRenderer.SetPosition(i, hit.point);
                FireIndicatorLineRenderer.positionCount = i + 1;

                FireIndicator.position = FireIndicatorLineRenderer.GetPosition(i);

                return;
            }
        }
    }
    private Vector3 GetSpawnPosition()
    {
        return transform.localToWorldMatrix.MultiplyPoint(spawnPosition);
    }
}
