using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class Shield : Ability
    {
        public float cooldown;
        public float currentCooldown;

        public int shieldAmount;

        public GameObject ShieldFX;

        private ShieldHealth health;

        private void Awake()
        {
            health = GetComponent<ShieldHealth>();
        }

        public override event Action DonePerforming;
        public override bool IsAvailable() => currentCooldown <= 0;

        public override void Perform()
        {
            currentCooldown = cooldown;
            ShieldFX.SetActive(true);
            health.ModifyShield(shieldAmount);
            DonePerforming?.Invoke();
        }

        private void Update()
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }else 
            {
                ShieldFX.SetActive(false);
            }
        }
}}