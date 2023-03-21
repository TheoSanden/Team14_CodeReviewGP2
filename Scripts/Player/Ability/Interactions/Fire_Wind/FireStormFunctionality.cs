using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStormFunctionality : MonoBehaviour
{
    [SerializeField] private IntVariable _fireStormDamage;
    [SerializeField] private FloatVariable _lifeTime;
    [SerializeField] private FloatVariable _areaOfEffect;
    [SerializeField] private FloatVariable _damageTickInterval;
    [SerializeField] private FloatVariable _suckForce;
    [SerializeField] private LayerMask _suckMask;
    [SerializeField] private LayerMask _damageMask;

    [SerializeField] private AudioClip clip;
    private AudioSource audioSource;

    private bool _canDamage = true;
    private float _count = 0;
    private float _lerpFloat;

    private void OnEnable()
    {
        if (!gameObject.TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(clip);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _areaOfEffect.Value);
    }
    private void Update()
    {
        _count += Time.deltaTime;
        if (_count > _lifeTime.Value)
        {
            Destroy(this.gameObject);
        }

        SuckEnemies();
        DamageInArea();

    }

    private void DamageInArea()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _areaOfEffect.Value, _damageMask);
        if (_canDamage)
        {
            _canDamage = false;
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<Health>().Apply(-_fireStormDamage.Value);
                }
            }
            StartCoroutine(DamageTickInterval());

        }
    }

    private void SuckEnemies()
    {
        float suck = _suckForce.Value / 10000;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _areaOfEffect.Value, _suckMask);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Transform tf = col.transform;
                _lerpFloat = Mathf.MoveTowards(_lerpFloat, 1, suck * Time.deltaTime);
                tf.position = Vector3.Lerp(tf.position, this.gameObject.transform.position, _lerpFloat);
            }
        }
    }

    IEnumerator DamageTickInterval()
    {
        yield return new WaitForSeconds(_damageTickInterval.Value);
        _canDamage = true;
    }
}
