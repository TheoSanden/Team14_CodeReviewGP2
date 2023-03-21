using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadBlastFunctionality : MonoBehaviour
{
    [SerializeField] private IntVariable _radBlastDamage;
    [SerializeField] private IntVariable _radBlastShieldDamage;
    [SerializeField] private int _lifeTime = 1;
    private float _count = 0;

    private AudioSource source;
    [SerializeField] private AudioClip clip;
    private void Update()
    {
        SelfDespawn();
    }

    private void OnEnable()
    {
        if (!gameObject.TryGetComponent<AudioSource>(out source))
        {
            source = this.gameObject.AddComponent<AudioSource>();
        }
        source.PlayOneShot(clip);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            ShieldHealth shieldHealth;
            Health health;
            if (col.gameObject.TryGetComponent<ShieldHealth>(out shieldHealth))
            {
                if (shieldHealth.ShieldBuffer > 0)
                {
                    shieldHealth.DamageShield(_radBlastShieldDamage.Value);
                }
                else
                {
                    shieldHealth.Apply(-_radBlastDamage.Value);
                }
            }
            else if (col.gameObject.TryGetComponent<Health>(out health))
            {
                health.Apply(-_radBlastDamage.Value);
            }
            else
            {
                Debug.LogWarning("Gameobject does not contain a Health Component.", col.gameObject);
                return;
            }
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }

    private void SelfDespawn()
    {
        _count += Time.deltaTime;
        if (_count >= _lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

}
