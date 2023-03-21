using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBehaviour
{
    EnemyMovement move;
    FloatVariable variable;
    protected override void Setup()
    {
        move = GetComponent<EnemyMovement>();
        move.atDestination += Attack;
        move.NavitageToTarget(target.transform);
    }

    void Attack()
    {
        anim.PlayTargetAnimation("Attack", true);
    }

    protected override void OnInteractChange()
    {
        move.NavitageToTarget(HelperSenses.FindClosestTarget(transform));
    }
    public void HandleDeath()
    {
        Destroy(this.gameObject);
    }
}
