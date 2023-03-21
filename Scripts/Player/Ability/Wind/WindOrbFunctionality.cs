using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindOrbFunctionality : Ability
{
    [SerializeField] private GameObject _windPuddle;
    [SerializeField] private IntVariable _windOrbDamage;
    [SerializeField] FloatVariable _lifeTime;
    private float _count = 0;

    void Start()
    {
        elementType = InteractionArgs.ElementType.wind;
    }

    private void Update()
    {
        _count += Time.deltaTime;
        if (_count > _lifeTime.Value)
        {
            GameObject.Instantiate(_windPuddle, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Vector3 worldHitPoint = col.contacts[0].point;
            GameObject.Instantiate(_windPuddle, worldHitPoint, default);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Health>().Apply(-_windOrbDamage.Value);
            HandleInteractions(col.transform);
            Destroy(this.gameObject);
        }
        else
        {
            HandleInteractions(col.transform);
        }
    }
}
