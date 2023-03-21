using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    int currentEnemyAmount;

    public GameObject projectilePrefab;
    public int projectilePoolAmount;
    private Projectile[] projectilePool;
    int lastProjectileIndex;

    public static EnemyManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        projectilePool = new Projectile[projectilePoolAmount];
    }

    private void Start()
    {
        SpawnProjectilePool();
    }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
    {
        GameObject go = Instantiate(enemyPrefab, position, Quaternion.identity);
        if (EnemyUiManager.Instance != null)
        {
            EnemyUiManager.Instance.SubscribeGameObject(go, true, true, true);
        }
        else
        {
            Debug.LogWarning("No instance EnemyUiManager found", this.gameObject);
        }
    }

    public void ShootProjectile(Vector3 startPos, Vector3 direction, ProjectileData data)
    {
        Projectile p = GetProjectile();
        if (p == null)
        {
            Debug.LogWarning("No unused projectile. try adding more to the pool");
            return;
        }
        p.transform.position = startPos;
        p.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        p.Perform(data);
    }

    private Projectile GetProjectile()
    {
        int p = lastProjectileIndex;
        for (int i = 0; i < projectilePool.Length; i++)
        {
            p++;
            if (p >= projectilePool.Length)
                p = 0;

            if (projectilePool[p].isUsed == false)
            {
                lastProjectileIndex = p;
                return projectilePool[p];
            }
        }
        return null;
    }

    private void SpawnProjectilePool()
    {

        for (int i = 0; i < projectilePoolAmount; i++)
        {
            projectilePool[i] = Instantiate(projectilePrefab, transform).GetComponent<Projectile>();
            projectilePool[i].gameObject.SetActive(false);
        }
    }
}
