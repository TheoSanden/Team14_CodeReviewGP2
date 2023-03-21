using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    public class Spawn : Ability
    {
        public float cooldown;
        private float currentCooldown;
        [Header("Data")]
        public int TanksToSpawn;
        public int TroopsToSpawn;
        public float spawnDuration = 5;
        public Vector2 spawnCircle;

        [Header("Prefabs")]
        public GameObject TroopPrefab;
        public GameObject TankPrefab;

        private int objectsToSpawn;

        public override event Action DonePerforming;
        public override bool IsAvailable() => currentCooldown <= 0;


        public override void Perform()
        {
            objectsToSpawn = TanksToSpawn + TroopsToSpawn;
            currentCooldown = cooldown;
            StartCoroutine(SpawnRoutine());
        }

        private void Update()
        {
            if(currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        private void SpawnEnemy()
        {
            //get a random position around the boss
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle;
            randomDirection.y = 0;
            Vector3 randomPosition = transform.position + (randomDirection.normalized * UnityEngine.Random.Range(spawnCircle.x, spawnCircle.y));


            if (objectsToSpawn > TanksToSpawn)
            {
                //spawn troop
                EnemyManager.instance.SpawnEnemy(TroopPrefab, randomPosition);
                return;
            }

            //spawn tank
            EnemyManager.instance.SpawnEnemy(TankPrefab, randomPosition);

        }

        IEnumerator SpawnRoutine()
        {
            int totalObjects = objectsToSpawn;
            while(objectsToSpawn > 0)
            {
                SpawnEnemy();
                objectsToSpawn--;
                
                yield return new WaitForSeconds(spawnDuration / totalObjects);
            }

            DonePerforming?.Invoke();
        }
}
}
