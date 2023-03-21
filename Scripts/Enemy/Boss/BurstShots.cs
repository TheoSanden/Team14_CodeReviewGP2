using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boss
{


    public class BurstShots : Ability
    {
        [Header("Stats")]
        public float duration;
        public GameObject projectilePrefab;

        GameObject[] players;
        int shotCount;
        private SpiderAnimatorController anim;

        private void Awake()
        {
            anim = GetComponentInChildren<SpiderAnimatorController>();
            anim.shootSkyProjectile += Shoot;
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        public override event Action DonePerforming;
        public override bool IsAvailable()
        {
            return true;

        }

        public override void Perform()
        {
            anim.PlayTargetAnimation("Barage");
            StartCoroutine(Duration());
        }

        public void Shoot()
        {
            shotCount++;
            Debug.Log(shotCount % players.Length);

            Transform target = players[shotCount % players.Length].transform;
            Instantiate(projectilePrefab, target.position, Quaternion.identity);
        }

        IEnumerator Duration()
        {
            yield return new WaitForSeconds(duration);
            DonePerforming?.Invoke();
        }

    }
}
