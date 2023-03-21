using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SkyProjectile : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float warningDuration;
    [SerializeField] private float projectileFallDuration;
    [SerializeField] private int damage; 
    [SerializeField] private int knockbackForce;
    private float currentWarningDuration;
    private float currentProjectileFallDuration;

    public GameObject projectile;
    private Vector3 projectileStartPos;

    private bool hasExploded;

    private DecalProjector projector;
    public VisualEffect explotion;

    private void Awake()
    {
        projectileStartPos = projectile.transform.position;
        projector = GetComponentInChildren<DecalProjector>();
    }
    private void OnEnable()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1 << 6))
        {
            transform.position = hit.point;
        }

        projectile.transform.position = new Vector3(projectile.transform.position.x, projectile.transform.position.y + 10, projectile.transform.position.z);
        projector.gameObject.SetActive(true);
        projectile.SetActive(true);
        hasExploded = false;
        currentWarningDuration = warningDuration;
        currentProjectileFallDuration = projectileFallDuration;
        Vector3 size = Vector3.one * radius;
        size.z = 5;
        projector.size = size;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down);

        if (currentWarningDuration > 0)
        {
            currentWarningDuration -= Time.deltaTime;
        }
        else if(currentProjectileFallDuration > 0)
        {
            currentProjectileFallDuration -= Time.deltaTime;

            projectile.transform.position = Vector3.Lerp(transform.position, projectileStartPos, currentProjectileFallDuration / projectileFallDuration);
        }
        else if(hasExploded == false)
        {
            hasExploded = true;
            Explode();
        }
    }

    private void Explode()
    {
        projector.gameObject.SetActive(false);
        projectile.SetActive(false);
        projectile.transform.position = projectileStartPos;
        explotion.Play();
        Collider[] hits = Physics.OverlapSphere(transform.position, radius/2);
        foreach (Collider hit in hits)
        {
            if (hit.tag == "Player")
            {
                Vector3 direction = (hit.transform.position - transform.position);
                direction.y = 0.1f;
                direction.Normalize();
                hit.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                hit.transform.GetComponent<Health>().Apply(-damage);
            }
        }
        StartCoroutine(temp());
    }

    IEnumerator temp()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
