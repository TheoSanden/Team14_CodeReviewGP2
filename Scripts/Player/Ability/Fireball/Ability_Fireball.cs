using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Ability_Fireball : ProjectileAbility
{
    [SerializeField] private GameObject fireballCD;
    [SerializeField] private FloatVariable maxForce;
    [SerializeField] private FloatVariable coolDownDuration;
    [SerializeField] private FloatVariable chargeRate;
    [SerializeField] private float baseForce = 7.1f;
    [SerializeField] Vector3 spawnPosition;
    private float force;
    bool chargingUp = false;
    private Transform tf;
    [SerializeField] private Transform fireIndicator;
    [SerializeField] private LayerMask indicatorMask;
    private LineRenderer fireIndicatorLineRenderer;
    [SerializeField][Range(10, 100f)] private int linePoints = 25;
    [SerializeField][Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

    private float fireMass;
    public UnityEvent wasOnCD;
    #region Audio
    private AudioSource source;
    [SerializeField] private AudioClip projectileClip;
    #endregion

    private void Awake()
    {
        fireMass = fireballCD.GetComponent<Rigidbody>().mass;
        tf = this.GameObject().transform;
        force = baseForce;
        fireIndicatorLineRenderer = fireIndicator.GetComponent<LineRenderer>();

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
        if (chargingUp)
        {
            force += chargeRate.Value * Time.deltaTime;
            force = Mathf.Clamp(force, baseForce, maxForce.Value);
            Indicator();
        }
        CooldownTimer();
    }

    public override void ChargedUp()
    {
        if (_onCD)
        {
            wasOnCD?.Invoke();
        }
        if (fireballCD != null && !_onCD && chargingUp)
        {
            source.PlayOneShot(projectileClip);

            FireballFunctionality fireball = Instantiate(fireballCD, GetSpawnPosition(), tf.rotation).GetComponent<FireballFunctionality>();


            fireball.GetComponent<Rigidbody>().AddForce( tf.forward + Vector3.up * 0.5f) * force, ForceMode.Impulse);

            _cdTimer = coolDownDuration.Value;
            _onCD = true;

        }
        chargingUp = false;
        fireIndicator.gameObject.SetActive(false);
        fireIndicatorLineRenderer.enabled = false;

    }

    public override void UseAbility()
    {
        if (_onCD || chargingUp)
        {
            return; 
        }

        force = baseForce;

        chargingUp = true;
        fireIndicator.gameObject.SetActive(true);
        fireIndicatorLineRenderer.enabled = true;

        fireIndicator.position = tf.position;
    }

    private void Indicator()
    {
        fireIndicatorLineRenderer.positionCount = Mathf.CeilToInt(MathF.Ceiling(linePoints / timeBetweenPoints
) + 1);
        Vector3 startPosition = GetSpawnPosition();
        Vector3 startVelocity = force *  tf.forward + Vector3.up * 0.5f) / fireMass;
        int i = 0;
        fireIndicatorLineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints
)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            fireIndicatorLineRenderer.SetPosition(i, point);

            Vector3 lastPosition = fireIndicatorLineRenderer.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, indicatorMask))
            {
                fireIndicatorLineRenderer.SetPosition(i, hit.point);
                fireIndicatorLineRenderer.positionCount = i + 1;

                fireIndicator.position = fireIndicatorLineRenderer.GetPosition(i);

                return;
            }
        }
    }
    private Vector3 GetSpawnPosition()
    {
        return transform.localToWorldMatrix.MultiplyPoint(spawnPosition);
    }
}
