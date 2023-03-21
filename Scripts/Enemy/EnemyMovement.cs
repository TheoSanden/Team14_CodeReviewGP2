using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    // States
    bool canMove;
    bool isNavigating;

    // data
    Transform target;
    Vector3 targetPosition;
    Vector3 velocity;

    // components
    NavMeshAgent agent;
    Rigidbody rb;

    // events
    public Action atDestination;
    Animator anim;

    // public vars
    public float stoppingDistance = 3;
    public bool onlyStopIfSeeingTarget;
    public LayerMask obscuringLayers;
    public float movementSpeed = 4;
    [Tooltip("max controllable velocity")] public float controllableVelocity = 4;
    //
    public Transform bodyMesh;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        agent = GetComponentInChildren<NavMeshAgent>();
        if(agent == null)
        {
            GameObject newAgent = new GameObject();
            newAgent.transform.parent = transform;
            newAgent.transform.name = "Navigation Agent";
            agent = newAgent.AddComponent<NavMeshAgent>();
        }
        agent.speed = movementSpeed;
        anim = GetComponentInChildren<Animator>();
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    #region Navigation
    // Handles the Navigation (called every frame)
    void HandleNavigation()
    {
        // If currently navigating
        if (isNavigating)
        {
            if (target != null) targetPosition = target.position;

            ///
            ///                         Calulate direction to walk in
            /// I'm using an agent on another object because i dont want the agent to apply my movement
            /// I want to move with a rigidbody because its easier to apply things like knockback
            ///
            // Make agent move towards target
            agent.SetDestination(targetPosition);
            // Get direction to move in
            Vector3 moveDirection = agent.desiredVelocity;
            moveDirection.Normalize();
            // Reset agent position for calculating direction again next frame
            agent.transform.position = transform.position;
            // Apply movement to me
            velocity += moveDirection * movementSpeed;

            // Check if reached destination
            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
            {
                // If there is something between this and target
                if(Physics.Linecast(transform.position, targetPosition, obscuringLayers))
                {
                    Debug.Log("cant see");
                  //  return;
                }

                atDestination?.Invoke();
                StopNavigation();
            }
        }
    }
    /// <summary>
    /// Start Navigating towards target.
    /// Stops when within stopping distance of target
    /// </summary>
    /// <param name="newTarget">transform of target</param>
    public void NavitageToTarget(Transform newTarget)
    {
        target = newTarget;
        isNavigating = true;
    }
    public void NavitageToTarget(Vector3 newTarget)
    {
        target = null;
        targetPosition = newTarget;
        isNavigating = true;
    }

    /// <summary>
    /// Stop and reset Navigation
    /// </summary>
    public void StopNavigation()
    {
        agent.ResetPath();
        isNavigating = false;
    }
    #endregion

    // Called every Fixed Update
    void HandleMovement()
    {
        // uncontrollable velocity
        if(rb.velocity.magnitude > controllableVelocity)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, Time.fixedDeltaTime);
            return;
        }

        rb.velocity = velocity;
    }

    private void Update()
    {
        // We're reseting the desiredVelocity at the start of everyframe so we can add to it
        velocity = Vector3.zero;

        HandleNavigation();
        HandleRotation();
        

        anim.SetBool("isWalking", isNavigating);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleRotation()
    {
        if (target == null) return;
        

        // calculate the Quaternion for the rotation
        Quaternion rot = Quaternion.LookRotation(target.position - bodyMesh.position);

        //Apply the rotation 
        bodyMesh.rotation = rot;
        // put 0 on the axys you do not want for the rotation object to rotate
        bodyMesh.eulerAngles = new Vector3(0, bodyMesh.eulerAngles.y + -90, 0);
    }
}
