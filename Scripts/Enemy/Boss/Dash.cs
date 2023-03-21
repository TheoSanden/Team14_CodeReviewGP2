using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class Dash : Ability
    {
        [Header("Stats")]
        public float WindupDuration;
        public float DashDistance;
        public float DashSpeed;
        public int Damage;
        public float StopDistance;

        [Header("Availability Stats")]
        public float closestDistance;


        private float currentWindup = 0;

        Transform target;
        private bool isDashing;

        public override event Action DonePerforming;
        public override bool IsAvailable()
        {
            return true;
            target = HelperSenses.FindClosestTarget(transform);

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < closestDistance)
            {
                // they're to close
                return false;
            }
            if(distance > DashDistance)
            {
                // they're to far
                return false;
            }
            return true;
        }

        public override void Perform()
        {
            // set defaults
            currentWindup = 0;
            target = HelperSenses.FindClosestTarget(transform);

            StartCoroutine(DashRoutine());
        }

        IEnumerator DashRoutine()
        {

            #region Windup
            Quaternion startRotation = transform.rotation;
            Quaternion finalRotation = Quaternion.LookRotation(target.position - transform.position);
            while (currentWindup < WindupDuration)
            {
                Vector3 targetRotation = Quaternion.Lerp(
                    startRotation,
                    finalRotation,
                    currentWindup / WindupDuration
                    ).eulerAngles;

                targetRotation = new Vector3(0, targetRotation.y, 0);
               // targetRotation = new Vector3(transform.eulerAngles.x, targetRotation.y, transform.eulerAngles.z);
                transform.eulerAngles = targetRotation;

                currentWindup += Time.deltaTime;
                yield return null;
            }

            #endregion

            #region Dash
            isDashing = true;

            Vector3 finalPosition = transform.position + (transform.forward * DashDistance);
            finalPosition.y = transform.position.y;
            Vector3 deltaPosition = transform.position;
            // While not at destination
            while(Vector3.Distance(transform.position, finalPosition) > 0.3f)
            {
                if(Physics.BoxCast(transform.position + (transform.forward * transform.localScale.z), transform.localScale, transform.forward, Quaternion.identity, StopDistance, 1<<6))
                {
                    break;
                }
                transform.position = Vector3.Lerp(transform.position, finalPosition, DashSpeed * Time.deltaTime);

                deltaPosition = transform.position;
                yield return null;
            }

            #endregion
            isDashing = false;
            DonePerforming?.Invoke();

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isDashing) return;
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<Health>().Apply(-Damage);

            }
        }
    }
}
