using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class HealthBar : MonoBehaviour
    {
        public Image HealthFill;
        public Image shieldFill;

        private int maxShield, maxHealth;
        public IntVariable shield, health;

        public ShieldHealth healthScript;
        private void Start()
        {
            maxHealth = health.Value;
            maxShield = shield.Value;
        }
        public void SetHealth(float amount)
        {
            HealthFill.fillAmount = amount / maxHealth;
        }
        public void SetShield(float amount)
        {
            shieldFill.fillAmount = (float)(healthScript.ShieldBuffer / maxShield);
        }
    }

}