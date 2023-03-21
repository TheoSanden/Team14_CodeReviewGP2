using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpiderLeg : MonoBehaviour
{
    public AudioSource source;
    public AudioClip footStep;
    public float stepDistance;
    public float stepSpeed;
    public float stepHeight;

    public Transform IKtarget;
    private Vector3 IKtargetPos;
    private Vector3 IKtargetOldPos;

    public Transform target;

    public SpiderAnimatorController controller;

    [SerializeField] private Vector3 movementDirection;
    private Vector3 lastFramePos;
    float lerp;

    public bool isMoving;
    private void Start()
    {
        IKtargetPos = IKtarget.position;
        target.position = IKtarget.position;
    }

    void Update()
    {
        //get velocity
        if (transform.position != lastFramePos)
        {
            movementDirection = (transform.position - lastFramePos);
            movementDirection.y = 0;
            movementDirection.Normalize();
        }

        //check distance
        if (Vector3.Distance(IKtargetPos, target.position) > stepDistance)
        {
            if (controller.AllowedToMove())
            {
                lerp = 0;
                IKtargetOldPos = IKtargetPos;
                RaycastHit hit;
                Physics.Raycast(target.position + (movementDirection * stepDistance) + (Vector3.up * 1f), Vector3.down, out hit, 10);
                IKtargetPos = hit.point;

                if(isMoving == false)
                {
                    source.PlayOneShot(footStep);
                }
                isMoving = true;
            }

        }
        //done moving leg
        if (lerp > 1)
        {
            IKtarget.position = IKtargetPos;
            isMoving = false;

        }
        //move leg
        if (isMoving)
        {
            Vector3 footPosition = Vector3.Lerp(IKtargetOldPos, IKtargetPos, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            IKtarget.position = footPosition;
            lerp += Time.deltaTime * stepSpeed;
        }



        lastFramePos = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.position + (movementDirection * stepDistance), .3f);
    }
}
