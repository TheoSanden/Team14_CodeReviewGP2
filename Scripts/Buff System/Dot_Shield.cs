using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffs
{
    [CreateAssetMenu(fileName = "New Shield Dot", menuName = "Buff/Shield Dot", order = 0)]
    public class Dot_Shield : Dot
    {
        [SerializeField] int shieldTickAmount;
        public override void Tick()
        {
            if (!initialized) return;
            if (health.ShieldBuffer > 0)
            {
                health.DamageShield(shieldTickAmount);
            }
            else
            {
                base.Tick();
            }
        }
    }
}
