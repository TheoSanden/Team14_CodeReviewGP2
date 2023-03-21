using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class AOEShockWave : Ability
    {
        [Header("Data")]
        public float windupDuration;
        public float shockwaveRadius;
        public float shockwaveKnockback;
        public int shockwaveDamage;

        public Indicator ShockwaveIndicator;
        private SpiderAnimatorController anim;

        public override event Action DonePerforming;
        public GameObject orb;

        private void Awake()
        {
            anim = GetComponentInChildren<SpiderAnimatorController>();
        }

        public override bool IsAvailable()
        {
            Transform target = HelperSenses.FindClosestTarget(transform);

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > shockwaveRadius/2)
            {
                // they're to far
                return false;
            }
            return true;
        }

        public override void Perform() 
        {
            ShockwaveIndicator.SetRadius(shockwaveRadius);
            ShockwaveIndicator.Display(true);
            anim.PlayTargetAnimation("orb");
            StartCoroutine(ShockwaveRoutine());
        }

        IEnumerator ShockwaveRoutine()
        {
            yield return new WaitForSeconds(windupDuration);
            ShockwaveIndicator.Display(false);
            anim.PlayTargetAnimation("orbExplosion");


            Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveRadius/2);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    hit.GetComponent<Health>().Apply(-shockwaveDamage);
                }
            }
            yield return new WaitForSeconds(1);
            DonePerforming?.Invoke();
            orb.SetActive(false);
        }

    }
}