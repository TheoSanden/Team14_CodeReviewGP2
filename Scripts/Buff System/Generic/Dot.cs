using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "New Dot", menuName = "Buff/Dot", order = 0)]
    public class Dot : Buff
    {
        [SerializeField] protected float tickFrequency;
        [SerializeField] protected int tickAmount;
        [SerializeField] protected Color tickDamageUIColor = Color.white;
        protected ShieldHealth health;
        protected float tickTimer;

        public override void OnBuffApplied()
        {
            base.OnBuffApplied();
            tickTimer = tickFrequency;
        }
        public override void Update()
        {
            if (!initialized) return;
            if (tickTimer >= tickFrequency) { Tick(); tickTimer = 0; }
            tickTimer += Time.deltaTime;
        }
        public virtual void Tick()
        {
            Debug.Log(health);
            health.Apply(-tickAmount, tickDamageUIColor);
        }
        public virtual void Initialize(GameObject go, ShieldHealth health)
        {
            base.Initialize(go);
            this.health = health;
            initialized = true;
        }
    }
}