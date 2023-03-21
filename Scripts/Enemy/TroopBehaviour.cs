using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopBehaviour : EnemyBehaviour
{
    EnemyMovement move;
    public int damage;
    public ProjectileData projectile;
    [SerializeField] Vector3 shootPosition;
    [SerializeField] Transform meshTransform;
    [SerializeField] private AudioSource soldier;
    [SerializeField] private AudioClip soldierAttack;
    [SerializeField] private AudioClip soldierDeath;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetShootPosition(), 0.2f);
    }
    protected override void Setup()
    {
        move = GetComponent<EnemyMovement>();
        move.atDestination += Attack;
    }
    void Attack()
    {
        if (anim.GetIsInteracting()) return;
        target = HelperSenses.FindClosestTarget(transform).gameObject;
        anim.PlayTargetAnimation("Attack", true);
        Vector3 direction = -(transform.position - target.transform.position);
        direction.y = 0;
        direction.Normalize();
        EnemyManager.instance.ShootProjectile(GetShootPosition(), direction, projectile);
        soldier.PlayOneShot(soldierAttack);
    }

    protected override void OnInteractChange()
    {
        target = HelperSenses.FindClosestTarget(transform).gameObject;
        move.NavitageToTarget(target.transform);
    }
    public void HandleDeath()
    {
        soldier.PlayOneShot(soldierDeath);
        Destroy(this.gameObject);
    }

    protected override void StartAggro()
    {
        move.NavitageToTarget(HelperSenses.FindClosestTarget(transform));
    }
    private Vector3 GetShootPosition()
    {
        Transform combinedTransform = transform;
        combinedTransform.rotation = meshTransform.rotation;
        return combinedTransform.localToWorldMatrix.MultiplyPoint(shootPosition);
    }
}
