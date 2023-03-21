using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : EnemyBehaviour
{
    EnemyMovement move;

    [SerializeField] private float retreatDistance;
    [SerializeField] private float cooldownDuration;
    [SerializeField] private int shootBurst;
    [SerializeField] private float ShootBurstTime;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField]  private float cooldown;

    [SerializeField] private AudioSource tankMan;
    [SerializeField] private AudioClip tankShoot;
    [SerializeField] private AudioClip tankDeath;
    [SerializeField] private int rotationSpeed;

    [Header("Data for moving. not used for now")]
    [SerializeField] private float searchRadius;
    [SerializeField] private int searchIterations;

    // Flags
    private bool canPerformAction = true;

    protected override void Setup()
    {
        move = GetComponent<EnemyMovement>();
        move.atDestination += HandleAI;
    }

    protected override void OnUpdate()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            canPerformAction = true;

            // calculate the Quaternion for the rotation
            Quaternion rot = Quaternion.LookRotation(target.transform.position - transform.position);

            //Apply the rotation 
          //  transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);

        }
        else if(canPerformAction == true)
        {
            HandleAI();
        }

        
    }

    void Attack()
    {
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        for (int i = 0; i < shootBurst; i++)
        {
            tankMan.PlayOneShot(tankShoot);

            anim.PlayTargetAnimation("Attack", true);
            Instantiate(projectilePrefab, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(ShootBurstTime);
        }
    }

    protected override void OnInteractChange()
    {
        cooldown = cooldownDuration;
        canPerformAction = true;
    }

    void HandleAI()
    {
        canPerformAction = false;
        // check distance to players
        /* Transform closestPlayer = HelperSenses.FindClosestTarget(transform);
         float distance = Vector3.Distance(closestPlayer.position, transform.position);

         // if no player is close attack again
         if (distance > retreatDistance)
         {
             target = closestPlayer.gameObject;
             Attack();
             return;
         }*/
        target = HelperSenses.FindClosestTarget(transform).gameObject;
        Attack();
       // Debug.Log("run!");

        // player(s) are a close, find new location and walk there
       // move.NavitageToTarget(GetNewPosition(searchRadius, searchIterations));
        //move.NavitageToTarget(Vector3.zero);
    }

    private Vector3 GetNewPosition(float radius, int iterations)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;

        Vector3 finalPosition = transform.position + randomDirection;
        return finalPosition;
    }

    public void HandleDeath()
    {

        tankMan.PlayOneShot(tankDeath);
        Destroy(this.gameObject);

    }
}
