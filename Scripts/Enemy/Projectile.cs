using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isUsed;
    public LayerMask hitableLayer;

    private float speed;
    private float lifeTime;
    private float currentLifeTime;

    ProjectileData currentData;
    Rigidbody rb;

    private Vector3 moveVector;

    public GameObject normalStyle;
    public GameObject bigStyle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Perform(ProjectileData data)
    {
        SetupData(data);
        gameObject.SetActive(true);
        isUsed = true;
    }

    private void SetupData(ProjectileData data)
    {
        currentData = data;
        speed = data.Speed;
        lifeTime = data.LifeTime;
        currentLifeTime = 0;

        switch (data.style)
        {
            case projectileStyle.normal:
                normalStyle.SetActive(true);
                break;
            case projectileStyle.big:
                bigStyle.SetActive(true);
                break;
        }
    }

    private void Hit(GameObject hit)
    {
       
        if (hit.CompareTag("Player"))
        {
            hit.GetComponent<Health>().Apply(-currentData.Damage);
        }
        Die();
    }
    private void Die()
    {
        isUsed = false;
        gameObject.SetActive(false);
        normalStyle.SetActive(false);
        bigStyle.SetActive(false);

    }

    private void FixedUpdate()
    {
        if (!isUsed) return;

        moveVector = transform.forward * (speed * Time.fixedDeltaTime);

        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + moveVector, Color.red, Time.fixedDeltaTime);
        if(Physics.Linecast(transform.position, transform.position + moveVector, out hit, hitableLayer))
        {
            Hit(hit.transform.gameObject);
        }
        
        
        transform.position += moveVector;

        // lifetime
        if(currentLifeTime > lifeTime)
        {
            Die();
        } else
        {
            currentLifeTime += Time.fixedDeltaTime;
        }


    }
}
